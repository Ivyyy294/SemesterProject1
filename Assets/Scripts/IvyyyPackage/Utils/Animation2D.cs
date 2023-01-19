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

			if (animationData.animationCurveScale != null)
				ScaleAnimation ();

			MoveAnimation ();

			if (timeFactor >= 1f)
				Done = true;
		}

		//PrivateFunctions
		void ScaleAnimation ()
		{
			float scaleFactor = animationData.animationCurveScale.Evaluate (timeFactor);
			objToMove.localScale = Vector3.one * scaleFactor;
		}

		void MoveAnimation ()
		{
			Vector3 pos = Vector3.Lerp (startPos, destination.position, timeFactor);
			float xOffset = 0f;
			float yOffset = 0f;
			
			if (animationData.animationCurveX.length > 0)
				xOffset = animationData.animationCurveX.Evaluate (timeFactor);

			if (animationData.animationCurveY.length > 0)
				yOffset = animationData.animationCurveY.Evaluate (timeFactor);

			objToMove.position = pos + new Vector3(xOffset, yOffset);
		}
	}
}
