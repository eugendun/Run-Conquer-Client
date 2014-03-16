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

	void LoadGame() {

		Application.LoadLevel("menuRoadMap");
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(0, 0, 1080, 200), "Red", Shared.ButtonStyle)) {
			Shared.teamId = 0;
			LoadGame();
		}
		if (GUI.Button(new Rect(0, 200, 1080, 200), "Blue", Shared.ButtonStyle)) {
			Shared.teamId = 1;
			LoadGame();
		}
		if (GUI.Button(new Rect(0, 400, 1080, 200), "Green", Shared.ButtonStyle)) {
			Shared.teamId = 2;
			LoadGame();
		}
		if (GUI.Button(new Rect(0, 600, 1080, 200), "Silver", Shared.ButtonStyle)) {
			Shared.teamId = 3;
			LoadGame();
		}
	}
}
