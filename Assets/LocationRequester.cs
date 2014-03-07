using UnityEngine;
using System.Collections;

public class LocationRequester {

	public string output;
	public Rect latLongBounds;

	private bool locationEnabled = false;
//	private float GPSUpdateInterval = 1.0f;

	public LocationRequester(MonoBehaviour caller) {
		caller.StartCoroutine( Start() );
	}

	// Use this for initialization
	private IEnumerator Start() {
		// First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
			output = "location not enabled!";
            return false;
		}
		
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return (new WaitForSeconds(1));
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
			output = "Timed out";
            return false;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
			output = "Unable to determine device location";
            return false;
        } else {
			locationEnabled = true;
        }

		output = "Location enabled successfully";
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop ();
		return true;
	}

	/// <summary>
	/// Gets the location.
	/// </summary>
	/// <returns>Value between (0, 0) and (1, 1) in normalized map-space</returns>
	public Vector3 GetLocation() {
		if (locationEnabled) {
			Vector3 v = new Vector3(Input.location.lastData.longitude, 0.0f, Input.location.lastData.latitude);
			output  = "\n   Me latLon  = " + v.x + ", " + v.z;
			output += "\n   Map center = " + latLongBounds.center.x + ", " + latLongBounds.center.y;
			v.x = (v.x - (latLongBounds.center.y - latLongBounds.height/2)) / latLongBounds.height;
			v.z = (v.z - (latLongBounds.center.x - latLongBounds.width/2))  / latLongBounds.width;
			return v;
		} else {
			return new Vector3(0, 0, 0);
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
