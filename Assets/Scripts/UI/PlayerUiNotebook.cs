using TMPro;
using UnityEngine;

public class PlayerUiNotebook : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI valueText;

	//Private Values
	Team team = null;

	//Public Functions
	public void SetTeam (Team t)
	{
		team = t;
	}

    // Update is called once per frame
    void Update()
    {
        if (valueText != null && team != null)
			valueText.text = ((int)team.Reputation).ToString();
    }
}
