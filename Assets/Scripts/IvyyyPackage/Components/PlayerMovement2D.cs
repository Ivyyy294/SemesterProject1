using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Ivyyy
{
	// PlayerScript requires the GameObject to have a Rigidbody component
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovement2D : MonoBehaviour
	{
		protected Rigidbody2D m_Rigidbody;

		//Offset that is going to be added to trajectory during FixedUpdate
		protected Vector3 trajectoryOffset;

		//Current Movement trajectory
		protected Line trajectory = new Line();

		[Header ("Acceleration")]
		[SerializeField] float maxSpeed = 0f;
		[SerializeField] float acceleration = 0f;
		[Space]
		[Header ("Deacceleration")]
		[SerializeField] float deacceleration = 0.1f;

		//Start is called before the first frame update
		void Start()
		{
			m_Rigidbody = gameObject.GetComponent <Rigidbody2D>();
			Assert.IsTrue (m_Rigidbody != null, "Missing Rigidbody!");
		}

		//Update is called once per frame
		protected virtual void Update()
		{
			trajectoryOffset = Vector2.zero;

			if (Input.GetKey(KeyCode.D))
				trajectoryOffset.x += acceleration;

			if (Input.GetKey(KeyCode.A))
				trajectoryOffset.x -= acceleration;

			if (Input.GetKey(KeyCode.W))
				trajectoryOffset.y += acceleration;

			if (Input.GetKey(KeyCode.S))
				trajectoryOffset.y -= acceleration;

			m_Rigidbody.velocity = Vector3.zero;
		}

		float GetAxisMovementOffset (float rawOffset, float currentAxisVelocity)
		{
			float val = 0f;
			float fixedDeltaTime = Time.fixedDeltaTime;

			if (rawOffset != 0f)
				val = rawOffset * fixedDeltaTime;
			//Deacceleration
			else if (currentAxisVelocity != 0f)
			{
				float deltaDeacceleration = deacceleration * fixedDeltaTime;
				float tmpDeacceleration = Mathf.Min (Mathf.Abs (currentAxisVelocity), deltaDeacceleration);

				if (currentAxisVelocity > 0f)
					val -= tmpDeacceleration;
				else if (currentAxisVelocity < 0f)
					val += tmpDeacceleration;
			}

			return val;
		}

		protected virtual void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;

			trajectoryOffset.x = GetAxisMovementOffset (trajectoryOffset.x, trajectory.P2.x);
			trajectoryOffset.y = GetAxisMovementOffset (trajectoryOffset.y, trajectory.P2.y);
			
			trajectory.P2 += trajectoryOffset;

			//Apply movement
			if (trajectory.Length > 0f && m_Rigidbody != null)
			{
				//Cap trajectory at max speed
				float deltaMaxSpeed = maxSpeed * fixedDeltaTime;

				if (trajectory.Length > deltaMaxSpeed)
					trajectory.Length = deltaMaxSpeed;

				//Debug.Log ("Player speed: " + (trajectory.Length / fixedDeltaTime));

				//m_Rigidbody.velocity = new Vector3 (0f, 0f);
				m_Rigidbody.MovePosition (transform.position + trajectory.P2);
			}

			//if (trajectory.P2.y > 0f)
			//	transform.localScale = new Vector3 (1, 1, 1);
			//else if (trajectory.P2.y < 0f)
			//	transform.localScale = new Vector3 (1, -1, 1);
		}

		private void OnCollisionEnter(Collision collision)
		{
			//Line tmp = new Line(transform.position, collision.gameObject.transform.position);

			//// X-Axis
			//if (tmp.Angle <= 45f || tmp.Angle >= 315f
			//	|| (tmp.Angle >= 135f && tmp.Angle <= 225))
			//{
			//	trajectory.P2 = new Vector3(0f, trajectory.P2.y);
			//}

			////Y-Axis
			//if (tmp.Angle >= 45f && tmp.Angle <= 90f
			//	|| tmp.Angle >= 225f && tmp.Angle <= 315f)
			//{
			//	trajectory.P2 = new Vector3(trajectory.P2.x, 0f);
			//}

			trajectory.Length = 0f;
			m_Rigidbody.velocity = Vector3.zero;
		}
	}
}
