using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerPauseMenu : MonoBehaviour
{
	[SerializeField] GameObject menu;
	[SerializeField] GameObject text;
	[SerializeField] float fourPlayerUiScale = 0.75f;

	//Public Functions
	public void Show ()
	{
		if (PlayerManager.Me.MaxPlayers == 4)
			transform.localScale = new Vector3 (fourPlayerUiScale, fourPlayerUiScale);

		//Setting the uiInputModule
		PlayerInput input = GetComponentInParent <PlayerInput>();

		if (input != null)
			input.uiInputModule = menu.GetComponentInChildren <InputSystemUIInputModule>();

		SetMenu (true);
	}

    public void ButtonContinue()
	{
		SetMenu (false);
	}

	public void ButtonMenu()
	{
		if (GameStatus.Me != null)
		{
			Time.timeScale = 1f;
			GameStatus.Me.GoToStatsScreen (-1);
		}
		else
			Debug.Log ("Missing GameStatus!");
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	//Private Functions
	void SetMenu (bool val)
	{
		if (GameStatus.Me != null)
		{
			GameStatus.Me.SetPause (val);
			menu.SetActive (val);
		}
		else
			Debug.Log("Missing GameStatus");
	}

	private void Update()
	{
		if (GameStatus.Me != null)
		{
			//Shows the text when the game is paused by another player
			bool textVisible = GameStatus.Me.GamePaused && !menu.activeInHierarchy;
			text.SetActive (textVisible);
		}
	}
}
