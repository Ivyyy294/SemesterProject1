using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class PlayerStats
{
	[System.Serializable]
	public class DayStats
	{
		int day;
		//Silver
		public float silverEarned;
		public float silverSpent;

		//Reputation
		public float reputationEarned;
		public float reputationLost;

		public DayStats (int _day) {day = _day;}
	}

	public PlayerStats (int pIndex, int tIndex)
	{
		playerIndex = pIndex;
		teamIndex = tIndex;
	}

	//General
	public int playerIndex;
	public int teamIndex;
	public float TimePlayed {get;set;}

	//Silver
	public float TaxPayed { get; set;}
	public float SilverEarnedTotal {get; private set;}
	public float SilverSpentTotal {get; private set;}

	//Reputation
	public float reputationEarnedTotal {get; private set;}
	public float reputationLostTotal {get; private set;}

	//Ware
	public int WarePickedUp{get; set;}
	public int WareDamaged{get; set;}
	//public int WareStoredCorrectly{get; set;}
	public int WareSold{get; set;}
	public int WareBought{get; set;}

	//Merchants
	public int RequestCompleted {get; set;}

	//Emotes
	//public int EmotesUsed {get;set;}

	//Private Values
	List <DayStats> dayStats = new List<DayStats>();

	public string GetAsJSON()
	{
		string val = "\"Player\":" + playerIndex.ToString();

		string baseValues = "\"Overview\":" + JsonConvert.SerializeObject(this) + "";
		string dayValues = "\"Days\":" + JsonConvert.SerializeObject(dayStats);

		val = "{" + val + "," + baseValues + "," + dayValues + "}";

		return val;
	}

	public void AddSilverEarned (float val)
	{
		DayStats day = GetCurrentDayStat();
		day.silverEarned += val;
		SilverEarnedTotal += val;
	}

	public void AddSilverSpent (float val)
	{
		DayStats day = GetCurrentDayStat();
		day.silverSpent += val;
		SilverSpentTotal += val;
	}
		public void AddReputationEarned (float val)
	{
		DayStats day = GetCurrentDayStat();
		day.reputationEarned += val;
		reputationEarnedTotal += val;
	}

	public void AddReputationLost (float val)
	{
		DayStats day = GetCurrentDayStat();
		day.reputationLost += val;
		reputationLostTotal += val;
	}

	//Private Functions
	DayStats GetCurrentDayStat ()
	{
		int currentDay = GameStatus.Me.GetCurrentDateTime().day;

		while (dayStats.Count < currentDay)
			dayStats.Add (new DayStats (dayStats.Count));
			
		return dayStats[currentDay - 1];
	}
}

public class PlayerStatsManager : MonoBehaviour
{
	private List <PlayerStats> stats = new List<PlayerStats>();
	string fileDirPath;
	//Public Values
	public static PlayerStatsManager Me;
	public int IndexTeamWon { get; set;}

	//Public Functions

	public void Init()
    {
		fileDirPath = Application.persistentDataPath;

		IndexTeamWon = -1;

		stats.Clear();

		foreach (PlayerConfigurationDisplay i in PlayerManager.Me.GetPlayerConfigs())
		{
			PlayerConfiguration pConfig = i.playerConfiguration;
			stats.Add (new PlayerStats(pConfig.PlayerIndex, pConfig.TeamIndex));
		}
    }

	public List <PlayerStats> Stats ()
	{
		return stats;
	}

	public PlayerStats Stats (uint indexPlayer)
	{
		if (indexPlayer < stats.Count)
			return stats[(int)indexPlayer];

		Debug.Log ("Invalid Player Index!");
		return default (PlayerStats);
	}

	public void SaveToFile ()
	{
		string Report = "";
		
		for (int i = 0; i < stats.Count; ++i)
		{
			string json = stats[i].GetAsJSON ();

			if (i == 0)
				Report = json;
			else
				Report += "," + json;
		}
		
		Report = "{\"Report\":[" + Report + "]}";

		Ivyyy.SaveGame.Save (fileDirPath, "SMP1Report.json", Report);
	}

	//Private Functions

    void Awake()
    {
		if (Me == null)
			Me = this;
		else
			Debug.Log ("Tried to create new Instance of Singelton!");
    }

    // Update is called once per frame

	private void Update()
	{
		foreach (PlayerStats i in stats)
			i.TimePlayed += Time.deltaTime;
	}
}
