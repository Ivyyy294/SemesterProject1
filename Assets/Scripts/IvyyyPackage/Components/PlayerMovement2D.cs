using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[System.Serializable]
public class SpeedProfile
{
	public float maxSpeed;
	public AnimationCurve accelerationCurve;
	public AnimationCurve deaccelerationCurve;
}

namespace Ivyyy
{
	// PlayerScript requires the GameObject to have a Rigidbody component
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovement2D : MonoBehaviour
	{
		//Protected Values
		protected Rigidbody2D m_Rigidbody;
		protected Vector2 inputVector;
		protected float timeAcceleration = 0f;
		protected float timeDeacceleration = 0f;
		protected Vector3 Velocity;
		protected float currentSpeed;
		protected SpeedProfile currentSpeedProfile;
		
		//Editor Values
		[Header ("Base Speed Profile")]
		public SpeedProfile speedProfile;

		//Public Functions
		public void Move(Vector2 input)
		{
			if (input != Vector2.zero)
			{
				timeAcceleration += Time.deltaTime;
				timeDeacceleration = 0f;
				inputVector = input;
			}
			else
			{
				timeAcceleration = 0f;

				if (timeDeacceleration < 1f)
					timeDeacceleration += Time.deltaTime;
			}

		}

		public float GetCurrentSpeedFactor ()
		{
			return currentSpeedProfile.accelerationCurve.Evaluate (timeAcceleration);
		}

		//Protected Functions
		virtual protected void Start()
		{
			m_Rigidbody = gameObject.GetComponent <Rigidbody2D>();
			Assert.IsTrue (m_Rigidbody != null, "Missing Rigidbody!");
			currentSpeedProfile = speedProfile;
		}

		protected virtual void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			float tmpSpeed = currentSpeedProfile.maxSpeed * fixedDeltaTime;

			if (timeAcceleration > 0f)
			{
				tmpSpeed *= GetCurrentSpeedFactor();
				currentSpeed = tmpSpeed;
			}
			else if (timeDeacceleration > 0f)
				tmpSpeed = currentSpeed * currentSpeedProfile.deaccelerationCurve.Evaluate (timeDeacceleration);

			Velocity = inputVector * tmpSpeed;

			//Apply movement
			if (Velocity != Vector3.zero && m_Rigidbody != null)
				m_Rigidbody.MovePosition (transform.position + Velocity);

			m_Rigidbody.velocity = Vector3.zero;
		}
	}
}
