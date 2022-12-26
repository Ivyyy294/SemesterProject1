using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu (fileName ="New Ware", menuName = "Ware")]
public class Ware : ScriptableObject
{
	public enum WeightCategory
	{
		Light,
		Medium,
		Heavy
	}

	//Generall
	[Header ("General")]
	public new string name;
	public uint ID = 0;
	public uint value = 1;
	public Sprite SpriteOk;
	public Sprite SpriteDamaged;
	public WeightCategory weight;
	public Vector2Int size = new Vector2Int (2, 2);

	//Durability
	[Header ("Durability")]
	public uint durability;
	[Space]
	public uint durabilityExtended;
	public List <StoringAreaId> storingAreaIds;

	public Vector2 GetSizeInWorld ()
	{
		Vector2 wSize = new Vector2(size.x * 0.5f, size.y * 0.5f);
		return wSize;
	}

	public static Ware GetFromGameObject (GameObject obj)
	{
		Ware result = null;

		if (obj != null)
		{
			WareDisplay tmp = obj.GetComponent <WareDisplay>();
			
			if (tmp != null)
				result = tmp.ware;
		}

		return result;
	}
}
