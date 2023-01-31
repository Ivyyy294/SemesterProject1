using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration
{
	public PlayerInput Input { get; set;}
	public int PlayerIndex {get; set;}
	public int TeamIndex {get; set;}
	public bool IsReady { get; set; }

	public PlayerConfiguration (PlayerInput pi)
	{
		PlayerIndex = pi.playerIndex;
		Input = pi;
	}
}

public class PlayerConfigurationDisplay : MonoBehaviour
{
	[SerializeField] RuntimeAnimatorController animatorControllerT1;
	[SerializeField] RuntimeAnimatorController animatorControllerT2;
	[SerializeField] float cameraDistanceFourPlayer = 4.21875f;

	[Header ("Recording")]
	[SerializeField] bool cinematicMode = false;
	[SerializeField] float cameraDistanceCinematic = 12.656250f;

	[Header ("Lara Values")]
	[SerializeField] SpawnPlayerSetupMenu menuScript;
	[SerializeField] GameObject player;
	[SerializeField] GameObject playerUi;
	[SerializeField] Camera pCamera;
	[SerializeField] SpriteRenderer spriteRendererPlayer;

	public PlayerConfiguration playerConfiguration;

	public void SpawnPlayer (Vector3 pos)
	{
		player.SetActive (true);

		if (!cinematicMode)
			EnableUi (true);
		else
		{
			if (spriteRendererPlayer != null)
				spriteRendererPlayer.enabled = false;

			Rigidbody2D rigidbody2D = player.GetComponent <Rigidbody2D>();

			if (rigidbody2D != null)
				rigidbody2D.isKinematic = true;
		}

		player.GetComponent<PlayerMotor>().InitPlayer (playerConfiguration);
		player.transform.position = pos;
		
		Animator animator = player.GetComponentInChildren<Animator>();

		if (playerConfiguration.TeamIndex == 0)
			animator.runtimeAnimatorController = animatorControllerT1;
		else
			animator.runtimeAnimatorController = animatorControllerT2;
	}

	public void EnableUi (bool val)
	{
		playerUi.SetActive (val);
	}

    // Start is called before the first frame update
    void Start()
    {
        menuScript.Show();

		if (cinematicMode)
			pCamera.orthographicSize = cameraDistanceCinematic;
		else if (PlayerManager.Me.MaxPlayers == 4)
			pCamera.orthographicSize = cameraDistanceFourPlayer;
    }
}
