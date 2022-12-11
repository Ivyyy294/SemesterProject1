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
	Crate currentRequest = null;

	//Public Functions
	public bool Interact (GameObject obj)
	{
		if (obj != null && currentRequest != null)
		{
			Crate tmp = GetCrateFromGameObject (obj);

			if (tmp != null && tmp.ID == currentRequest.ID)
			{
				GameStatus.Me.AddSilverCoins (0, tmp.value);
				ChangeRequest (null);
				return true;
			}
		}

		return false;
	}

	//Private Functions
	private void Start()
	{
		spriteRenderer = GetComponent <SpriteRenderer>();
		spriteRenderer.sprite = merchant.sprite;
	}

	private void Update()
	{
		if (currentRequest == null)
		{
			if (lifeTime >= merchant.requestFrequency)
			{
				ChangeRequest (merchant.GetNewRequest());
				lifeTime = 0.0;
			}
			else
				lifeTime += Time.deltaTime;
		}
	}

	private void ChangeRequest (Crate obj)
	{
		currentRequest = obj;
		DisplayRequest (obj);
	}

	private void DisplayRequest (Crate obj)
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

	private Crate GetCrateFromGameObject (GameObject obj)
	{
		if (obj != null)
		{
			CrateDisplay tmp = obj.GetComponent<CrateDisplay>();

			if (tmp != null)
				return tmp.crate;
		}

		return null;
	}
}
