using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarbourBell : MonoBehaviour
{
	[SerializeField] AudioClip audioBell;

    public void Interact()
	{
		if (audioBell != null && Ivyyy.AudioHandler.Me != null)
			Ivyyy.AudioHandler.Me.PlayOneShot (audioBell);
	}
}
