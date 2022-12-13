using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSlot : MonoBehaviour
{
	//private Values
	private Ware ware;

	//Public Functions
	public void SetWare (Ware w) {ware = w;}

	public GameObject BuyWare ()
	{
		if (ware != null)
		{
			GameObject obj = WarePool.Me.GetPooledObject ();

			if (obj != null)
			{
				obj.SetActive (true);

				WareDisplay wDisplay = obj.GetComponent<WareDisplay>();

				if (wDisplay != null)
				{
					wDisplay.Init (ware);
				}
			}

			return obj;
		}
		
		return null;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
