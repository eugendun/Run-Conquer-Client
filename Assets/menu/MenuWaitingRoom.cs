using UnityEngine;
using System.Collections;

public class MenuWaitingRoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// TODO if not creator, wait for event to start game (maybe just a state inside shared e.g.)
		// FIXME
//		if (Shared.gameInstance.gameStarted) {
//			Application.LoadLevel("game");
//		}
	}
	
	void Next() {
		Application.LoadLevel("game");
	}
	
	void OnGUI() {
		
		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "Waiting Room", Shared.TitleStyle);
		
		GUI.Label(new Rect(50, 150, 500, 60), "Player", Shared.ListLabelStyle);
		GUI.Label(new Rect(800, 150, 300, 60), "Team", Shared.ListLabelStyle);

		GUI.Label(new Rect(0, 190, 1080, 40), "_________________________________________________________", Shared.LabelStyle);

		// players
		// TODO list each player from gameInstance
		
		// next
		if (Shared.creator) {
			if (GUI.Button(new Rect(0, 1600, 1080, 150), "Next", Shared.ButtonStyle)) {
				Next();
			}
		}
	}
}
