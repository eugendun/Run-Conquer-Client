using UnityEngine;
using System.Collections;
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
		GUI.Label(new Rect(0, 0, 1080, 100), "Select Team", Shared.TitleStyle);
		
		if (GUI.Button(new Rect(240, 200, 640, 300), Shared.teamNames[0], Shared.ButtonStyle)) {
			Shared.teamId = 0;
			Next();
		}
		if (GUI.Button(new Rect(240, 600, 640, 300), Shared.teamNames[1], Shared.ButtonStyle)) {
			Shared.teamId = 1;
			Next();
		}
		if (GUI.Button(new Rect(240, 1000, 640, 300), Shared.teamNames[2], Shared.ButtonStyle)) {
			Shared.teamId = 2;
			Next();
		}
		if (GUI.Button(new Rect(240, 1400, 640, 300), Shared.teamNames[3], Shared.ButtonStyle)) {
			Shared.teamId = 3;
			Next();
		}
	}
}
