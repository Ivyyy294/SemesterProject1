using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RequestItem
{
	public Ware ware;
	public float requestChance;
}

[CreateAssetMenu (fileName ="New Merchant", menuName = "Merchant")]
public class Merchant : ScriptableObject
{
	[Header ("General")]
	public new string name;
	public Sprite sprite;

	[Header ("Goods")]
	public Ivyyy.WeightedSpawnManager <Ware> requestManager = new Ivyyy.WeightedSpawnManager <Ware>();
	public uint requestFrequency;
}
