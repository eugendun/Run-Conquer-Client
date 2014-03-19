using UnityEngine;
using AssemblyCSharp;
using System.Text;

public class MenuStart : MonoBehaviour {

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
			}

			Application.LoadLevel("menuTeamSelection");
		}
	}

	void OnGUI() {

		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "Run & Conquer", Shared.TitleStyle);

		// show waiting screen until location has been tracked
		if (decided && !Shared.LocationRequester.LocationEnabled) {
			GUI.Label(new Rect(0, 900, 1080, 100), "Requesting location, please wait ... ", Shared.LabelStyle);
		} else {
            if (GUI.Button(new Rect(240, 600, 640, 300), "Create Game", Shared.ButtonStyle)) {
				CreateNewGame();
                SetupTeams();
                SetupMap();
                RefreshGameOnServer();
				Shared.creator = true;
				decided = true;
			}
			if (GUI.Button(new Rect(240, 1000, 640, 300), "Join Game", Shared.ButtonStyle)) {
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

    private void SetupMap()
    {
        Shared.gameInstance.Map = new MapModel();
		}

    private void SetupTeams()
    {
        Shared.gameInstance.Teams.Add(new TeamModel(1) { Name = "Red Team", Color = "red" });
        Shared.gameInstance.Teams.Add(new TeamModel(2) { Name = "Blue Team", Color = "blue" });
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
        //Shared.gameInstance = game;
	}
}
