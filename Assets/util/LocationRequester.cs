using UnityEngine;
using System.Collections;

public class LocationRequester {

	public string output;

	private bool locationEnabled = false;
	public bool LocationEnabled { get { return locationEnabled; } }

//	private float GPSUpdateInterval = 1.0f;

	public LocationRequester(MonoBehaviour caller) {
		caller.StartCoroutine( Start() );
	}

	// Use this for initialization
	private IEnumerator Start() {
		output = "started";

		// First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
			output = "location not enabled!";
             yield return false;
		}
		
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			output += " " + maxWait + " seconds left";
			yield return (new WaitForSeconds(1));
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
			output = "Timed out";
            yield return false;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
			output = "Unable to determine device location";
            yield return false;
        } else {
			locationEnabled = true;
        }

		output = "Location enabled successfully";
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop ();
		yield return true;
	}

	/// <summary>
	/// Gets the location.
	/// </summary>
	/// <returns>(Lat, Lon)</returns>
	public Vector2 GetLocation() {
		if (locationEnabled) {
			Vector2 v = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
			output  = "\n   Me latLon  = " + v.x + ", " + v.y;

			return v;
		} else {
			return new Vector2(0, 0);
		}
	}

	// Update is called once per frame
//	void Update () {
//		timeAccu += Time.deltaTime;
//		if (timeAccu >= gpsUpdateInterval) {
//			if (locationEnabled) {
//				// Access granted and location value could be retrieved
//            	output = "Location: " + Input.location.lastData.latitude + ",   " +
//                   Input.location.lastData.longitude + "    -    " +
//                   Input.location.lastData.altitude + ",  " +
//                   Input.location.lastData.horizontalAccuracy + "    -    " +
//                   Input.location.lastData.timestamp;
//			} else {
//				output += "\nno data";
//			}
//			
//			timeAccu = 0;
//		}
//	}
}
