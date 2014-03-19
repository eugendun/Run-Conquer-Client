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
		GUI.Label(new Rect(0, 0, 1080, 100), "Select Team", Shared.TitleStyle);
		
		if (GUI.Button(new Rect(240, 200, 640, 300), "Rette", Shared.ButtonStyle)) {
			Shared.teamId = 0;
			Next();
		}
		if (GUI.Button(new Rect(240, 600, 640, 300), "Bloo", Shared.ButtonStyle)) {
			Shared.teamId = 1;
			Next();
		}
		if (GUI.Button(new Rect(240, 1000, 640, 300), "Griene", Shared.ButtonStyle)) {
			Shared.teamId = 2;
			Next();
		}
		if (GUI.Button(new Rect(240, 1400, 640, 300), "Yello", Shared.ButtonStyle)) {
			Shared.teamId = 3;
			Next();
		}
	}
}
