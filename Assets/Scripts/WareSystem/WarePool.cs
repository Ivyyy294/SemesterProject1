using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarePool : Ivyyy.ObjectPool
{
	public static WarePool Me {get; private set;}

	protected override void Awake()
	{
		if (Me != null && Me != this)
		{
			Debug.Log ("Trying to create a new Singelton instance!");
			Destroy (this);
			return;
		}
		
		Me = this;
		
		base.Awake();
	}
}
