using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
	//Private values
	private int playerIndex;
	private float ingnoreInputTime = 0.5f;
	private float timer = 0f;
	private bool inputEnabled;

	//Editor values
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] GameObject readyPanel;
	[SerializeField] GameObject menuPanel;
	[SerializeField] Button readyButton;

	public void SetPlayerIndex (int index)
	{
		playerIndex = index;
		titleText.SetText ("Player " + (index + 1).ToString());
		timer = 0f;
	}

    // Update is called once per frame
    void Update()
    {
        if (timer > ingnoreInputTime)
			inputEnabled = true;
		else
			timer += Time.deltaTime;
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
}
