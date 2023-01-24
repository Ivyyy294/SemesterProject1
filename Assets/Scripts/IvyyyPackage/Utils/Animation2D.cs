using UnityEngine;
using System.Collections;

namespace Ivyyy
{
	[System.Serializable]
	public class AnimationData2D
	{
		public AnimationCurve animationCurveX = AnimationCurve.Constant (0f,1f,1f);
		public AnimationCurve animationCurveY = AnimationCurve.Constant (0f,1f,1f);
		public AnimationCurve animationCurveScale = AnimationCurve.Constant (0f,1f,1f);
		public float animationDuration;
		public bool loop = false;
	}

	public class Animation2D
	{
		AnimationData2D animationData;
		Transform objToMove;
		Vector3 startPos;
		Vector3 destination;
		float startTime;
		float timeFactor;
		public bool Done { get; private set;}

		Vector3 lastOffset;
		Vector3 lastPos;
		
		//Public Functions
		public void Init (Transform obj, AnimationData2D data)
		{
			Init (obj, obj.position, data);
		}

		public void Init (Transform obj, Vector3 dest, AnimationData2D data)
		{
			startTime = Time.time;
			animationData = data;
			objToMove = obj;
			startPos = obj.position;
			destination = dest;
			timeFactor = 0f;
			Done = false;
		}

		public IEnumerator Play()
		{
			while (!Done)
			{
				Next();
				yield return null;
			}
		}

		//PrivateFunctions
		private void Next()
		{
			float currentTime = Time.time;
			timeFactor = (currentTime - startTime) / animationData.animationDuration;

			if (animationData.animationCurveScale.length > 0)
				ScaleAnimation ();

			MoveAnimation ();

			if (timeFactor >= 1f)
			{
				if (animationData.loop)
					startTime = Time.time;
				else
					Done = true;
			}
		}

		void ScaleAnimation ()
		{
			float scaleFactor = animationData.animationCurveScale.Evaluate (timeFactor);
			objToMove.localScale = Vector3.one * scaleFactor;
		}

		void MoveAnimation ()
		{
			Vector3 newPos;

			if (destination != startPos)
				newPos = Vector3.Lerp (startPos, destination, timeFactor);
			else
			{
				newPos = objToMove.position;

				//If the object didnt got moved, nullify offset
				if (lastPos == objToMove.position)
					newPos -= lastOffset;
			}

			float xOffset = 0f;
			float yOffset = 0f;
			
			if (animationData.animationCurveX != null
				&& animationData.animationCurveX.length > 0)
				xOffset = animationData.animationCurveX.Evaluate (timeFactor);

			if (animationData.animationCurveY != null
				&& animationData.animationCurveY.length > 0)
				yOffset = animationData.animationCurveY.Evaluate (timeFactor);

			lastOffset = new Vector3(xOffset, yOffset);
			newPos += lastOffset;

			Rigidbody2D rigidbody2D = objToMove.GetComponent<Rigidbody2D>();

			if (rigidbody2D != null && !rigidbody2D.isKinematic)
				rigidbody2D.MovePosition (newPos);
			else
				objToMove.position = newPos;
			
			lastPos = newPos;
		}
	}
}
