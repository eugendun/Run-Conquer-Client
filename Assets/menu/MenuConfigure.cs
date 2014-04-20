using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Text;

public class MenuConfigure : MonoBehaviour, MapListener {

	private MapRequester mapRequester;
	private Texture2D mapTexture;

	private int zoom = 17;
	private int time = 5;		// in minutes

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MapRequester>();
		mapRequester = gameObject.GetComponent<MapRequester>();
		mapRequester.addListener(this);
		RequestMap();
	}


	private void RequestMap() {
		mapRequester.Request(Shared.mapLatLon, zoom, Shared.mapSize);
	}


	public void mapDidLoad(Texture2D texture) {
		mapTexture = texture;
	}


	void Next() {
		
        // GameInstance configuration

        Shared.mapZoom = zoom;
        
        Shared.gameInstance.Map = new MapModel { LatLon = Shared.mapLatLon, Size = Shared.mapSize, Zoom = zoom };
        Shared.gameInstance.StartDate = Shared.StartDate;
        Shared.gameInstance.EndDate = Shared.EndDate;

        RefreshGameOnServer();
		
		Application.LoadLevel("menuWaitingRoom");
	}


	void OnGUI() {
		
		// title
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Configure Game", Shared.TitleStyle);
        GUI.DrawTexture(new Rect(10, 10, 100, 100), Shared.iconTexture);

		if (mapTexture != null) {
            GUI.DrawTexture(new Rect(0, Screen.height * 0.1f, Screen.width, Screen.height * 0.8f), mapTexture);
		}

		// zoom buttons
        if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.85f, Screen.width * 0.2f, Screen.height * 0.04f), "-", Shared.ButtonStyle)) {
			zoom = Mathf.Max(0, zoom - 1);
			RequestMap();
		}
        if (GUI.Button(new Rect(Screen.width * 0.6f, Screen.height * 0.85f, Screen.width * 0.2f, Screen.height * 0.04f), "+", Shared.ButtonStyle)) {
			zoom = Mathf.Min(19, zoom + 1);
			RequestMap();
		}
		
		// time buttons
        if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.04f), "-", Shared.ButtonStyle)) {
			time = Mathf.Max(5, time - 5);
		}
		GUI.Label(new Rect(400, 1400, 280, 150), time + " min", Shared.LabelStyle);
        if (GUI.Button(new Rect(Screen.width * 0.6f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.04f), "+", Shared.ButtonStyle)) {
			time += 5;
		}

		// next
        if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.95f, Screen.width * 0.6f, Screen.height * 0.04f), "Next", Shared.ButtonStyle)) {
			Next();
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
}
