using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivyyy
{
	[System.Serializable]
	public class Path2D
	{
		public string name;
		public List <Transform> wayPoints;
	}

	public class Pathfinding2D : MonoBehaviour
	{
		//Editor Values
		[SerializeField] List <Path2D> paths;
		[SerializeField] GameObject objectToMove;
		[SerializeField] float speed;
		[SerializeField] List <int> layerMask;
		[SerializeField] float safetyDistance = 1f;
		
		//Private Values
		private Path2D activePath = null;
		private int currentWayPoint;
		private float timer;
		private bool done;

		//Public Functions
		public bool CurrentPathDone()
		{
			return done;
		}

		public string CurrentPath ()
		{
			if (activePath != null)
				return activePath.name;

			return "";
		}

		public void StartPath (string name)
		{
			foreach (Path2D i in paths)
			{
				if (i.name.Equals (name))
				{
					activePath = i;
					done = false;
					break;
				}
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			StartPath ("test");
		}

		private Vector2 GetSizeOfObject()
		{
			Vector2 size = new Vector2(1f, 1f);
			Renderer renderer = objectToMove.GetComponent<Renderer>();

			if (renderer == null)
				renderer = objectToMove.GetComponentInChildren <Renderer>();

			if (renderer != null)
				size = renderer.bounds.size;

			return size;
		}

		private bool IsPathBlocked()
		{
			Vector3 p1 = activePath.wayPoints[currentWayPoint -1].position;
			Vector3 p2 = activePath.wayPoints[currentWayPoint].position;
			
			Vector3 dir = (p2 - p1).normalized;
			Vector2 size = GetSizeOfObject();

			int _layerMask = 0;

			foreach (int i in layerMask)
				_layerMask |= 1 << i;

			RaycastHit2D[] hitInfo =  _layerMask == 0 ? Physics2D.BoxCastAll (objectToMove.transform.position, size, 0f, dir, safetyDistance)
				: Physics2D.BoxCastAll (objectToMove.transform.position, size, 0f, dir, safetyDistance, _layerMask);

			foreach (RaycastHit2D i in hitInfo)
			{
				if (!i.collider.isTrigger && !i.collider.transform.IsChildOf (objectToMove.transform))
					return true;
			}

			return false;
		}

		private void Move()
		{
			Vector3 p1 = activePath.wayPoints[currentWayPoint -1].position;
			Vector3 p2 = activePath.wayPoints[currentWayPoint].position;
			float journeyLength = Vector3.Distance (p1, p2);

			if (!IsPathBlocked())
			{
				// Distance moved equals elapsed time times speed..
				float distCovered = timer * speed;

				// Fraction of journey completed equals current distance divided by total distance.
				float fractionOfJourney = distCovered / journeyLength;

				// Set our position as a fraction of the distance between the markers.
				objectToMove.transform.position = Vector3.Lerp(p1, p2, fractionOfJourney);

				timer += Time.deltaTime;
			}
		}

		void Update()
		{
			if (activePath != null && objectToMove != null && !done)
			{
				//Setting start position
				if (currentWayPoint == 0)
				{
					objectToMove.transform.position = activePath.wayPoints[0].position;
					currentWayPoint = 1;
					timer = 0f;
				}
				else
				{
					Move();

					//Waypoint reached
					if (objectToMove.transform.position == activePath.wayPoints[currentWayPoint].position)
					{
						++currentWayPoint;
						timer = 0f;
					}
				}

				//Endpoint reached
				if (currentWayPoint >= activePath.wayPoints.Count)
				{
					done = true;
					currentWayPoint = 0;
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (activePath != null)
			{
				for (int i = 0; i < activePath.wayPoints.Count - 1; ++i)
				{
					Vector3 p1 = activePath.wayPoints[i].position;
					Vector3 p2 = activePath.wayPoints[i + 1].position;
					Vector3 dir = (p2 - p1).normalized;

					Gizmos.color = Color.green;
					Gizmos.DrawLine(p1, p2);

					Gizmos.color = Color.red;
					Vector3 pos = objectToMove.transform.position + dir * safetyDistance;
					Gizmos.DrawWireCube (pos, GetSizeOfObject());
				}
				// Draws a blue line from this transform to the target
			}
		}
	}
}

