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
	[Header ("General")]
	public new string name;
	public Sprite sprite;

	[Header ("Goods")]
	public List <RequestItem> requestList;
	public uint requestFrequency;

	public Crate GetNewRequest ()
	{
		if (requestList.Count > 0)
			return requestList[0].crate;

		return null;
	}
}
