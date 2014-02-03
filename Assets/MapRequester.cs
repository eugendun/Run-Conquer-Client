using UnityEngine;
using System.Collections;

public class MapRequester : MonoBehaviour {
	
	public Material material;
	
	public string output;
	
	// Use this for initialization
	IEnumerator Start () {
		//string mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=Mainz&zoom=14&size=600x600&sensor=true";
		//string mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=Mainz&zoom=16&size=600x600&sensor=true";
		string mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=49.9945287,8.2630081&zoom=17&size=600x600&sensor=true";
		string apiKey = "AIzaSyBJCOY2_vCFdIBtwR-0zE21bzLjuqKBEkU";
		
		string url = mapUrl + "&key=" + apiKey;
		
		// 8.273144,50,16
		
//		string url1 = "http://www.canadianpetconnection.com/wp-content/uploads/2011/09/Cats1.jpg";
//		string url2 = "http://api.tiles.mapbox.com/v3/examples.map-zr0njcqy/8.273144,50,16/512x512.png";
//		string url3 = "http://ojw.dev.openstreetmap.org/StaticMap/?lat=50&lon=8.273144&z=16&mode=Export&show=1";
//		string url4 = "http://staticmap.openstreetmap.de/staticmap.php?center=50.000000,8.273144&zoom=16&size=512x512&markers=0.000000,0.000000,red-pushpin";
//		string url5 = "http://api.tiles.mapbox.com/v3/examples.map-zr0njcqy/8.273144,50,16/512x512.png";
		WWW www = new WWW(url);
		yield return www;
		renderer.material.mainTexture = www.texture;
		output = www.error;
		//material.mainTexture = www.texture;
	}
	
	// Update is called once per frame
	void OnGUI () {
		Rect rect = new Rect(0, 0, 800, 800);
		GUI.Label(rect, output);
	}
}
