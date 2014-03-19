using UnityEngine;
using System.Collections;

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
		Shared.mapZoom = zoom;
		Shared.playTime = time * 60;
		Application.LoadLevel("menuWaitingRoom");
	}


	void OnGUI() {
		
		// title
		GUI.Label(new Rect(0, 0, 1080, 100), "Configure Game", Shared.TitleStyle);
		GUI.DrawTexture(new Rect(10, 10, 100, 100), Shared.iconTexture);

		if (mapTexture != null) {
			GUI.DrawTexture(new Rect(0, 120, 1080, 1080), mapTexture);
		}

		// zoom buttons
		if (GUI.Button(new Rect(0, 1200, 540, 150), "-", Shared.ButtonStyle)) {
			zoom = Mathf.Max(0, zoom - 1);
			RequestMap();
		}
		if (GUI.Button(new Rect(540, 1200, 540, 150), "+", Shared.ButtonStyle)) {
			zoom = Mathf.Min(19, zoom + 1);
			RequestMap();
		}
		
		// time buttons
		if (GUI.Button(new Rect(0, 1400, 400, 150), "-", Shared.ButtonStyle)) {
			time = Mathf.Max(5, time - 5);
		}
		GUI.Label(new Rect(400, 1400, 280, 150), time + " min", Shared.LabelStyle);
		if (GUI.Button(new Rect(680, 1400, 400, 150), "+", Shared.ButtonStyle)) {
			time += 5;
		}

		// next
		if (GUI.Button(new Rect(0, 1600, 1080, 150), "Next", Shared.ButtonStyle)) {
			Next();
		}
	}
}
