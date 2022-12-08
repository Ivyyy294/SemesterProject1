using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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
				float deltaMaxSpeed = maxSpeed * fixedDeltaTime;

				if (trajectory.Length > deltaMaxSpeed)
					trajectory.Length = deltaMaxSpeed;
				m_Rigidbody.MovePosition (transform.position + trajectory.P2);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			trajectory.Length = 0f;
			m_Rigidbody.velocity = Vector3.zero;
		}

		public void Move(Vector2 inputVector)
		{
			trajectoryOffset = inputVector * acceleration;
		}
	}
}
