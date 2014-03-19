using AssemblyCSharp;
using System.Collections;
using System.Text;
using UnityEngine;

public class MenuWaitingRoom : MonoBehaviour
{
    private  float SyncRate = 0.7f;
    private Vector2 scrollPosition = Vector2.zero;

    // Use this for initialization
    void Start()
    {
        if (Shared.gameInstance == null) {
            throw new UnityException("There is no game instance!");
        }

        AddPlayerToGame();
        StartCoroutine(SyncGame());
    }

    // Update is called once per frame
    void Update()
    {
        // TODO if not creator, wait for event to start game (maybe just a state inside shared e.g.)
        // FIXME
        //		if (Shared.gameInstance.gameStarted) {
        //			Application.LoadLevel("game");
        //		}
    }

    void OnGUI()
    {

        // title
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Waiting Room", Shared.TitleStyle);

        GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.1f), "Player", Shared.ListLabelStyle);
        GUI.Label(new Rect(Screen.width * 0.6f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.1f), "Team", Shared.ListLabelStyle);

        // player list
        if (Shared.gameInstance.Players.Count > 0) {
            GUI.BeginScrollView(new Rect(Screen.width * 0.2f, Screen.height * 0.25f, Screen.width * 0.6f, Screen.height * 0.75f), scrollPosition, new Rect(0, 0, Screen.width * 0.6f, Screen.height * 0.75f));

            int heightOffset = 1;
            foreach (var player in Shared.gameInstance.Players) {
                GUI.Label(new Rect(0, 0, Screen.width * 0.2f * heightOffset, Screen.height * 0.1f), player.Id.ToString());
                GUI.Label(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.2f * heightOffset, Screen.height * 0.1f), "unknown");
                heightOffset++;
            }

            GUI.EndScrollView(); 
        }

        // next button
        if (Shared.creator) {
            if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.8f, Screen.width * 0.6f, Screen.height * 0.1f), "Next", Shared.ButtonStyle)) {
                Application.LoadLevel("game");
            }
        }
    }

    private void AddPlayerToGame()
    {
        string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
        PlayerModel player = new PlayerModel(uniqDeviceId.GetHashCode());
        player.Team = new TeamModel(0);
        Shared.gameInstance.Players.Add(player);
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
        //Debug.Log("Ref player count: " + Shared.gameInstance.Players.Count);
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
        //Debug.Log("PlayerCount from response game: " + game.Players.Count);
        Shared.gameInstance = game;
    }
}
