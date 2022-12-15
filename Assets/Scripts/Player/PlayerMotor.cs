using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : Ivyyy.PlayerMovement2D
{
	public Animator animator;
	public uint playerId = 0;
	private PlayerInput input;
	private InputAction moveAction;

	private void Update()
	{
		if (input == null)
			InitInput();

		Vector2 movementVec = moveAction.ReadValue <Vector2>();
		Move (movementVec);
		animator.SetFloat ("Horizontal", movementVec.x);
		animator.SetFloat ("Vertical", movementVec.y);
		animator.SetFloat ("Speed", movementVec.sqrMagnitude);
	}

	private void InitInput()
	{
		input = GetComponent <PlayerInput>();
		moveAction = input.actions["Movement"];
	}
}
