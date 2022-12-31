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

public class JettyContainer
{
	public Jetty jetty;
	public float timerSpawn;
	public float timerDocked;
	public float spawnTime;
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
	private SpawnProfile sProfile;
	private Ivyyy.WeightedRandom random = new Ivyyy.WeightedRandom();
	private List <JettyContainer> jettyContainers = new List <JettyContainer>();
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

			foreach (Jetty i in jetties)
			{
				JettyContainer c = new JettyContainer();
				c.jetty = i;
				c.spawnTime = GetNewSpawnTime();
				jettyContainers.Add (c);
			}

			initDone = true;
		}

		//Jetty timer update
		foreach (JettyContainer i in jettyContainers)
		{
			if (i.jetty.IsShipDocked())
			{
				i.timerDocked += Time.fixedDeltaTime;

				if (i.timerDocked >= shipStayTime)
				{
					i.jetty.CastOffShip();
					i.timerDocked = 0f;
				}
			}
			else if (!i.jetty.IsShipActive())
			{
				i.timerSpawn += Time.fixedDeltaTime;

				if (i.timerSpawn >= i.spawnTime)
				{
					SpawnShip (i.jetty);
					i.spawnTime = GetNewSpawnTime();
					i.timerSpawn = 0f;
				}
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

