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

	[Header ("Lara Values")]
	[SerializeField] Animator animator;
	[SerializeField] PlayerInteraktions interactionScript;

	//Private Values
	private SpeedProfile speedProfileLightWare;
	private PlayerConfiguration playerConfiguration;
	//private PlayerInput input;
	private InputAction moveAction;

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
		if (moveAction != null)
		{
			SetCurrentPlayerSpeed();

			//Call Move from PlayerMovement2D
			Vector2 movementVec = moveAction.ReadValue <Vector2>();
			Move (movementVec);

			//Update Animator Values
			animator.SetFloat ("Horizontal", movementVec.x);
			animator.SetFloat ("Vertical", movementVec.y);
			animator.SetFloat ("Speed", movementVec.sqrMagnitude);
		}
	}

	//private void InitInput()
	//{
	//	input = GetComponent <PlayerInput>();
	//	moveAction = input.actions["Movement"];
	//}

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
}
