using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIndicator : MonoBehaviour
{
	//Editor Values
	[SerializeField] Color blockedColor;

	//Private Values
	private Color clearColor;
	private SpriteRenderer spriteRenderer;
	uint collisionCounter;

	//Public Values
	public bool IsDropAreaClear() { return collisionCounter == 0; }

	//Private Values
	private void Start()
	{
		collisionCounter = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();
		clearColor = spriteRenderer.color;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			Debug.Log (collision.gameObject.tag);
			++collisionCounter;

			spriteRenderer.color = blockedColor;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			--collisionCounter;

			if (IsDropAreaClear())
				spriteRenderer.color = clearColor;
		}
	}
}
