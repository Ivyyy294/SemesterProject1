using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
	public UnityEvent callBack;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		callBack.Invoke();
	}
}
