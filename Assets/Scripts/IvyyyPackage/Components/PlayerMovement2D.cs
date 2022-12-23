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
		//Private Values
		Rigidbody2D m_Rigidbody;
		Vector2 inputVector;
		float timeAcceleration = 0f;
		float timeDeacceleration = 0f;
		Vector3 Velocity;
		float currentSpeed;
		
		//Protected Values
		protected SpeedProfile currentSpeedProfile;

		[Header ("Base Speed Profile")]
		public SpeedProfile speedProfile;

		//Start is called before the first frame update
		virtual protected void Start()
		{
			m_Rigidbody = gameObject.GetComponent <Rigidbody2D>();
			Assert.IsTrue (m_Rigidbody != null, "Missing Rigidbody!");
			currentSpeedProfile = speedProfile;
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
			float tmpSpeed = currentSpeedProfile.maxSpeed * fixedDeltaTime;

			if (timeAcceleration > 0f)
			{
				tmpSpeed *= currentSpeedProfile.accelerationCurve.Evaluate (timeAcceleration);
				currentSpeed = tmpSpeed;
			}
			else if (timeDeacceleration > 0f)
				tmpSpeed = currentSpeed * currentSpeedProfile.deaccelerationCurve.Evaluate (timeDeacceleration);

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
