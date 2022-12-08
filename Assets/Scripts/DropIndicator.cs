using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIndicator : MonoBehaviour
{
	[SerializeField] Color blockedColor;
	private Color clearColor;
	private SpriteRenderer renderer;
	uint collisionCounter;

	private void Start()
	{
		collisionCounter = 0;
		renderer = GetComponent<SpriteRenderer>();
		clearColor = renderer.color;
	}

	public bool IsDropAreaClear() { return collisionCounter == 0; }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag ("Player"))
		{
			Debug.Log (collision.gameObject.tag);
			++collisionCounter;

			renderer.color = blockedColor;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag ("Player"))
			--collisionCounter;

		if (IsDropAreaClear())
			renderer.color = clearColor;
	}
}
