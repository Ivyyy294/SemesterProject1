using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProfile
{
	public uint day;
	public float tMin;
	public float tMax;
	public AnimationCurve spawnChanceCurve;
}

public class Harbour : MonoBehaviour
{
	//Editor Values
	[SerializeField] List <ShipSpawn> shipSpawn;
	[SerializeField] List <Jetty> jetties;
	[SerializeField] List <SpawnProfile> spawnProfiles = new List <SpawnProfile>();
	[SerializeField] float shipStayTime;

	//Private Values
	private float[] Weights;
	private float[] spawnTimings;
	private SpawnProfile sProfile;
	private Ivyyy.WeightedRandom random = new Ivyyy.WeightedRandom();
	bool initDone = false;

	//Private Functions
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		SetCurrentSpawnProfile();

		if (!initDone)
		{
			ResetSpawnWeights();
			spawnTimings = new float [jetties.Count];

			for (int i = 0; i < jetties.Count; ++i)
				spawnTimings[i] = GetNewSpawnTime();

			initDone = true;
		}

		//Jetty timer update
		for (int i = 0; i < jetties.Count; ++i)
		{
			Jetty tmp = jetties[i];

			if (tmp.ShipDocked && tmp.timerShipDocked >= shipStayTime)
				tmp.CastOffShip();
			else if (!tmp.IsShipActive() && tmp.timerInactice >= spawnTimings[i])
			{
				SpawnShip(tmp);
				spawnTimings[i] = GetNewSpawnTime();
			}
		}
	}

	void SetCurrentSpawnProfile()
	{
		int currentDay = GameStatus.Me.GetCurrentDateTime().day;

		for (int i = spawnProfiles.Count -1; i >= 0; --i)
		{
			if (currentDay >= spawnProfiles[i].day)
			{
				sProfile = spawnProfiles[i];
				random.SetWeight (sProfile.spawnChanceCurve);
				break;
			}
		}
	}

	void SpawnShip (Jetty jetty)
	{
		float val = Random.value;

		for (int i = 0; i < Weights.Length; ++i)
		{
			if (val < Weights[i])
			{
				jetty.SpawnShip (shipSpawn[i].ship);
				return;
			}

			val -= Weights[i];
		}
	}

	void ResetSpawnWeights ()
	{
		Weights = new float [shipSpawn.Count];

		float totalWeight = 0f;

		for (int i = 0; i < shipSpawn.Count; ++i)
		{
			Weights[i] = shipSpawn[i].GetWeight();
			totalWeight += Weights[i];
		}

		for (int i = 0; i < Weights.Length; ++i)
			Weights[i] = Weights[i] / totalWeight;
	}

	float GetNewSpawnTime ()
	{
		float offset = sProfile.tMax - sProfile.tMin;
		float val = sProfile.tMin + offset * random.Value(); 
		Debug.Log ("New Spawn TIme: " + val);
		return val;
	}
}

