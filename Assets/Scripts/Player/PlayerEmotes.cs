using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class PlayerEmotes : MonoBehaviour
{
	//Private Values
	private SpriteRenderer spriteRenderer;
	private float timer = 0f;
	private float frameTime = 0f;
	int currentFrame;
	Emote emote;

	//Public Functions

	public void PlayEmote (Emote e)
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponent <SpriteRenderer>();

		if (spriteRenderer != null && e != null)
		{
			if (e.frames.Length > 0)
				spriteRenderer.sprite = e.frames[0];

			timer = 0f;
			currentFrame = 0;
			frameTime = 1 / e.fps;
			emote = e;

			spriteRenderer.enabled = true;
			Ivyyy.AudioHandler.Me.PlayOneShot (e.audio, e.volume);
		}
		else
			Debug.Log ("Missing sprite renderer!");
	}

	//Private FUnctions
	void Update()
    {
		if (timer >= frameTime)
		{
			++currentFrame;
			timer = 0f;

			if (currentFrame >= emote.frames.Length)
				spriteRenderer.enabled = false;
			else if (spriteRenderer != null)
				spriteRenderer.sprite = emote.frames[currentFrame];
			
		
		}
		else
			timer += Time.deltaTime;
    }
}
