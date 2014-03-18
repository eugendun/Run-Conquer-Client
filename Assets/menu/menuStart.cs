using UnityEngine;
using System.Collections;

public class MenuStart : MonoBehaviour {

	private bool decided = false;

	// Use this for initialization
	void Start () {
		Shared.Initialize(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (decided && Shared.LocationRequester.LocationEnabled) {

			// DEBUG: create map parameters, if creator
			if (Shared.creator) {
				Shared.mapLatLon = Shared.LocationRequester.GetLocation();
			}

			Application.LoadLevel("menuTeamSelection");
		}
	}

	void OnGUI() {

		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "Run & Conquer", Shared.TitleStyle);

		// show waiting screen until location has been tracked
		if (decided && !Shared.LocationRequester.LocationEnabled) {
			GUI.Label(new Rect(0, 900, 1080, 100), "Requesting location, please wait ... ", Shared.LabelStyle);
		} else {
			if (GUI.Button(new Rect(240, 600, 640, 300), "Create Game", Shared.ButtonStyle)) {
				Shared.creator = true;
				decided = true;
			}
			if (GUI.Button(new Rect(240, 1000, 640, 300), "Join Game", Shared.ButtonStyle)) {
				Shared.creator = false;
				decided = true;
			}
		}
	}
}
