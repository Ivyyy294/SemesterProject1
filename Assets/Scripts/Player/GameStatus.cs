using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDateTime
{
	public int day = 0;
	public string hour;
	public string minute;

	public void Init ()
	{
		day = 0;
		hour = "0";
		minute = "00";
	}
}

public class CityTax
{
	//Public Values
	public float TaxToPay {get; private set;}

	//Private Values
	private uint numberOfTaxes;

	public void CalculateTaxToPay (float earnedSilverCoins, float currentSilverCoins)
	{
		int playercount = PlayerManager.Me.MaxPlayers;
		++numberOfTaxes;
		float tax = (5 + (2f * numberOfTaxes) + (currentSilverCoins * 0.1f)) * playercount/2f;

		if (earnedSilverCoins > 0f)
			tax += ((Mathf.Pow (earnedSilverCoins, 1.4f) / numberOfTaxes) * 0.1f);

		TaxToPay = tax;
	}
}

public class Team
{
	//Public Values
	public int Id;
	public float Reputation {private set;get;}
	public float SilverCoins {private set;get;}
	public float SilverCoinsEarned {private set; get;}
	public List <uint> playerIds = new List<uint>();
	public CityTax tax = new CityTax();
	
	//Private Values
	private float timeSinceRequest = 0f;
	private float timeSincePassiveReputationLoss;
	private bool taxCollected = false;
	GameStatus gameStatus;
	PlayerStatsManager statsManager;

	//Public Functions
	public Team(float reputation, float silver)
	{
		Reputation = reputation;
		SilverCoins = silver;
		tax.CalculateTaxToPay(SilverCoinsEarned, SilverCoins);

		gameStatus = GameStatus.Me;
		statsManager = PlayerStatsManager.Me;
		timeSincePassiveReputationLoss = gameStatus.passiveReputationLossInterval;
	}

	public void ReputationGain (float requestTime)
	{
		float gain = (2 + (2 - Reputation/50)) * (((requestTime - timeSinceRequest) * 3) / requestTime);
		Reputation += gain;
		timeSinceRequest = 0f;
		Ivyyy.AudioHandler.Me.PlayOneShot (gameStatus.audioRepGain);

		if (gameStatus.CurrentMarketEvet != null)
			gain *= gameStatus.CurrentMarketEvet.reputationMod;

		foreach (uint i in playerIds)
			statsManager.Stats (i).AddReputationEarned(gain);
	}

	public void ReputationLoss()
	{
		Reputation -= 1;

		foreach (uint i in playerIds)
			statsManager.Stats (i).AddReputationLost(1);
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
		int playercount = PlayerManager.Me.MaxPlayers;

        if (statsManager == null)
			statsManager = PlayerStatsManager.Me;

		if (timeSinceRequest >= gameStatus.passiveReputationLossThreshold * 2f / playercount)
			PassiveReputationLoss();

		timeSinceRequest += Time.deltaTime;

		if (gameStatus.GetCurrentDateTime().day % gameStatus.cityTaxInterval == 0)
		{
			if (!taxCollected)
			{
				CollectCityTax();
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

	private void CollectCityTax ()
	{
		float taxToPay = tax.TaxToPay;

		//Calculate new tax before removing silvercoins
		tax.CalculateTaxToPay (SilverCoinsEarned, SilverCoins);
		SilverCoins -= taxToPay;

		foreach (uint i in playerIds)
			statsManager.Stats (i).TaxPayed += taxToPay;

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
	public float reputationNeededToWin = 100f;
	public float startReputation = 50f;
	public float startSilver = 10f;

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
	public MarketEvent CurrentMarketEvet { get;set;}
	public bool GamePaused {get; private set;}

	public void SetPause (bool val)
	{
		GamePaused = val;

		if (GamePaused)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}

	public void AddPlayerToTeam (uint playerId, int teamId)
	{
		if (teamId < teams.Count)
			teams[teamId].playerIds.Add (playerId);
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
		{
			t.ReputationLoss ();
			Ivyyy.AudioHandler.Me.PlayOneShot (audioRepLoss);
		}
	}

	public void AddSilverCoins (uint playerId, float val)
	{
		Team t = GetTeamForPlayer (playerId);

		if (t != null)
			t.AddSilverCoins (val);

		if (PlayerStatsManager.Me != null)
		{
			if (val < 0f)
				PlayerStatsManager.Me.Stats (playerId).AddSilverSpent (val * -1);
			else
				PlayerStatsManager.Me.Stats (playerId).AddSilverEarned (val);
		}
		
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

		for (int i = 0; i < 2; ++i)
		{
			Team t = new Team(startReputation, startSilver);
			t.Id = i;
			teams.Add (t);
		}

		lifeTime = 0f;
		currentDateTime.Init();

		var playerConfigs = PlayerManager.Me.GetPlayerConfigs().ToArray();

		foreach (PlayerConfigurationDisplay i in playerConfigs)
		{
			PlayerConfiguration pc = i.playerConfiguration;
			Transform pos = spawnPoints[pc.TeamIndex];
			AddPlayerToTeam ( (uint) pc.PlayerIndex, pc.TeamIndex);
			i.SpawnPlayer (pos.position);
		}

		PlayerStatsManager.Me.Init();
	}

	public void GoToStatsScreen ()
	{
		foreach (PlayerConfigurationDisplay i in PlayerManager.Me.GetPlayerConfigs().ToArray())
			i.EnableUi (false);

		MapManager.Me.LoadMap (Map.PlayerStats);
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

		CheckWinLoseCondition();
    }

	void CheckWinLoseCondition()
	{
		//Win condition
		foreach (Team i in teams)
		{
			//Team win when reaching 100 reputation
			if (i.Reputation >= reputationNeededToWin)
				PlayerStatsManager.Me.IndexTeamWon.Add (i.Id);

			//Team loses when negative silver or 0 reputation
			if (i.SilverCoins < 0f || i.Reputation <= 0)
				PlayerStatsManager.Me.IndexTeamLose.Add (i.Id);
		}

		if (PlayerStatsManager.Me.IndexTeamWon.Count > 0
			|| PlayerStatsManager.Me.IndexTeamLose.Count > 0)
			GoToStatsScreen();
	}
}
