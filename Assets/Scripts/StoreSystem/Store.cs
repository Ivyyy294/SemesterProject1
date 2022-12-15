using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Store", menuName = "Store")]
public class Store : ScriptableObject
{
	public new string name;
	public Sprite sprite;
	public Ware[] waresToSell = new Ware[2];
}
