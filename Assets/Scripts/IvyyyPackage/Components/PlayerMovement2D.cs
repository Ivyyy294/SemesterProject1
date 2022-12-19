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
		//Private Values
		Rigidbody2D m_Rigidbody;
		Vector2 inputVector;
		float timeAcceleration = 0f;
		float timeDeacceleration = 0f;
		Vector3 Velocity;
		float currentSpeed;

		[Header ("Acceleration")]
		[SerializeField] float maxSpeed = 0f;
		[SerializeField] AnimationCurve accelerationCurve;
		[Space]
		[Header ("Deacceleration")]
		[SerializeField] AnimationCurve deaccelerationCurve;

		//Start is called before the first frame update
		void Start()
		{
			m_Rigidbody = gameObject.GetComponent <Rigidbody2D>();
			Assert.IsTrue (m_Rigidbody != null, "Missing Rigidbody!");
		}

		//float GetAxisMovementOffset (float rawOffset, float currentAxisVelocity)
		//{
		//	float val = 0f;
		//	float fixedDeltaTime = Time.fixedDeltaTime;

		//	if (rawOffset != 0f)
		//		val = rawOffset * fixedDeltaTime;
		//	//Deacceleration
		//	else if (currentAxisVelocity != 0f)
		//	{
		//		float deltaDeacceleration = deacceleration * fixedDeltaTime;
		//		float tmpDeacceleration = Mathf.Min (Mathf.Abs (currentAxisVelocity), deltaDeacceleration);

		//		if (currentAxisVelocity > 0f)
		//			val -= tmpDeacceleration;
		//		else if (currentAxisVelocity < 0f)
		//			val += tmpDeacceleration;
		//	}

		//	return val;
		//}

		protected virtual void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			float tmpSpeed = maxSpeed * fixedDeltaTime;

			if (timeAcceleration > 0f)
			{
				tmpSpeed *= accelerationCurve.Evaluate (timeAcceleration);
				currentSpeed = tmpSpeed;
			}
			else if (timeDeacceleration > 0f)
				tmpSpeed = currentSpeed * deaccelerationCurve.Evaluate (timeDeacceleration);

			Velocity = inputVector * tmpSpeed;

			//Apply movement
			if (Velocity != Vector3.zero && m_Rigidbody != null)
				m_Rigidbody.MovePosition (transform.position + Velocity);
		}

		//private void OnCollisionEnter(Collision collision)
		//{
		//	trajectory.Length = 0f;
		//	m_Rigidbody.velocity = Vector3.zero;
		//}

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
	}
}
