using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
	public UnityEvent callBackTriggerEnter;
	public UnityEvent callBackTriggerStay;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (callBackTriggerEnter != null)
			callBackTriggerEnter.Invoke();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (callBackTriggerStay != null)
			callBackTriggerStay.Invoke();
	}
}
