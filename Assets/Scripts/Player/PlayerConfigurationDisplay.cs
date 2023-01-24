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

	[Header ("Lara Values")]
	[SerializeField] SpawnPlayerSetupMenu menuScript;
	[SerializeField] GameObject player;
	[SerializeField] GameObject playerUi;

	public PlayerConfiguration playerConfiguration;

	public void SpawnPlayer (Vector3 pos)
	{
		player.SetActive (true);
		playerUi.SetActive (true);
		player.GetComponent<PlayerMotor>().InitPlayer (playerConfiguration);
		player.transform.position = pos;
		
		Animator animator = player.GetComponentInChildren<Animator>();

		if (playerConfiguration.TeamIndex == 0)
			animator.runtimeAnimatorController = animatorControllerT1;
		else
			animator.runtimeAnimatorController = animatorControllerT2;
	}

    // Start is called before the first frame update
    void Start()
    {
        menuScript.Show();
    }
}
