using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameController : MonoBehaviour, MapListener {

//	public Transform spawnThis;
	private MeController me;

	private Map map;

	private GameInstanceModel gameModel;
	private List<PlayerController> players = new List<PlayerController>();

	private string output;

	// Use this for initialization
	void Start () {
//		MapModel mapModel = new MapModel();
//		mapModel.Id = 10002;
//
//		gameModel = new GameInstanceModel();
//		gameModel.Id = 10001;
//		gameModel.Map = mapModel;

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

		map.listener = this;
		map.Create(Shared.mapLatLon, Shared.mapZoom, Shared.mapSize);
		output = "created map at " + "(" + Shared.mapLatLon.x + ", " + Shared.mapLatLon.y + ")";

		// TODO
		// read player (opponents and me) models from game model and create controllers

		// create players, when map has been loaded
	}
	
	public void mapDidLoad(Texture2D texture) {
		// DEBUG
		// create one player for each team
		for (int i = 0; i < 4; i++) {
			GameObject playerGameObject = Instantiate(Resources.Load<GameObject>("playerPrefab")) as GameObject;
			playerGameObject.AddComponent<MeshRenderer>();
			
			GameObject playerTeamGameObject = Instantiate(Resources.Load<GameObject>("playerTeamPrefab")) as GameObject;
			playerTeamGameObject.AddComponent<MeshRenderer>();
			
			playerTeamGameObject.transform.parent = playerGameObject.transform;
			playerGameObject.transform.Rotate(0, 0, 0);
			playerGameObject.transform.localScale = new Vector3(50, 50, 50);
			
			if (i == Shared.teamId) {
				playerGameObject.AddComponent<MeController>();
				me = playerGameObject.GetComponent<MeController>();
			} else {
				playerGameObject.AddComponent<KIPlayerController>();
			}
			
			PlayerController player = playerGameObject.GetComponent<PlayerController>();
			players.Add(player);
			player.teamId = i;
			player.map = map;
			player.TeamObject = playerTeamGameObject;
		}
		
		// pass me to camera controller
		CameraController cameraController = gameObject.GetComponent<CameraController>();
		cameraController.player = me.transform;
	}

	void OnGUI() {
		GUI.Label(new Rect(700, 20, 300, 300), output);
	}
	
	// Update is called once per frame
	void Update () {
		// transform camera such that is always on top of ME
		Vector3 pos = transform.position;
//		me.transform.position = new Vector3(mapCreator.getMapSize().x/2, 0.0f, mapCreator.getMapSize().y/2);
//		transform.position = new Vector3(me.transform.position.x, pos.y, me.transform.position.z);

		// flip map tile
		if (map.MapTiles != null) {
			foreach (GameObject mapTile in map.MapTiles) {
				MapTileController mapTileController = mapTile.GetComponent<MapTileController>();
				mapTileController.occupants.Clear();
				foreach (PlayerController player in players) {
					if (Mathf.Pow(mapTile.transform.position.x - player.transform.position.x, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS &&
					    Mathf.Pow(mapTile.transform.position.z - player.transform.position.z, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS) {
						mapTileController.occupants.Add(player);
					}
				}
			}
		}
	}
}