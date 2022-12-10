using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RequestItem
{
	public Crate crate;
	public float requestChance;
}

[CreateAssetMenu (fileName ="New Merchant", menuName = "Merchant")]
public class Merchant : ScriptableObject
{
	public new string name;
	public Sprite sprite;
	public List <RequestItem> goodsToRequest;
	public uint requestFrequency;

	public Crate GetNewRequest ()
	{
		if (goodsToRequest.Count > 0)
			return goodsToRequest[0].crate;

		return null;
	}
}
