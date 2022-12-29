using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUi : MonoBehaviour
{
	//Editor Values
	[Header ("Lara Values")]
	[SerializeField] PlayerInput input;
	[SerializeField] PlayerConfigurationDisplay player;

    //Private Values
	uint playerId;

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (input != null && player != null)
		{
			Team team = GameStatus.Me.GetTeamForPlayer ((uint) player.playerConfiguration.PlayerIndex);
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
	}
}
