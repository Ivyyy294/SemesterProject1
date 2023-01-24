using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantHandler : MonoBehaviour
{
	//Editor Values
	[SerializeField] MerchantDisplay[] merchantsToControll;
	[SerializeField] float requestCooldown;

	//Private Values
	private float timer;
	private float internRequestCooldown;

    // Start is called before the first frame update
    void Start()
    {
        internRequestCooldown = requestCooldown * (4f / PlayerManager.Me.GetPlayerConfigs().Count);
    }

    // Update is called once per frame
    void Update()
    {
		//Tries to activate a request at a random merchant every frame, until a request gets activated
		if (timer >= internRequestCooldown)
		{
			int anzMerchants = merchantsToControll.Length;
			int randomIndex = Random.Range (0, anzMerchants);
			MerchantDisplay m = merchantsToControll[randomIndex];

			if (m.IsRequestReady())
			{
				m.ActivateRequestIfReady();
				timer = 0f;
			}
		}
        else
			timer += Time.deltaTime;
    }
}
