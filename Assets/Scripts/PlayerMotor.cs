using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : Ivyyy.PlayerMovement2D
{
	public Animator animator;
	public uint playerId = 0;
	private PlayerInputActions playerInputActions;

	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
	}

	private void Update()
	{
		Vector2 input = playerInputActions.Player.Movement.ReadValue <Vector2>();
		Move (input);
		animator.SetFloat ("Horizontal", input.x);
		animator.SetFloat ("Vertical", input.y);
		animator.SetFloat ("Speed", input.sqrMagnitude);
	}
}
