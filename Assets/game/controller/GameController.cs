using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameController : MonoBehaviour, MapListener {

	private MeController me;

	private Map map;

	private GameInstanceModel gameModel;
	private List<PlayerController> players = new List<PlayerController>();

	private Texture2D headerTexture;
	private Texture2D iconTexture;

	private float leftTime;

	private string output;


	// Use this for initialization
	void Start () {
//		MapModel mapModel = new MapModel();
//		mapModel.Id = 10002;
//
		gameModel = new GameInstanceModel();
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

		headerTexture = Resources.Load<Texture2D>("/textures/game_header");
		iconTexture   = Resources.Load<Texture2D>("/textures/iTunesArtwork");

		leftTime = Shared.playTime;

		// TODO
		// read player (opponents and me) models from game model and create controllers

		// create players, when map has been loaded
	}
	
	public void mapDidLoad(Texture2D texture) {

		// FIXME DEBUG create one player
		string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
		PlayerModel model = new PlayerModel(uniqDeviceId.GetHashCode());
		model.Team = new TeamModel(0);
		gameModel.Players.Add(model);
		// END FIXME


		// create players
		foreach (PlayerModel playerModel in gameModel.Players) {
			GameObject playerGameObject = Instantiate(Resources.Load<GameObject>("playerPrefab")) as GameObject;
			playerGameObject.AddComponent<MeshRenderer>();
			
			GameObject playerTeamGameObject = Instantiate(Resources.Load<GameObject>("playerTeamPrefab")) as GameObject;
			playerTeamGameObject.AddComponent<MeshRenderer>();
			
			playerTeamGameObject.transform.parent = playerGameObject.transform;
			playerGameObject.transform.Rotate(0, 0, 0);
			playerGameObject.transform.localScale = new Vector3(50, 50, 50);
			
			if (playerModel.Id == uniqDeviceId.GetHashCode()) {
				playerGameObject.AddComponent<MeController>();
				me = playerGameObject.GetComponent<MeController>();
			} else {
				playerGameObject.AddComponent<PlayerController>();
			}
			
			PlayerController player = playerGameObject.GetComponent<PlayerController>();
			players.Add(player);
			player.Player = playerModel;
			player.map = map;
			player.TeamObject = playerTeamGameObject;
		}
		
		// pass me to camera controller
		CameraController cameraController = gameObject.GetComponent<CameraController>();
		cameraController.player = me.transform;
	}
	
	// Update is called once per frame
	void Update () {

		leftTime -= Time.deltaTime;

		if (leftTime <= 0) {
			// TODO
//			evaluate winning Team And // load win screen
		}

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
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0, 0, 1080, 150), headerTexture);
		
		// title (time)
		int minutes = (int)(leftTime) % 60;
		int seconds = (int)(leftTime - (minutes * 60));
		string minutesString = (minutes < 10)? "0" + minutes : minutes.ToString();
		string secondsString = (seconds < 10)? "0" + seconds : seconds.ToString();

		GUI.Label(new Rect(0, 0, 1080, 100), minutesString + ":" + secondsString + " min", Shared.TitleStyle);
	}
}