using UnityEngine;
using System.Collections;
using System.Text;
using AssemblyCSharp;

public class MenuTeamSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Next() {

		if (Shared.creator) {
			Application.LoadLevel("menuConfigure");
		} else {
			Application.LoadLevel("menuWaitingRoom");
		}
	}
	
	void OnGUI() {
		
		// title
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Select Team", Shared.TitleStyle);
        GUI.DrawTexture(new Rect(10, 10, 100, 100), Shared.iconTexture);
		
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.1f), TeamColor.Bloo.ToString(), Shared.ButtonStyle)) {
            Shared.playerTeamColor = TeamColor.Bloo;
			Next();
		}
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.1f), TeamColor.Griene.ToString(), Shared.ButtonStyle)) {
            Shared.playerTeamColor = TeamColor.Griene;
			Next();
		}
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.6f, Screen.width * 0.5f, Screen.height * 0.1f), TeamColor.Rette.ToString(), Shared.ButtonStyle)) {
            Shared.playerTeamColor = TeamColor.Rette;
			Next();
		}
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.1f), TeamColor.Yello.ToString(), Shared.ButtonStyle)) {
            Shared.playerTeamColor = TeamColor.Yello;
			Next();
		}
	}
}
