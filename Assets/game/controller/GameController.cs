using AssemblyCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameController : MonoBehaviour, MapListener
{
    protected const float SyncRate = 1.0f;

    private Map map;

    private List<PlayerController> PlayerControllerList = new List<PlayerController>();

    private Texture2D headerTexture;
    private Texture2D iconTexture;

    private TimeSpan leftTime;

    // Use this for initialization
    void Start()
    {
        if (playersGO == null)
        {
            playersGO = new GameObject("Players");
        }

        gameObject.AddComponent<Map>();
        map = gameObject.GetComponent<Map>();
        map.tilesX = 14;
        map.tilesY = 15;

        map.listener = this;
        map.Create(Shared.mapLatLon, Shared.mapZoom, Shared.mapSize);

        headerTexture = Resources.Load<Texture2D>("textures/game_header");
        iconTexture = Resources.Load<Texture2D>("textures/iTunesArtwork");

    }

    public Transform PlayerPrefab;
    GameObject playersGO = null;

    public void mapDidLoad(Texture2D texture)
    {
        StartCoroutine(SyncGame());
    }

    protected IEnumerator SyncGame()
    {
        while (true)
        {
            PushPlayerToServer();
            PullGameStateFromServer();
            yield return new WaitForSeconds(SyncRate);
        }
    }

    private void PushPlayerToServer()
    {
        if (Shared.player != null)
        {
            string apiCall = Shared.GetApiCallUrl("Player/PutPlayer");
            var data = Encoding.ASCII.GetBytes(Shared.player.ToJsonString());
            WWW webClient = new WWW(apiCall, data, Shared._headers);
            while (!webClient.isDone)
            {
                // wait until request is done
            }

            if (!string.IsNullOrEmpty(webClient.error))
            {
                throw new UnityException("Player could not be refreshed on the server!");
            }

        }
    }

    private void PullGameStateFromServer()
    {
        GameInstanceModel game = GetGameFromServer();
        foreach (var player in game.Players)
        {
            var pc = PlayerControllerList.Find(p => p.Player.Id == player.Id);
            if (pc == null)
            {
                pc = CreateNewPlayerController(player);
                PlayerControllerList.Add(pc);
            }

            if (player.Id != Shared.player.Id)
            {
                pc.Player.Position.x = player.Position.x;
                pc.Player.Position.y = player.Position.y;
            }
        }
    }

    private GameInstanceModel GetGameFromServer()
    {
        string apiCall = Shared.GetApiCallUrl(string.Format("GameInstance/GetGameInstance/{0}", Shared.gameInstance.Id));
        WWW webClient = new WWW(apiCall);
        while (!webClient.isDone)
        {
            // wait until request is done
        }

        if (!string.IsNullOrEmpty(webClient.error))
        {
            throw new UnityException(webClient.error);
        }

        string jsonGame = Encoding.ASCII.GetString(webClient.bytes);
        GameInstanceModel game = GameInstanceModel.FromJson(jsonGame);

        return game;
    }

    private PlayerController CreateNewPlayerController(PlayerModel player)
    {
        GameObject playerGameObject = (GameObject)Instantiate(PlayerPrefab);
        playerGameObject.transform.position = new Vector3(player.Position.x, 0.0f, player.Position.y);
        playerGameObject.transform.parent = playersGO.transform;
        playerGameObject.AddComponent<PlayerController>();

        PlayerController playerController;
        if (player.Id == Shared.player.Id)
        {
            playerGameObject.AddComponent<MeController>();
            playerGameObject.tag = "MeController";

            playerController = playerGameObject.GetComponent<MeController>();

            playerController.Player = Shared.player;

            // pass me to camera controller
            CameraController cameraController = gameObject.GetComponent<CameraController>();
            cameraController.player = playerController.transform;
        }
        else
        {
            playerGameObject.AddComponent<PlayerController>();
            playerController = playerGameObject.GetComponent<PlayerController>();

            playerController.Player = player;
        }

        if (player.Team != null)
        {
            var mat = Resources.Load("materials/" + player.Team.Color, typeof(Material)) as Material;
            if (mat != null)
            {
                playerGameObject.renderer.material = mat;
            }
        }

        playerController.map = map;         // remove it later, maybe it is not necessary

        return playerController;
    }

    // Update is called once per frame
    void Update()
    {
        //leftTime = Shared.gameInstance.EndDate.Value.Subtract(DateTime.Now);
        //if (leftTime != TimeSpan.Zero)
        //{
        //    // evaluate winning team
        //    int[] teamAccu = new int[] { 0, 0, 0, 0 };
        //    foreach (GameObject mapTile in map.MapTiles)
        //    {
        //        MapTileController mapTileController = mapTile.GetComponent<MapTileController>();
        //        if (mapTileController.team != null)
        //        {
        //            teamAccu[mapTileController.team.internalId]++;
        //        }
        //    }
        //    int maxValue = 0;
        //    int maxTeamId = 0;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (teamAccu[i] > maxValue)
        //        {
        //            maxValue = teamAccu[i];
        //            maxTeamId = i;
        //        }
        //    }
        //    Shared.winningTeamId = maxTeamId;
        //    Application.LoadLevel("menuWin");
        //}

        // flip map tile
        if (map.MapTiles != null)
        {
            foreach (GameObject mapTile in map.MapTiles)
            {
                MapTileController mapTileController = mapTile.GetComponent<MapTileController>();
                mapTileController.occupants.Clear();

                foreach (PlayerController player in PlayerControllerList)
                {
                    if (Mathf.Pow(mapTile.transform.position.x - player.transform.position.x, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS &&
                        Mathf.Pow(mapTile.transform.position.z - player.transform.position.z, 2) < Map.TILE_RADIUS * Map.TILE_RADIUS)
                    {
                        mapTileController.occupants.Add(player);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 1080, 120), headerTexture);
        GUI.DrawTexture(new Rect(10, 10, 100, 100), iconTexture);

        GUI.Label(new Rect(0, 0, 1080, 100), leftTime.Minutes + ":" + leftTime.Seconds + " min", Shared.TitleStyle);

        if (Shared.InDebug)
        {
            var style = new GUIStyle(GUI.skin.label);
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 20;

            int hOffset = 0;
            GUI.color = Color.yellow;
            GUI.BeginGroup(new Rect(10, 300, 300, 100));
            foreach (var pc in PlayerControllerList)
            {
                var content = string.Format("ID: {0}, Pos: {1}, {2}", pc.Player.Id, pc.Player.Position.x, pc.Player.Position.y);
                GUI.Label(new Rect(0, 40 * hOffset, 300, 40), content, style);
                hOffset++;
            }
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(Screen.width * 0.6f, Screen.height * 0.6f, (float)Screen.width, (float)Screen.height));
            float w = Screen.width * 0.4f;
            float h = Screen.height * 0.4f;
            if (GUI.Button(new Rect(0f, 0f, w, h * 0.3f), "Up"))
            {
                Shared.player.Position.y += 0.5f;
            }
            if (GUI.Button(new Rect(0f, h * 0.3f, w * 0.5f, h * 0.3f), "Left"))
            {
                Shared.player.Position.x -= 0.5f;
            }
            if (GUI.Button(new Rect(w * 0.5f, h * 0.3f, w * 0.5f, h * 0.3f), "Right"))
            {
                Shared.player.Position.x += 0.5f;
            }
            if (GUI.Button(new Rect(0f, h * 0.3f * 2, w, h * 0.3f), "Down"))
            {
                Shared.player.Position.y -= 0.5f;
            }
            GUI.EndGroup();
        }
    }
}