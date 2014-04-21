using UnityEngine;
using System.Collections;

public class MenuWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Next() {
		Application.LoadLevel("menuStart");
	}
	
	void OnGUI() {
		
		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "And the Winner is ...", Shared.TitleStyle);
		GUI.DrawTexture(new Rect(10, 10, 100, 100), Shared.iconTexture);
		
		GUI.Label(new Rect(0, 900, 1080, 200), "Team " + Shared.winnerTeam.ToString(), Shared.TitleStyle);
		
		// next
		if (Shared.creator) {
			if (GUI.Button(new Rect(0, 1600, 1080, 150), "Again", Shared.ButtonStyle)) {
				Next();
			}
		}
	}
}
