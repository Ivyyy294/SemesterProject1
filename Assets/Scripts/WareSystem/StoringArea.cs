using UnityEngine;

public enum StoringAreaId
{
	Cold,
	Warm,
	Wet,
	Dry
}

public class StoringArea : MonoBehaviour
{
	//Editor Values
	public StoringAreaId areaId;
}
