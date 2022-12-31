using UnityEngine;

[CreateAssetMenu (fileName ="NewShipSpawn", menuName = "Spawn/Ship")]
public class ShipSpawn : ScriptableObject
{
	public Ship ship;
	[Range (0f, 1f)]
	public float weight;
	//[Range (0f, 1f)]
	//public float maxWeight;

	public float GetWeight()
	{
		return weight;// Random.Range (minWeight, maxWeight);
	}
}
