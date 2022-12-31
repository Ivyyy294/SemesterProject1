using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProfile
{
	public uint day;
	public float tMin;
	public float tMax;
}

class JettyContainer
{
	public Jetty jetty;
	public float timer;
	public float SpawnTime;
}

public class Harbour : MonoBehaviour
{
	//Editor Values
	[SerializeField] List <Ship> ships;
	[SerializeField] List <ShipSpawn> shipSpawn;
	[SerializeField] List <Jetty> jetties;
	[SerializeField] List <SpawnProfile> spawnProfiles = new List <SpawnProfile>();

	//Private Values
	private float[] Weights;
	private JettyContainer[] jettyContainers;
	private SpawnProfile sProfile;
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
			InitJettyContainers();

			initDone = true;
		}

		SpawnShipSimple();
	}

	void SetCurrentSpawnProfile()
	{
		int currentDay = GameStatus.Me.GetCurrentDateTime().day;

		for (int i = spawnProfiles.Count -1; i >= 0; --i)
		{
			if (currentDay >= spawnProfiles[i].day)
			{
				sProfile = spawnProfiles[i];
				break;
			}
		}

	}

	void SpawnShipSimple ()
	{
		foreach (JettyContainer i in jettyContainers)
		{
			if (i != null && !i.jetty.IsShipActive())
			{
				if (i.timer >= i.SpawnTime)
				{
					SpawnShip (i.jetty);
					i.timer = 0f;
					i.SpawnTime = Random.Range (sProfile.tMin, sProfile.tMax);
				}
				else
					i.timer += Time.deltaTime;
			}
		}
	}

	//void SpawnShipWithMagic()
	//{
		//for (int i = 0; i < jetties.Count; ++i)
		//{
		//	if (!jetties[i].IsShipActive())
		//	{
		//		float var = Random.value;

		//		float magic = Mathf.Pow (((timerJetties[i] - sProfile.tMin) / (sProfile.tMax - sProfile.tMin)), 5f);
			
		//		if (magic > var)
		//		{
		//			Debug.Log ("Var: " + var + " Magic: " + magic + " Timer: " + timerJetties[i]);
		//			SpawnShip (jetties[i]);
		//			timerJetties[i] = 0f;
		//			cooldownTimer = 0f;
		//			break;
		//		}
		//	}
		//}

		//for (int i = 0; i < jetties.Count; ++i)
		//{
		//	if (!jetties[i].IsShipActive())
		//		timerJetties[i] += Time.fixedDeltaTime;
		//}
	//}

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

	void InitJettyContainers ()
	{
		if (jettyContainers == null)
		{
			jettyContainers = new JettyContainer [jetties.Count];

			for (int i = 0; i < jetties.Count; ++i)
			{
				jettyContainers[i] = new JettyContainer();
				JettyContainer tmp = jettyContainers[i];
				tmp.jetty = jetties[i];
				tmp.timer = 0f;
				tmp.SpawnTime = Random.Range (sProfile.tMin, sProfile.tMax);
			}
		}
	}
}
