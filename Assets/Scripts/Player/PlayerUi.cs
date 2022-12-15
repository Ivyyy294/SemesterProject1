using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUi : MonoBehaviour
{
    //Private Values
	PlayerInput input;
	uint playerId;

	//Private Functions
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
			InitInput();
    }

	private void OnGUI()
	{
		Team team = GameStatus.Me.GetTeamForPlayer (playerId);
		GameDateTime dateTime = GameStatus.Me.GetCurrentDateTime ();

		GUI.skin.label.fontStyle = FontStyle.Bold;
		GUI.skin.label.fontSize = 24;
		GUI.skin.label.normal.textColor = Color.black;

		Vector3 pos = input.camera.ViewportToScreenPoint (new Vector3 (0.01f, 0f));

		GUILayout.BeginArea(new Rect (pos.x, pos.y, 500, 500));
		GUILayout.Label ("Day: " + dateTime.day + " Time: " + dateTime.hour + ":" + dateTime.minute);

		GUILayout.Label ("Reputation: " + team.Reputation.ToString());
		GUILayout.Label ("Silver: " + team.SilverCoins.ToString());

		GUILayout.EndArea();
	}

	private void InitInput()
	{
		input = GetComponent <PlayerInput>();

		if (input != null)
			playerId = (uint) input.playerIndex;
	}
}
