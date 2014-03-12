using UnityEngine;
using System.Collections;

public class MenuTeamSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(0, 0, 1080, 800), "Blue", Shared.ButtonStyle)) {
			Shared.teamId = 1;
			Application.LoadLevel("game");
		}
		if (GUI.Button(new Rect(0, 800, 1080, 800), "Red", Shared.ButtonStyle)) {
			Shared.teamId = 0;
			Application.LoadLevel("game");
		}
	}
}
