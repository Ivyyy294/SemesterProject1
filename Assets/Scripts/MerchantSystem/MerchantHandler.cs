using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantHandler : MonoBehaviour
{
	//Editor Values
	[SerializeField] MerchantDisplay[] merchantsToControll;
	[SerializeField] float requestCooldown;
	[SerializeField] float chatterCooldown;

	//Private Values
	private float timerRequest;
	private float timerChatter;
	private float internRequestCooldown;
	private int playerInReach = 0;

    // Start is called before the first frame update
    void Start()
    {
        internRequestCooldown = requestCooldown * (4f / PlayerManager.Me.GetPlayerConfigs().Count);
    }

    // Update is called once per frame
    void Update()
    {
		//Requests

		if (playerInReach > 0)
		{
			Request();
			Chatter();
		}

		if (timerRequest < internRequestCooldown)
			timerRequest += Time.deltaTime;
		
		//Chatter timer only runs when a player in at the market
		if (playerInReach > 0 && timerChatter < chatterCooldown)
			timerChatter += Time.deltaTime;
		else	
			timerChatter = 0f;
    }

	MerchantDisplay PickRandomMerchant ()
	{
		int anzMerchants = merchantsToControll.Length;
		int randomIndex = Random.Range (0, anzMerchants);
		return merchantsToControll[randomIndex];
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
			playerInReach++;		
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
			playerInReach--;		
	}

	private void Request()
	{
		if (timerRequest >= internRequestCooldown)
		{
			MerchantDisplay m = PickRandomMerchant();

			if (m != null && m.IsRequestReady())
			{
				m.ActivateRequestIfReady();
				timerRequest = 0f;
			}
		}
	}

	private void Chatter()
	{
		if (timerChatter >= chatterCooldown)
		{
			MerchantDisplay m = PickRandomMerchant();

			if (m != null && !m.IsRequestReady())
			{
				Debug.Log ("Play Chatter");
				m.PlayChatter();
				timerChatter = 0f;
			}
		}
	}
}
