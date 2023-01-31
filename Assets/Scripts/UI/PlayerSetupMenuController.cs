using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
	//Private values
	private int playerIndex;
	private float ingnoreInputTime = 0.5f;
	private float timer = 0f;
	private bool inputEnabled;

	//Editor values
	[Header ("Lara values")]
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] GameObject readyPanel;
	[SerializeField] GameObject menuPanel;
	[SerializeField] Button readyButton;

	//Private
	InputAction cancel;

	public void SetPlayerIndex (int index)
	{
		playerIndex = index;
		titleText.SetText ("Player " + (index + 1).ToString());
		timer = 0f;

		if (PlayerManager.Me != null)
		{
			PlayerInput input = PlayerManager.Me.GetPlayerConfigs()[playerIndex].playerConfiguration.Input;
			
			if (input != null)
				cancel = input.actions["Cancel"];
		}

	}

	public void SetTeam (int team)
	{
		if (!inputEnabled)
			return;

		PlayerManager.Me.SetPlayerTeam (playerIndex, team);
		readyPanel.SetActive (true);
		readyButton.Select();
		menuPanel.SetActive (false);
	}

	public void ReadyPlayer()
	{
		if (!inputEnabled)
			return;

		PlayerManager.Me.ReadyPlayer (playerIndex);
		readyButton.gameObject.SetActive (false);
	}

	void Update()
    {
        if (timer > ingnoreInputTime)
			inputEnabled = true;
		else
			timer += Time.deltaTime;

		if (cancel != null && cancel.WasPressedThisFrame() && MapManager.Me != null)
			MapManager.Me.LoadMap (Map.Menu);
    }
}
