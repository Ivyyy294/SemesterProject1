using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
	[SerializeField] float timer = 1f;
	private float timeActive = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timeActive = 0f;
    }

	private void OnEnable()
	{
		timeActive = 0f;
	}

	// Update is called once per frame
	void Update()
    {
		if (timeActive >= timer)
			gameObject.SetActive (false);

        timeActive += Time.deltaTime;
    }
}
