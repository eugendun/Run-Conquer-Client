using UnityEngine;
using System.Collections;

public class MeController : PlayerController {

	public LocationRequester locationRequester;

	private string errorOutput;
	private string output;

	public MapCreator map;

	// Use this for initialization
	public void Start () {
		base.Start();

		// TODO
		// start a timer that requests location and transforms itself
		// (or sends it to server to wait until it sets all player's positions)
		Player.Team = new AssemblyCSharp.TeamModel(0);
	}

	// Update is called once per frame
	void Update () {
		errorOutput = "Initializing location: " + locationRequester.output;

		Vector3 location = locationRequester.GetLocation();
		output = "location: " + location.x + ",  " + location.z;

//		location.x -=  8.082f;
//		location.z -= 49.884f;
//
//		location.x *= 10000;
//		location.z *= 10000;

//		location.x *= 5;
//		location.z *= 5;
		Vector2 mapSize = map.getMapSize();
		location.x *= mapSize.x;
		location.z *= mapSize.y;

		output += "\nmaps to : " + location.x + ",  " + location.z;
		transform.position = location;
	}


	void OnGUI() {
		GUI.Label(new Rect(10, 10, 400, 900), errorOutput);
		GUI.Label(new Rect(10, 100, 400, 900), output);
	}
}
