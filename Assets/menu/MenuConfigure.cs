using UnityEngine;
using System.Collections;

public class MenuConfigure : MonoBehaviour, MapListener {

	private MapRequester mapRequester;
	private Texture2D mapTexture;

	private int zoom = 17;

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
		Shared.mapZoom = zoom;
		//Application.LoadLevel("menuWaitingRoom");
		// FIXME
		Application.LoadLevel("game");
	}


	void OnGUI() {
		
		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "Configure Game", Shared.TitleStyle);

		if (mapTexture != null) {
			GUI.DrawTexture(new Rect(0, 120, 1080, 1080), mapTexture);
		}

		// zoom buttons
		if (GUI.Button(new Rect(0, 1200, 540, 150), "-", Shared.ButtonStyle)) {
			zoom = Mathf.Max(0, zoom - 1);
			RequestMap();
		}
		if (GUI.Button(new Rect(540, 1200, 540, 150), "+", Shared.ButtonStyle)) {
			zoom = Mathf.Max(19, zoom + 1);
			RequestMap();
		}

		// next
		if (GUI.Button(new Rect(240, 1500, 640, 300), "Next", Shared.ButtonStyle)) {
			Next();
		}
	}
}
