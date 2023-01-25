using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MerchantDisplay : MonoBehaviour
{
	//Editor Values
	[SerializeField] Merchant merchant;
	[SerializeField] List <AudioClip> audioSmallSale = new List<AudioClip>();
	[SerializeField] List <AudioClip> audioMediumSale = new List<AudioClip>();
	[SerializeField] List <AudioClip> audioLargeSale = new List<AudioClip>();
	[SerializeField] float maxRequestTime;

	[Header ("Lara Values")]
	[SerializeField] GameObject requestIndicator;
	[SerializeField] GameObject requestIcon;
	[SerializeField] SpriteRenderer merchantSpriteRenderer;
	
	//Private Values
	double lifeTime = 0.0;
	float effectiveRequestFrequency;
	Ware currentRequest = null;
	PlayerStatsManager statsManager;
	Ivyyy.AudioHandler audioHandler;

	//Public Functions
	public bool Interact (GameObject obj, uint playerId)
	{
		if (obj != null && currentRequest != null)
		{
			WareDisplay wareDisplay = obj.GetComponent<WareDisplay>();

			if (wareDisplay != null)
			{
				PlayerStats playerStats = statsManager.Stats (playerId);
				playerStats.WareSold++;

				if (wareDisplay.damaged)
				{
					Debug.Log ("Play Upset");
					GameStatus.Me.LossReputation (playerId);
					return true;
				}
				else
				{
					float wareValue = wareDisplay.ware.sellValue;

					if (GameStatus.Me.CurrentMarketEvet != null)
						wareValue *= GameStatus.Me.CurrentMarketEvet.sellMod;

					PlaySaleAudio (wareValue);

					GameStatus.Me.AddSilverCoins (playerId, wareValue);

					//Reputation Gain / Loss
					if (wareDisplay.ware.ID == currentRequest.ID)
					{
						Debug.Log ("Play Happy");
						audioHandler.PlayOneShotFromList (merchant.audioHappy);
						GameStatus.Me.AddReputation (playerId, maxRequestTime);
						playerStats.RequestCompleted++;
						ChangeRequest (null);
					}
					else
					{
						GameStatus.Me.LossReputation (playerId);
						Debug.Log ("Play Upset");
						audioHandler.PlayOneShotFromList (merchant.audioUpset);
					}

					return true;
				}
			}

		}

		return false;
	}

	public bool IsRequestReady ()
	{
		return currentRequest == null && lifeTime >= effectiveRequestFrequency;
	}

	public void ActivateRequestIfReady()
	{
		if (IsRequestReady())
		{
			audioHandler.PlayOneShotFromList (merchant.audioRequest);
			ChangeRequest (merchant.requestManager.GetObjectToSpawn());
		}
	}

	public void SetSprite ()
	{
		if (merchant != null)
		{
			if (merchantSpriteRenderer != null)
				merchantSpriteRenderer.sprite = merchant.sprite;
		}
	}

	public void PlayChatter()
	{
		audioHandler.PlayOneShotFromList (merchant.audioChatter);
	}

	//Private Functions
	private void Start()
	{
		statsManager = PlayerStatsManager.Me;
		audioHandler = Ivyyy.AudioHandler.Me;
		SetSprite();
		effectiveRequestFrequency = GetEffectiveRequestFrequency();
		merchant.requestManager.Init();
	}

	private void Update()
	{
		if (currentRequest == null && lifeTime < effectiveRequestFrequency)
			lifeTime += Time.deltaTime;
		else if (currentRequest != null && lifeTime >= maxRequestTime)
			ChangeRequest (null);
	}

	private void ChangeRequest (Ware obj)
	{
		Debug.Log ("Play Request");
		currentRequest = obj;
		DisplayRequest (obj);
		lifeTime = 0f;
	}

	private void DisplayRequest (Ware obj)
	{
		if (obj != null)
		{
			requestIndicator.SetActive (true);
			SetRequestIcon (obj);
		}
		else
			requestIndicator.SetActive (false);
	}

	private void SetRequestIcon (Ware obj)
	{
		if (requestIcon != null)
		{
			SpriteRenderer r = requestIcon.GetComponent<SpriteRenderer>();
		
			if (r != null && obj != null)
			{
				r.sprite = obj.SpriteOk;

				//Andjust size to fit in bubble
				Vector2Int wareSize = obj.size;

				if (wareSize.x >= 4 || wareSize.y >= 4)
					requestIcon.transform.localScale = new Vector3 (0.5f, 0.5f);
				else if (wareSize.x >= 2 || wareSize.y >= 2)
					requestIcon.transform.localScale = new Vector3 (0.75f, 0.75f);
				else
					requestIcon.transform.localScale = new Vector3 (1f, 1f);

				Bounds bounds = r.bounds;
				requestIcon.transform.localPosition = new Vector3 (0f, -bounds.size.y / 2);
			}
		}
	}

	private void PlaySaleAudio (float wareValue)
	{
		if (wareValue <= 2f)
			audioHandler.PlayOneShotFromList (audioSmallSale);
		else if (wareValue <= 4f)
			audioHandler.PlayOneShotFromList (audioMediumSale);
		else
			audioHandler.PlayOneShotFromList (audioLargeSale);
		
	}

	private float GetEffectiveRequestFrequency ()
	{
		float effectiveRequestFrequency = merchant.requestFrequency * (4f / PlayerManager.Me.GetPlayerConfigs().Count);
		return effectiveRequestFrequency;
	}
}

#if UNITY_EDITOR
[CustomEditor (typeof (MerchantDisplay))]
public class MerchantDisplayEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		MerchantDisplay merchantDisplay = (MerchantDisplay) target;
		merchantDisplay.SetSprite();
	}
}
#endif