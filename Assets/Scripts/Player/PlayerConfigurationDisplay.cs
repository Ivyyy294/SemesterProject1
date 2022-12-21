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
	[SerializeField] SpawnPlayerSetupMenu menuScript;
	[SerializeField] GameObject player;

	public PlayerConfiguration playerConfiguration;

	public void SpawnPlayer (Vector3 pos)
	{
		player.SetActive (true);
		player.GetComponent<PlayerMotor>().InitPlayer (playerConfiguration);
		player.transform.position = pos;
	}

    // Start is called before the first frame update
    void Start()
    {
        menuScript.Show();
    }
}
