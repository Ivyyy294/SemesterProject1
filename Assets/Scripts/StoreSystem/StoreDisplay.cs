using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class StoreDisplay : MonoBehaviour
{
	//Editor Values
	[SerializeField] Store store;

	[Space]
	[SerializeField] StoreSlot slotLeft;
	[SerializeField] StoreSlot slotRight;

	// Start is called before the first frame update
	void Start()
	{
		if (store != null)
		{
			GetComponent<SpriteRenderer>().sprite = store.sprite;
			slotLeft.SetWare (store.waresToSell[0]);
			slotRight.SetWare (store.waresToSell[1]);
		}
		else
			Debug.Log ("Store not set!");
	}
}
