using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Team
{
	public float Reputation {set;get;}
	public float SilverCoins {set;get;}
	public List <uint> playerIds = new List<uint>();
}

public class GameStatus : MonoBehaviour
{
	//Public Values
	static public GameStatus Me {get; private set; }
	[SerializeField] float dayLenght = 60f;

	//Private Values
	List <Team> teams;
	int currentDay = 0;
	string currentHour;
	string currentMinute;
	float lifeTime = 0.0f;

	//Public Functions
	public void AddPlayerToTeam (uint playerId, int teamId)
	{
		if (teamId < teams.Count)
			teams[teamId].playerIds.Add (playerId);
		else
		{
			Team t = new Team();
			t.playerIds.Add (playerId);
			teams.Add (t);
		}
	}

	public void AddReputation (uint playerId, float val)
	{
		foreach (Team i in teams)
		{
			if (i.playerIds.Contains (playerId))
			{
				i.Reputation += val;
				return;
			}
		}
	}

	public void AddSilverCoins (uint playerId, float val)
	{
		foreach (Team i in teams)
		{
			if (i.playerIds.Contains (playerId))
			{
				i.SilverCoins += val;
				return;
			}
		}
	}

	//Private Functions

    // Start is called before the first frame update
    void Awake ()
    {
        if (Me == null)
		{
			Me = this;
			DontDestroyOnLoad (Me);
			teams = new List<Team>();
			AddPlayerToTeam (0,0);
		}
		else
			Debug.Log ("Trying to create a new Singelton instance!");
    }

	void CalculateDayTime ()
	{
		int rawDay = Mathf.FloorToInt (lifeTime / dayLenght);
		currentDay = 1 + rawDay;

		float rawTime = lifeTime - dayLenght * rawDay;
		float rawHour = 24 * rawTime / dayLenght;

		currentHour = Mathf.FloorToInt (rawHour).ToString();

		float rawMinute = rawHour % 1f * 60f;
		currentMinute = Mathf.FloorToInt (rawMinute).ToString();

		if (rawMinute < 10f)
			currentMinute = "0" + currentMinute;
	}

	// Update is called once per frame
	void Update()
    {
		lifeTime += Time.deltaTime;
		CalculateDayTime();
    }

	void OnGUI()
	{
		if (teams.Count > 0)
		{
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.skin.label.normal.textColor = Color.black;

			GUILayout.BeginArea(new Rect (10, 10, 150, 100));
			GUILayout.Label ("Day: " + currentDay.ToString() + " Time: " + currentHour + ":" + currentMinute);
			GUILayout.Label ("Reputation: " + teams[0].Reputation.ToString());
			GUILayout.Label ("Silver: " + teams[0].SilverCoins.ToString());
			GUILayout.EndArea();
		}
	}
}
