using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : Ivyyy.PlayerMovement2D
{
	//Editor Values
	[Header ("Additional Settings")]
	[SerializeField] SpeedProfile speedProfileMediumWare;
	[SerializeField] SpeedProfile speedProfileHeavyWare;

	[Header ("Collision Penalty")]
	[Range (0f, 1f)]
	[SerializeField] float collisionAccelerationPenalty = 1f;
	[SerializeField] float collisionPenaltyThreshold = 0f;

	[Header ("Lara Values")]
	[SerializeField] Animator animator;
	[SerializeField] PlayerInteraktions interactionScript;

	//Private Values
	private SpeedProfile speedProfileLightWare;
	private PlayerConfiguration playerConfiguration;
	//private PlayerInput input;
	private InputAction moveAction;
	private bool collisionTimerRunning = false;
	private float collisionTimer;

	//Public Functions
	public void InitPlayer (PlayerConfiguration pc)
	{
		playerConfiguration = pc;
		moveAction = playerConfiguration.Input.actions["Movement"];

		interactionScript.InitInput (pc);
	}

	protected override void Start()
	{
		base.Start();
		speedProfileLightWare = speedProfile;
	}

	private void Update()
	{
		if (GameStatus.Me != null && !GameStatus.Me.GamePaused)
		{
			if (collisionTimerRunning && collisionAccelerationPenalty > 0f)
			{
				if (collisionTimer >= collisionPenaltyThreshold)
				{
					timeAcceleration *= 1 - collisionAccelerationPenalty;
					collisionTimerRunning = false;
				}
				else
					collisionTimer += Time.deltaTime;
			}
			else
			{
				collisionTimer = 0f;
			}

			if (moveAction != null)
			{
				SetCurrentPlayerSpeed();

				//Call Move from PlayerMovement2D
				Vector2 movementVec = moveAction.ReadValue <Vector2>();
				Move (movementVec);

				UpdateAnimator(movementVec);
			}
		}
	}

	private void SetCurrentPlayerSpeed ()
	{
		if (interactionScript != null && interactionScript.CarriesWare())
		{
			Ware ware = interactionScript.GetCarriedWare();

			if (ware != null)
			{
				switch (ware.weight)
				{
					case Ware.WeightCategory.Light:
						currentSpeedProfile = speedProfileLightWare;
						break;
					case Ware.WeightCategory.Medium:
						currentSpeedProfile = speedProfileMediumWare;
						break;
					case Ware.WeightCategory.Heavy:
						currentSpeedProfile = speedProfileHeavyWare;
						break;
				}
			}
			else
				currentSpeedProfile = speedProfileLightWare;
		}
		else
			currentSpeedProfile = speedProfileLightWare;
	}

	//Resets the acceleration when a player bumbs into another player
	void OnCollisionEnter2D (Collision2D collision)
	{
		collisionTimerRunning = true;
	}

	//Resets the acceleration when a player bumbs into another player
	void OnCollisionExit2D (Collision2D collision)
	{
		collisionTimerRunning = false;
	}

	private void UpdateAnimator (Vector2 movementVec)
	{
		//Factor is the current player speed relative to maxspeed when not carrying an item
		float factor = currentSpeedProfile.maxSpeed / speedProfileLightWare.maxSpeed;
		factor *= GetCurrentSpeedFactor();

		//Update Animator Values
		animator.SetFloat ("Speed", factor);

		//This saves the last direction for the animatior, to play the correct idl animation
		if (factor > 0f)
		{
			animator.SetFloat ("Horizontal", movementVec.x);
			animator.SetFloat ("Vertical", movementVec.y);
		}
	}
}
