using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarketEvent
{
	public float duration;
	public float reputationMod;
	public float buyMod;
	public float sellMod;
}

public class MarketEventHandler : MonoBehaviour
{
	//Editor Value
	[SerializeField] SpawnProfile spawnProfile;
	[SerializeField] MarketEvent marketEvent;
	[SerializeField] GameObject eventIndicator;

	//private Values
	private Ivyyy.WeightedRandom random = new Ivyyy.WeightedRandom();
	private float spawnTime = 0f;
	private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
		spawnTime = GetNewSpawnTime();
		random.SetWeight (spawnProfile.spawnChanceCurve);
    }

	// Update is called once per frame
	void Update()
	{
		if (GameStatus.Me != null)
		{
			if (GameStatus.Me.CurrentMarketEvet == null)
			{
				if (timer >= spawnTime)
				{
					Debug.Log ("Event started");
					timer = 0f;
					GameStatus.Me.CurrentMarketEvet = marketEvent;

					if (eventIndicator != null)
						eventIndicator.SetActive (true);
				}
			}
			else
			{
				if (timer >= GameStatus.Me.CurrentMarketEvet.duration)
				{
					Debug.Log ("Event ended");
					GameStatus.Me.CurrentMarketEvet = null;
					spawnTime = GetNewSpawnTime();
					timer = 0f;

					if (eventIndicator != null)
						eventIndicator.SetActive (false);
				}
			}
		}

		timer += Time.deltaTime;
    }

	float GetNewSpawnTime ()
	{
		float offset = spawnProfile.tMax - spawnProfile.tMin;
		float val = spawnProfile.tMin + offset * random.Value(); 
		return val;
	}
}
