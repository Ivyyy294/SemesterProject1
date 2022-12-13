using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplay : MonoBehaviour
{
	//Editor values
	[SerializeField] List <Transform> warePos;

    // Start is called before the first frame update
    void OnEnable ()
    {
		foreach (Transform i in warePos)
		{
			GameObject obj = WarePool.Me.GetPooledObject();

			if (obj != null)
			{
				obj.SetActive (true);
				obj.transform.position = i.position;
				obj.transform.SetParent (transform);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
