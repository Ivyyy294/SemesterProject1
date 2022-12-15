using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Ship", menuName = "Ship")]
public class Ship : ScriptableObject
{
	public new string name;
	float tMin;
	float tMax;
	public List <Ware> wares;
}
