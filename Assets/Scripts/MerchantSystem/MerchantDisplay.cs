using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	//Public Functions
	public bool Interact (GameObject obj, uint playerId)
	{
		if (obj != null && currentRequest != null)
		{
			WareDisplay wareDisplay = obj.GetComponent<WareDisplay>();

			if (wareDisplay != null)
			{
				if (wareDisplay.damaged)
				{
					GameStatus.Me.LossReputation (playerId);
					return true;
				}
				else
				{
					uint wareValue = wareDisplay.ware.value;

					PlaySaleAudio (wareValue);

					GameStatus.Me.AddSilverCoins (playerId, wareValue);

					//Reputation Gain / Loss
					if (wareDisplay.ware.ID == currentRequest.ID)
					{
						GameStatus.Me.AddReputation (playerId, maxRequestTime);
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
		//Debug.Log ("ActivateRequestIfReady");

		if (currentRequest == null
			&& lifeTime >= merchant.requestFrequency)
		{
			ChangeRequest (merchant.GetNewRequest());
		}
	}

	//Private Functions
	private void Start()
	{
		spriteRenderer = GetComponent <SpriteRenderer>();
		spriteRenderer.sprite = merchant.sprite;
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

	private void PlaySaleAudio (uint wareValue)
	{
		if (wareValue <= 2)
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioSmallSale);
		else if (wareValue <= 4)
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioMediumSale);
		else
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (audioLargeSale);
		
	}
}
