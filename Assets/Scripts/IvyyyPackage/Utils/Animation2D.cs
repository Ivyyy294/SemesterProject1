using UnityEngine;

namespace Ivyyy
{
	[System.Serializable]
	public class AnimationData2D
	{
		public AnimationCurve animationCurveX = null;
		public AnimationCurve animationCurveY = null;
		public AnimationCurve animationCurveScale = null;
		public float animationDuration;
		public bool loop = false;
	}
	public class Animation2D
	{
		AnimationData2D animationData;
		Transform objToMove;
		Vector3 startPos;
		Transform destination;
		float startTime;
		float timeFactor;
		public bool Done { get; private set;}

		//Public Functions
		public void Init (Transform obj, AnimationData2D data)
		{
			Init (obj, null, data);
		}

		public void Init (Transform obj, Transform dest, AnimationData2D data)
		{
			startTime = Time.time;
			animationData = data;
			objToMove = obj;
			startPos = obj.position;
			destination = dest;
			timeFactor = 0f;
			Done = false;
		}

		public void Next()
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

		//PrivateFunctions
		void ScaleAnimation ()
		{
			float scaleFactor = animationData.animationCurveScale.Evaluate (timeFactor);
			objToMove.localScale = Vector3.one * scaleFactor;
		}

		void MoveAnimation ()
		{
			if (destination != null)
			{
				Vector3 pos = Vector3.Lerp (startPos, destination.position, timeFactor);
				objToMove.position = pos;
			}

			float xOffset = 0f;
			float yOffset = 0f;
			
			if (animationData.animationCurveX.length > 0)
				xOffset = animationData.animationCurveX.Evaluate (timeFactor);

			if (animationData.animationCurveY.length > 0)
				yOffset = animationData.animationCurveY.Evaluate (timeFactor);

			objToMove.localPosition += new Vector3(xOffset, yOffset);
		}
	}
}
