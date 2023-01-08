using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDateTime
{
	public int day = 0;
	public string hour;
	public string minute;
}

public class Team
{
	//Public Values
	public int Id;
	public float Reputation {private set;get;}
	public float SilverCoins {private set;get;}
	public float SilverCoinsEarned {private set; get;}
	public List <uint> playerIds = new List<uint>();
	
	//Private Values
	private float timeSinceRequest = 0f;
	private float timeSincePassiveReputationLoss;
	private uint numberOfTaxes;
	private bool taxCollected = false;
	GameStatus gameStatus;

	//Public Functions
	public Team()
	{
		Reputation = 50f;
		SilverCoins = 0f;

		gameStatus = GameStatus.Me;
		timeSincePassiveReputationLoss = gameStatus.passiveReputationLossInterval;
	}

	public void ReputationGain (float requestTime)
	{
		Reputation += (1 + (2 - Reputation/50)) * ((requestTime - timeSinceRequest) / 100);
		timeSinceRequest = 0f;
		Ivyyy.AudioHandler.Me.PlayOneShot (gameStatus.audioRepGain);
	}

	public void ReputationLoss()
	{
		Reputation -= 1;
		Ivyyy.AudioHandler.Me.PlayOneShot (gameStatus.audioRepLoss);
		//Reputation -= 1 + Reputation / 50;
	}

	public void AddSilverCoins (float val)
	{
		if (val > 0f)
			SilverCoinsEarned += val;

		SilverCoins += val;
	}

	public void Update()
	{
		if (timeSinceRequest >= gameStatus.passiveReputationLossThreshold)
			PassiveReputationLoss();

		timeSinceRequest += Time.deltaTime;

		if (gameStatus.GetCurrentDateTime().day % gameStatus.cityTaxInterval == 0)
		{
			if (!taxCollected)
			{
				CityTax();
				taxCollected = true;
			}
		}
		else
			taxCollected = false;
	}

	//Private Values

	private void PassiveReputationLoss ()
	{
		if (timeSincePassiveReputationLoss >= gameStatus.passiveReputationLossInterval)
		{
			ReputationLoss();
			timeSincePassiveReputationLoss = 0;
		}
		else
			timeSincePassiveReputationLoss += Time.deltaTime;
	}

	private void CityTax ()
	{
		++numberOfTaxes;
		float tax = 5 + (2f * numberOfTaxes) + (SilverCoins * 0.1f);

		if (SilverCoinsEarned > 0f)
			tax += ((Mathf.Pow (SilverCoinsEarned, 1.4f) / numberOfTaxes) * 0.1f);

		SilverCoins -= tax;

		Ivyyy.AudioHandler.Me.PlayOneShot (gameStatus.audioTax);
	}
}

public class GameStatus : MonoBehaviour
{
	//Public Values
	static public GameStatus Me {get; private set; }

	[Header ("Player Settings")]
	[SerializeField] Transform[] spawnPoints = new Transform[2];

	[Header ("Game Settings")]
	[SerializeField] float dayLenght = 60f;

	[Header ("Reputation Settings")]
	public float passiveReputationLossThreshold;
	public float passiveReputationLossInterval;
	public AudioClip audioRepGain;
	public AudioClip audioRepLoss;

	[Header ("Tax Settings")]
	public int cityTaxInterval;
	public AudioClip audioTax;

	//Private Values
	List <Team> teams;
	float lifeTime = 0.0f;
	GameDateTime currentDateTime = new GameDateTime();

	//Public Functions
	public void AddPlayerToTeam (uint playerId, int teamId)
	{
		if (teamId < teams.Count)
			teams[teamId].playerIds.Add (playerId);
		else
		{
			Team t = new Team();
			t.Id = teamId;
			t.playerIds.Add (playerId);
			teams.Add (t);
		}
	}

	public void AddReputation (uint playerId, float requestTime)
	{
		Team t = GetTeamForPlayer (playerId);

		if (t != null)
			t.ReputationGain (requestTime);
	}

	public void LossReputation (uint playerId)
	{
		Team t = GetTeamForPlayer (playerId);

		if (t != null)
			t.ReputationLoss ();
	}

	public void AddSilverCoins (uint playerId, float val)
	{
		Team t = GetTeamForPlayer (playerId);

		if (t != null)
			t.AddSilverCoins (val);
	}

	public float GetPlayerMoney (uint playerId)
	{
		Team t = GetTeamForPlayer (playerId);

		if (t != null)
			return t.SilverCoins;

		return 0f;
	}

	public Team GetTeamForPlayer (uint playerId)
	{
		foreach (Team i in teams)
		{
			if (i.playerIds.Contains (playerId))
				return i;
		}

		return null;
	}

	public GameDateTime GetCurrentDateTime () { return currentDateTime;}

	public void Init()
	{
		teams = new List<Team>();
		lifeTime = 0f;

		var playerConfigs = PlayerManager.Me.GetPlayerConfigs().ToArray();

		foreach (PlayerConfigurationDisplay i in playerConfigs)
		{
			PlayerConfiguration pc = i.playerConfiguration;
			Transform pos = spawnPoints[pc.TeamIndex];
			AddPlayerToTeam ( (uint) pc.PlayerIndex, pc.TeamIndex);
			i.SpawnPlayer (pos.position);
		}
	}


	//Private Functions

    // Start is called before the first frame update
    void Awake ()
    {
        if (Me == null)
		{
			Me = this;
			Init();
		}
		else
			Debug.Log ("Trying to create a new Singelton instance!");
    }

	void CalculateDayTime ()
	{
		int rawDay = Mathf.FloorToInt (lifeTime / dayLenght);
		currentDateTime.day = 1 + rawDay;

		float rawTime = lifeTime - dayLenght * rawDay;
		float rawHour = 24 * rawTime / dayLenght;

		currentDateTime.hour = Mathf.FloorToInt (rawHour).ToString();

		float rawMinute = rawHour % 1f * 60f;
		currentDateTime.minute = Mathf.FloorToInt (rawMinute).ToString();

		if (rawMinute < 10f)
			currentDateTime.minute = "0" + currentDateTime.minute;
	}

	// Update is called once per frame
	void Update()
    {
		lifeTime += Time.deltaTime;
		CalculateDayTime();

		foreach (Team i in teams)
			i.Update();
    }

	/* void OnGUI()
	{
		if (teams.Count > 0)
		{
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.skin.label.fontSize = 24;
			GUI.skin.label.normal.textColor = Color.black;

			GUILayout.BeginArea(new Rect (10, 0, 500, 500));
			GUILayout.Label ("Day: " + currentDay.ToString() + " Time: " + currentHour + ":" + currentMinute);

			foreach (Team i in teams)
			{
				GUILayout.Label ("Team " + i.Id.ToString());
				GUILayout.Label ("Reputation: " + i.Reputation.ToString());
				GUILayout.Label ("Silver: " + i.SilverCoins.ToString());
			}

			GUILayout.EndArea();
		}
	} */
}
