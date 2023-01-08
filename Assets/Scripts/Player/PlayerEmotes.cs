using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Emote
{
	public Sprite sprite;
	public float duration = 1f;
	public AudioClip audio;
	[Range (0f, 1f)]
	public float volume = 1f;
}

[RequireComponent (typeof (SpriteRenderer))]
public class PlayerEmotes : MonoBehaviour
{
	//Private Values
	private SpriteRenderer spriteRenderer;
	private float timer = 0f;
	private float duration = 0f;

	//Public Functions

	public void PlayEmote (Emote e)
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponent <SpriteRenderer>();

		if (spriteRenderer != null && e != null)
		{
			spriteRenderer.sprite = e.sprite;
			timer = 0f;
			duration = e.duration;
			spriteRenderer.enabled = true;
			Ivyyy.AudioHandler.Me.PlayOneShot (e.audio, e.volume);
		}
		else
			Debug.Log ("Missing sprite renderer!");
	}

	//Private FUnctions

    // Update is called once per frame
    void Update()
    {
		if (timer >= duration)
		{
			if (spriteRenderer != null)
				spriteRenderer.enabled = false;
		}
		else
			timer += Time.deltaTime;
    }
}
