using UnityEngine;
using AssemblyCSharp;
using System.Text;

public class menuStart : MonoBehaviour {

	private bool decided = false;

	// Use this for initialization
	void Start () {
		Shared.Initialize(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (decided && Shared.LocationRequester.LocationEnabled) {

			// DEBUG: create map parameters, if creator
			if (Shared.creator) {
				Shared.mapLatLon = Shared.LocationRequester.GetLocation();
			} else {
                Shared.mapLatLon = Shared.gameInstance.Map.LatLon;
                Shared.mapZoom = Shared.gameInstance.Map.Zoom;
                Shared.mapSize = Shared.gameInstance.Map.Size;
			}

			Application.LoadLevel("menuTeamSelection");
		}
	}

	void OnGUI() {
		// title
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Run & Conquer", Shared.TitleStyle);
        GUI.DrawTexture(new Rect(10, 10, 100, 100), Shared.iconTexture);

		// show waiting screen until location has been tracked
		if (decided && !Shared.LocationRequester.LocationEnabled) {
            GUI.Label(new Rect(0, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f), "Requesting location, please wait ... ", Shared.LabelStyle);
		} else {
            if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.32f, Screen.width * 0.5f, Screen.height * 0.16f), "Create Game", Shared.ButtonStyle)) {
				CreateNewGame();
                RefreshGameOnServer();
				Shared.creator = true;
				decided = true;
			}
            if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.66f, Screen.width * 0.5f, Screen.height * 0.16f), "Join Game", Shared.ButtonStyle)) {
                LoadGame();
				Shared.creator = false;
				decided = true;
			}
		}
	}

    private void RefreshGameOnServer()
    {
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
    }

    private void CreateNewGame()
    {
        string apiCall = Shared.GetApiCallUrl("GameInstance/PostGameInstance");
        var data = Encoding.ASCII.GetBytes("{}");   // send an empty json object to create new game instance
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
	}

    private void LoadGame()
    {
        string apiCall = Shared.GetApiCallUrl("GameInstance/GetLastGameInstance");
        WWW webClient = new WWW(apiCall);
        while (!webClient.isDone) {
            // wait until request is done
        }
        if (!string.IsNullOrEmpty(webClient.error)) {
            throw new UnityException("Game instance could not be created on the server!");
        }
        string jsonGame = Encoding.ASCII.GetString(webClient.bytes);
        GameInstanceModel game = GameInstanceModel.FromJson(jsonGame);
        Shared.gameInstance = game;
	}
}
