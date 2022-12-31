using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivyyy
{
    public class Utils
    {
		public static float RoundF (float val, uint dp)
		{
			float tmp = Mathf.Pow (10, dp);
			return Mathf.Round(val * tmp) / tmp;
		}


		public static Texture2D FillTexture2D (Texture2D texture, Color color)
		{
			var fillColorArray = texture.GetPixels ();

			for (int i = 0; i < fillColorArray.Length; ++ i)
				fillColorArray[i] = color;

			texture.SetPixels (fillColorArray);

			return texture;

			/*
			 for (int x = 0; x < texture.width; ++x)
			{
				for (int y = 0; y < texture.width; ++y)
					texture.SetPixel (x, y, color);
			}

			return texture;
			*/
		}
		public static Vector2 WorldToGuiPoint2D (Vector2 worldPos)
		{
			return WorldToGuiPoint2D (worldPos, Camera.main);
		}
		public static Vector2 WorldToGuiPoint2D (Vector2 worldPos, Camera cam)
		{
			Vector2 guiPos = cam.WorldToScreenPoint (worldPos);
			return guiPos;
		}

		public static Vector3 GetMouseWorldPosition()
		{
			Vector3 vec = Input.mousePosition;
			vec.z = Camera.main.nearClipPlane;
			vec = GetMouseWorldPositionWithZ (vec, Camera.main);
			return vec;
		}

		public static Vector3 GetMouseWorldPositionWithZ()
		{
			return GetMouseWorldPositionWithZ (Input.mousePosition, Camera.main);
		}

		public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
		{
			return GetMouseWorldPositionWithZ (Input.mousePosition, worldCamera);
		}

		public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
		{
			Vector3 worldPosition = worldCamera.ScreenToWorldPoint (screenPosition);
			return worldPosition;
		}
    }
}
