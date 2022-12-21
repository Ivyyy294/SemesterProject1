using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
	public GameObject playerSetupMenuPrefab;
	public PlayerInput input;

	private GameObject menu;

	public void Show()
	{
		var rootMenu = GameObject.Find ("PlayerJoinLayout");

		if (rootMenu != null)
		{
			menu = Instantiate (playerSetupMenuPrefab, rootMenu.transform);
			input.uiInputModule = menu.GetComponentInChildren <InputSystemUIInputModule>();
			menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex (input.playerIndex);
		}
	}
}
