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
	bool raycastCheckOk;

	//Public Values
	public bool IsDropAreaClear() { return collisionCounter == 0 && raycastCheckOk; }

	public void RaycastCheck (Vector3 target)
	{
		raycastCheckOk = true;

		Vector3 dir = (target - transform.position).normalized;
		float dist = Vector3.Distance (target, transform.position);
		int layerMask = 1 << 0;
		
		foreach (RaycastHit2D hitInfo in Physics2D.RaycastAll (transform.position, dir, dist, layerMask))
		{
			if (!hitInfo.collider.CompareTag ("Player"))
			{
				raycastCheckOk = false;
				break;
			}
		}
	}

	//Private Values
	private void Start()
	{
		collisionCounter = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();
		clearColor = spriteRenderer.color;
	}

	private void Update()
	{
		if (IsDropAreaClear())
			spriteRenderer.color = clearColor;
		else
			spriteRenderer.color = blockedColor;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (IsColliderValid (collision))
			++collisionCounter;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (IsColliderValid (collision))
			--collisionCounter;
	}

	bool IsColliderValid (Collider2D collider)
	{
		bool valid = !collider.isTrigger;

		if (collider.CompareTag ("Player"))
		{
			Transform playerTrans = collider.transform.parent;

			if (playerTrans != null)
				valid &= !transform.IsChildOf (playerTrans);
		}

		return valid;
	}
}
