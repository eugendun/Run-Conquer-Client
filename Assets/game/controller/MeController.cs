using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Text;

public class MeController : PlayerController {

	private string errorOutput;

	// Use this for initialization
	public void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		Vector2 location = Shared.LocationRequester.GetLocation();

		Rect latLonBounds = map.LatLonBounds;
		output = "\nmap bounds = (" + latLonBounds.x + ", " + latLonBounds.y + ", " + latLonBounds.width + ", " + latLonBounds.height + ")";
		output += "\nmap center = (" + (latLonBounds.x - latLonBounds.width/2) + ", " + (latLonBounds.y - latLonBounds.height/2) + ")";

		output += "\nlocation: " + location.x + ",  " + location.y;

		// calculate value between (0, 0) and (1, 1) in normalized map-space (lat -> y, lon -> x)
		Vector2 v = new Vector2();
		v.x = (location.y - (latLonBounds.center.y - latLonBounds.height/2)) / latLonBounds.height;
		v.y = (location.x - (latLonBounds.center.x - latLonBounds.width/2))  / latLonBounds.width;
		output += "\nmapspace: " + v.x + ",  " + v.y;

		// transform into world-space
		Vector2 mapSize = map.getMapSize();
		v.x *= mapSize.x;
		v.y *= mapSize.y;

//		// DEBUG auto-walk
//		if (Mathf.Abs(v.x - transform.position.x) < 17) {
//			v.x = transform.position.x - 0.003f;
//			v.y = transform.position.z - 0.002f;
//		}

		output += "\nmaps to : " + v.x + ",  " + v.y;
        transform.position = new Vector3(v.x, 0.0f, v.y);

        base.Update();
        //Debug.Log("PlayerPos: " + this.Player.Position);

	}

    //protected IEnumerator SyncPosition ()
    //{
    //    while(true) {
    //        PutPosition ();
    //        yield return new WaitForSeconds(SyncRate);
    //    }
    //}
	
    //protected IEnumerator PutPosition ()
    //{
    //    string url = ServerPrefixUrl + "/api/Player/PutPlayer";
    //    byte[] data = Encoding.ASCII.GetBytes (ToJson ());
    //    WWW webClient = new WWW (url, data, _headers);
    //    yield return webClient;
    //}

//	void OnGUI() {
//		base.OnGUI();
//		GUI.Label(new Rect(10, 10, 400, 900), errorOutput);
//	}
}
