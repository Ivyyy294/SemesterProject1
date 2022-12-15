using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class MerchantDisplay : MonoBehaviour
{
	//Editor Values
	[SerializeField] Merchant merchant;
	[SerializeField] GameObject requestIndicator;
	
	//Private Values
	SpriteRenderer spriteRenderer;
	double lifeTime = 0.0;
	Ware currentRequest = null;

	//Public Functions
	public bool Interact (GameObject obj)
	{
		if (obj != null && currentRequest != null)
		{
			WareDisplay wareDisplay = obj.GetComponent<WareDisplay>();

			if (wareDisplay.damaged)
			{
				GameStatus.Me.AddReputation (0, -1f);
				return true;
			}
			else
			{
				Ware tmp = wareDisplay.GetWare();

				if (tmp != null && tmp.ID == currentRequest.ID)
				{
					GameStatus.Me.AddSilverCoins (0, tmp.value);
					ChangeRequest (null);
					return true;
				}
			}
		}

		return false;
	}

	public void ActivateRequestIfReady()
	{
		Debug.Log ("ActivateRequestIfReady");

		if (currentRequest == null
			&& lifeTime >= merchant.requestFrequency)
		{
			ChangeRequest (merchant.GetNewRequest());
			lifeTime = 0.0;
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
	}

	private void ChangeRequest (Ware obj)
	{
		currentRequest = obj;
		DisplayRequest (obj);
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
}
