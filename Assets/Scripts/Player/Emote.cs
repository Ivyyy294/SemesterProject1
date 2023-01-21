using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Emote", menuName = "Emote")]
public class Emote : ScriptableObject
{
	public Sprite[] frames;
	public float fps = 1f;
	public AudioClip audio;
	[Range (0f, 1f)]
	public float volume = 1f;
}
