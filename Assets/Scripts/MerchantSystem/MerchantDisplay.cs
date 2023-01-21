using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (SpriteRenderer))]
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
	
	//Private Values
	SpriteRenderer spriteRenderer;
	double lifeTime = 0.0;
	Ware currentRequest = null;
	PlayerStatsManager statsManager;

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
						GameStatus.Me.AddReputation (playerId, maxRequestTime);
						playerStats.RequestCompleted++;
						ChangeRequest (null);
					}
					else
						GameStatus.Me.LossReputation (playerId);

					return true;
				}
			}

		}

		return false;
	}

	public void ActivateRequestIfReady()
	{
		if (currentRequest == null
			&& lifeTime >= merchant.requestFrequency)
		{
			ChangeRequest (merchant.requestManager.GetObjectToSpawn());
		}
	}

	public void SetSprite ()
	{
		if (merchant != null)
		{
			spriteRenderer = GetComponent <SpriteRenderer>();

			if (spriteRenderer != null)
				spriteRenderer.sprite = merchant.sprite;
		}
	}
	//Private Functions
	private void Start()
	{
		statsManager = PlayerStatsManager.Me;
		SetSprite();
		merchant.requestManager.Init();
	}

	private void Update()
	{
		if (currentRequest == null && lifeTime < merchant.requestFrequency)
			lifeTime += Time.deltaTime;
		else if (currentRequest != null && lifeTime >= maxRequestTime)
			ChangeRequest (null);
	}

	private void ChangeRequest (Ware obj)
	{
		currentRequest = obj;
		DisplayRequest (obj);
		lifeTime = 0f;
	}

	private void DisplayRequest (Ware obj)
	{
		SpriteRenderer r = requestIndicator.GetComponent<SpriteRenderer>();
		
		if (r != null)
		{
			if (obj != null)
				r.sprite = obj.SpriteOk;
			else
				r.sprite = null;
		}
	}

	private Ware GetCrateFromGameObject (GameObject obj)
	{
		if (obj != null)
		{
			WareDisplay tmp = obj.GetComponent<WareDisplay>();

			if (tmp != null)
				return tmp.ware;
		}

		return null;
	}

	private void PlaySaleAudio (float wareValue)
	{
		if (wareValue <= 2f)
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioSmallSale);
		else if (wareValue <= 4f)
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioMediumSale);
		else
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioLargeSale);
		
	}
}

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