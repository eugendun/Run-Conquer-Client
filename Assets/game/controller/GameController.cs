using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System.Text;

public class GameController : MonoBehaviour, MapListener {
    protected const float SyncRate = 1.0f;

	private MeController me;

	private Map map;

	private List<PlayerController> players = new List<PlayerController>();

	private Texture2D headerTexture;
	private Texture2D iconTexture;

	private float leftTime;

	private string output;


	// Use this for initialization
    void Start()
    {
		gameObject.AddComponent<Map>();
		map = gameObject.GetComponent<Map>();
		map.tilesX = 14;
		map.tilesY = 15;

		map.listener = this;
		map.Create(Shared.mapLatLon, Shared.mapZoom, Shared.mapSize);
		output = "created map at " + "(" + Shared.mapLatLon.x + ", " + Shared.mapLatLon.y + ")";

		headerTexture = Resources.Load<Texture2D>("textures/game_header");
		iconTexture   = Resources.Load<Texture2D>("textures/iTunesArtwork");

		leftTime = Shared.playTime;
	}
	
    public void mapDidLoad(Texture2D texture)
    {
		string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;

		// TODO: @Eugen It is important, that me gets that team which internalId matches Shared.teamId.
		// Shared.teamId is this one, which was chosen inside menu. Team with internalId 0 is red (Rette), 1 is blue (Bloo), ... 3 is yellow (Yello)
		// Maybe, create all four Teams here (internalId is automatically set) and choose that one for ME with matching internalId

		// create players
		foreach (PlayerModel playerModel in Shared.gameInstance.Players) {
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

        StartCoroutine(SyncGame());
	}
	
    protected IEnumerator SyncGame()
    {
        while (true) {
            RefreshGameOnServer();
            yield return new WaitForSeconds(SyncRate);
        }
	}

    private void RefreshGameOnServer()
    {
        // as players in the list are decoupled from the shared game instance, we have to update position explicitly
        // find foreach player in the shared game instance and update the player position in the shared game instance
        foreach (var player in Shared.gameInstance.Players) {
            var playerToUpdate = players.Find(p => p.Player.Id == player.Id);
            if (playerToUpdate != null) {
                player.Position = playerToUpdate.Player.Position; 
            }
        }

        Debug.Log("Ref player count: " + Shared.gameInstance.Players.Count);
        string apiCall = Shared.GetApiCallUrl(string.Format("GameInstance/PutGameInstance/{0}", Shared.gameInstance.Id));
        var data = Encoding.ASCII.GetBytes(Shared.gameInstance.ToJson());
        WWW webClient = new WWW(apiCall, data, Shared._headers);
        while (!webClient.isDone) {
            // wait until request is done
        }
        if (!string.IsNullOrEmpty(webClient.error)) {
            throw new UnityException("Game instance could not be created on the server!");
        }
        string jsonGame = Encoding.ASCII.GetString(webClient.bytes);
        GameInstanceModel game = GameInstanceModel.FromJson(jsonGame);
        Shared.gameInstance = game;

        // as players in the list are decoupled from the shared game instance, we have to update position explicitly
        // find foreach player in the shared game instance and update the corresponded player model in the player list
        foreach (var player in Shared.gameInstance.Players) {
            var playerToUpdate = players.Find(p => p.Player.Id == player.Id);
            if (playerToUpdate != null) {
                playerToUpdate.Player.Position = player.Position; 
            }
        }
    }

	// Update is called once per frame
    void Update()
    {

		leftTime -= Time.deltaTime;

		if (leftTime <= 0) {
			// evaluate winning team
			int[] teamAccu = new int[] { 0, 0, 0, 0 };
			foreach (GameObject mapTile in map.MapTiles) {
				MapTileController mapTileController = mapTile.GetComponent<MapTileController>();
				if (mapTileController.team != null) {
					teamAccu[mapTileController.team.internalId]++;
				}
			}
			int maxValue = 0;
			int maxTeamId = 0;
			for (int i = 0; i < 4; i++) {
				if (teamAccu[i] > maxValue) {
					maxValue = teamAccu[i];
					maxTeamId = i;
				}
			}
			Shared.winningTeamId = maxTeamId;
			Application.LoadLevel("menuWin");
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
		GUI.DrawTexture(new Rect(0, 0, 1080, 120), headerTexture);
		GUI.DrawTexture(new Rect(10, 10, 100, 100), iconTexture);
		
		// title (time)
		int minutes = (int)(leftTime) / 60;
		int seconds = (int)(leftTime - (minutes * 60));
		string minutesString = (minutes < 10)? "0" + minutes : minutes.ToString();
		string secondsString = (seconds < 10)? "0" + seconds : seconds.ToString();

		GUI.Label(new Rect(0, 0, 1080, 100), minutesString + ":" + secondsString + " min", Shared.TitleStyle);
	}
}