using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameController : MonoBehaviour {

	public Transform spawnThis;
	public MeController me;

	private MapCreator mapCreator;

	private GameInstanceModel gameModel;
	private List<PlayerController> opponents;

	private string output;

	// Use this for initialization
	void Start () {
		// TODO
		// create map with coordinates from game model
//		gameModel.Map.

		// DEBUG
//		Vector2 center = new Vector2(49.9945287f, 8.2630081f);
//		int     zoom   = 17;
		Vector2 center = new Vector2(49.88433f, 8.082729f);
		int     zoom   = 19;
		Vector2 size   = new Vector2(1000, 1000);

		gameObject.AddComponent<MapCreator>();
		mapCreator = gameObject.GetComponent<MapCreator>();
		mapCreator.center = center;
		mapCreator.zoom = zoom;
		mapCreator.size = size;
		mapCreator.spawnThis = spawnThis;
		mapCreator.x = 10;
		mapCreator.y = 11;

		// read player (opponents and me) models from game model and create controllers

		// DEBUG
		// create me game object

		// TODO
		// create location requester and give it information to transform geo-coordinates into game-space
		Rect mapLatLonRect = GoogleMapsTransformation.getCorners(center, zoom, (int)size.x, (int)size.y);
		output = "(" + mapLatLonRect.x + ", " + mapLatLonRect.y + ", " + mapLatLonRect.width + ", " + mapLatLonRect.height + ")";
		LocationRequester locationRequester = new LocationRequester(this);
		locationRequester.latLongBounds = mapLatLonRect;
		me.locationRequester = locationRequester;
		me.map = mapCreator;
	}

	void OnGUI() {
		GUI.Label(new Rect(700, 20, 300, 300), output);
	}
	
	// Update is called once per frame
	void Update () {
		// transform camera such that is always on top of ME
		Vector3 pos = transform.position;
//		me.transform.position = new Vector3(mapCreator.getMapSize().x/2, 0.0f, mapCreator.getMapSize().y/2);
		transform.position = new Vector3(me.transform.position.x, pos.y, me.transform.position.z);

		// flip map tile
		foreach (GameObject mapTile in mapCreator.MapTiles) {
			if (Mathf.Pow(mapTile.transform.position.x - me.transform.position.x, 2) < MapCreator.TILE_RADIUS * MapCreator.TILE_RADIUS &&
			    Mathf.Pow(mapTile.transform.position.z - me.transform.position.z, 2) < MapCreator.TILE_RADIUS * MapCreator.TILE_RADIUS &&
			    mapTile.GetComponent<MapTileController>().team == null) {
				mapTile.GetComponent<MapTileController>().team = me.Player.Team;
				mapTile.GetComponent<MapTileController>().Flip();
				output += "\nflip";
			}
		}
	}
}