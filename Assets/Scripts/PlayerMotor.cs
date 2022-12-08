using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : Ivyyy.PlayerMovement2D
{
	private PlayerInputActions playerInputActions;

	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
	}

	private void Update()
	{
		Move (playerInputActions.Player.Movement.ReadValue <Vector2>());
	}
}
