using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (SpriteRenderer))]
public class StoreDisplay : MonoBehaviour
{
	//Editor Values
	[SerializeField] Store store;

	[Space]
	[SerializeField] StoreSlot slotLeft;
	[SerializeField] StoreSlot slotRight;

	//Public
	public void Init()
	{
		if (store != null)
		{
			GetComponent<SpriteRenderer>().sprite = store.sprite;
			slotLeft.SetWare (store.waresToSell[0]);
			slotRight.SetWare (store.waresToSell[1]);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		if (store == null)
			Debug.Log ("Store not set!");

		Init();
	}
}

#if UNITY_EDITOR
[CustomEditor (typeof (StoreDisplay))]
public class StoreDisplayEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		StoreDisplay storeDisplay = (StoreDisplay) target;
		storeDisplay.Init();
	}
}
#endif