using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MeController : PlayerController {

	private string errorOutput;

	public Map map;

	// Use this for initialization
	public void Start () {
		base.Start();
		Player.Team = new TeamModel(Shared.teamId);

		// TODO
		// start a timer that requests location and transforms itself
		// (or sends it to server to wait until it sets all player's positions)
		Player.Team = new TeamModel(Shared.teamId);
	}

	// Update is called once per frame
	void Update () {
		Vector2 location = Shared.LocationRequester.GetLocation();

		Rect latLonBounds = map.LatLonBounds;
		output = "\nmap bounds = (" + latLonBounds.x + ", " + latLonBounds.y + ", " + latLonBounds.width + ", " + latLonBounds.height + ")";
		output += "\nmap center = (" + (latLonBounds.x - latLonBounds.width/2) + ", " + (latLonBounds.y - latLonBounds.height/2) + ")";

		output += "\nlocation: " + location.x + ",  " + location.y;

		// calculate value between (0, 0) and (1, 1) in normalized map-space (lat -> y, lon -> x)
		Vector2 v = new Vector2();
		v.x = (location.y - (latLonBounds.center.y - latLonBounds.height/2)) / latLonBounds.height;
		v.y = (location.x - (latLonBounds.center.x - latLonBounds.width/2))  / latLonBounds.width;
		output += "\nmapspace: " + v.x + ",  " + v.y;

		// transform into world-space
		Vector2 mapSize = map.getMapSize();
		v.x *= mapSize.x;
		v.y *= mapSize.y;

		output += "\nmaps to : " + v.x + ",  " + v.y;
		transform.position = new Vector3(v.x, 0.0f, v.y);
	}


	void OnGUI() {
		base.OnGUI();
		GUI.Label(new Rect(10, 10, 400, 900), errorOutput);
	}
}
