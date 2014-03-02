using UnityEngine;
using System.Collections;

public class MeController : PlayerController {

	private LocationRequester locationRequester;

	private string errorOutput;
	private string output;

	// Use this for initialization
	void Start () {
		// TODO
		// start a timer that requests location and transforms itself
		// (or sends to server to wait until it sets all player's positions)

		locationRequester = new LocationRequester(this);
	}

	// Update is called once per frame
	void Update () {
		errorOutput = "hello " + locationRequester.output;

		Vector3 location = locationRequester.GetLocation();
		output = "location: " + location.x + ",  " + location.z;

		location.x -=  8.082f;
		location.z -= 49.884f;

		location.x *= 10000;
		location.z *= 10000;

		output += "\nmaps to : " + location.x + ",  " + location.z;
		transform.position = location;
	}


	void OnGUI() {
		GUI.Label(new Rect(10, 10, 400, 900), errorOutput);
		GUI.Label(new Rect(10, 100, 400, 900), output);
	}
}
