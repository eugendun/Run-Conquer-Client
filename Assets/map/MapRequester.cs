using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapRequester : MonoBehaviour {
	
	public Vector2 center;
	public int zoom;
	public Vector2 size;
	
	public string output;

	private List<MapListener> listeners = new List<MapListener>();
	public void addListener(MapListener listener) {
		listeners.Add(listener);
	}
	
	// Use this for initialization
	void Start () {
	}

	public void Request(Vector2 center, int zoom, Vector2 size) {
		this.center = center;
		this.zoom = zoom;
		this.size = size;
		StartCoroutine(Request());
	}

	public IEnumerator Request() {
		string mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + center.x + "," + center.y + "&zoom=" + zoom + "&size=" + (int)size.x + "x" + (int)size.y + "&sensor=true";
		string apiKey = "AIzaSyBJCOY2_vCFdIBtwR-0zE21bzLjuqKBEkU";
		
		string url = mapUrl + "&key=" + apiKey;
		output += "\n" + mapUrl;
		
		WWW www = new WWW(url);
		yield return www;
		
		// inform listeners
		foreach (MapListener listener in listeners) {
			listener.mapDidLoad(www.texture);
			yield return listener;
		}
		//renderer.material.mainTexture = www.texture;
		
		output = www.error;
	}
	
	// Update is called once per frame
	void OnGUI () {
		Rect rect = new Rect(300, 250, 700, 800);
		GUI.Label(rect, output);
	}
}
