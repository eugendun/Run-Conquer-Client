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

        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.1f), Shared.teamNames[0], Shared.ButtonStyle)) {
            Shared.teamId = 0;
            Next();
        }
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.1f), Shared.teamNames[1], Shared.ButtonStyle)) {
            Shared.teamId = 1;
            Next();
        }
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.6f, Screen.width * 0.5f, Screen.height * 0.1f), Shared.teamNames[2], Shared.ButtonStyle)) {
            Shared.teamId = 2;
            Next();
        }
        if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.1f), Shared.teamNames[3], Shared.ButtonStyle)) {
            Shared.teamId = 3;
            Next();
        }
	}
}
