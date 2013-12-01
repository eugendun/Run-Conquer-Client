using UnityEngine;
using System.Collections;

public class LocationRequester : MonoBehaviour {
	
	string output;
	bool locationEnabled = false;
	float gpsUpdateInterval = 1.0f;
	float timeAccu = 0.0f;
	
	// Use this for initialization
	IEnumerator Start () {
		// First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
			output = "location not enabled!";
            return null;
		}
		
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            output = "Timed out";
            return null;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            output = "Unable to determine device location";
            return null;
        } else {
        	locationEnabled = true;
        }
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop ();
		return null;
	}
	
	// Update is called once per frame
	void OnGUI () {
		Rect rect = new Rect(0, 200, 800, 800);
		GUI.Label(rect, output);
	}
	
	// Update is called once per frame
	void Update () {
		timeAccu += Time.deltaTime;
		if (timeAccu >= gpsUpdateInterval) {
			if (locationEnabled) {
				// Access granted and location value could be retrieved
            	output = "Location: " + Input.location.lastData.latitude + " " +
                   Input.location.lastData.longitude + " " +
                   Input.location.lastData.altitude + " " +
                   Input.location.lastData.horizontalAccuracy + " " +
                   Input.location.lastData.timestamp;
			} else {
				output += "\nno data";
			}
			
			timeAccu = 0;
		}
	}
}
