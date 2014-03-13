using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameController : MonoBehaviour {

//	public Transform spawnThis;
	private MeController me;

	private Map map;

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
		gameObject.AddComponent<Map>();
		map = gameObject.GetComponent<Map>();
//		map.spawnThis = spawnThis;// Resources.Load<Transform>("maptile");
		map.tilesX = 14;
		map.tilesY = 15;


		map.Create(Shared.mapLatLon, Shared.mapZoom, Shared.mapSize);
		output = "created map at " + "(" + Shared.mapLatLon.x + ", " + Shared.mapLatLon.y + ")";

		// TODO
		// read player (opponents and me) models from game model and create controllers

		// create player object
		GameObject playerGameObject = Instantiate(Resources.Load<GameObject>("playerPrefab")) as GameObject;
		playerGameObject.AddComponent<MeshRenderer>();

		GameObject playerTeamGameObject = Instantiate(Resources.Load<GameObject>("playerTeamPrefab")) as GameObject;
		playerTeamGameObject.AddComponent<MeshRenderer>();

		playerTeamGameObject.transform.parent = playerGameObject.transform;
		playerGameObject.transform.Rotate(0, 0, 0);
		playerGameObject.transform.localScale = new Vector3(50, 50, 50);
		playerGameObject.AddComponent<MeController>();
		me = playerGameObject.GetComponent<MeController>();
		me.map = map;
		me.TeamObject = playerTeamGameObject;
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
		if (map.MapTiles != null) {
			foreach (GameObject mapTile in map.MapTiles) {
				if (Mathf.Pow(mapTile.transform.position.x - me.transform.position.x, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS &&
				    Mathf.Pow(mapTile.transform.position.z - me.transform.position.z, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS &&
				    mapTile.GetComponent<MapTileController>().team == null) {
					mapTile.GetComponent<MapTileController>().team = me.Player.Team;
					mapTile.renderer.material = me.TeamObject.renderer.material;
					mapTile.GetComponent<MapTileController>().Flip();
				}
			}
		}
	}
}