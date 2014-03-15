using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed = 0.05f;
	private Vector2 touchStartPosition;
	private Vector2 lastDeltaPosition;
	private float remainingTimeToFocus = 0;

	public Transform player;

	void Update() {

		remainingTimeToFocus -= Time.deltaTime;

		// focus player
		if (remainingTimeToFocus < 0 && player != null) {
			transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
		}

		// panning
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			lastDeltaPosition = new Vector2(0, 0);
			remainingTimeToFocus = float.MaxValue;
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition - lastDeltaPosition;
			lastDeltaPosition = touchDeltaPosition;
			transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			remainingTimeToFocus = 5;
		}
	}

//	private bool viewScreenPanning = false;
//	private float viewScreenOldMouseX;
//	private float viewScreenOldMouseY;
//	private Vector3 viewScreenPanVector;
//	private float viewScreenPanSpeed = 0.1f;
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		// ***** PANNING *****
//		// Hold Right Mouse Button to pan the screen in a direction
//		if(Input.GetButtonDown("MouseRight")) {
//			print ("Panning...");
//			viewScreenPanning = true;
//			viewScreenOldMouseX = Input.mousePosition.x;
//			viewScreenOldMouseY = Input.mousePosition.y;
//		}
//		
//		// If panning, find the angle to pan based on camera angle not screen
//		if(viewScreenPanning==true) {
//			if(Input.GetButtonUp("MouseRight")) {
//				viewScreenPanning = false;
//			}
//			viewScreenPanVector = transform.TransformPoint(new Vector3((-viewScreenOldMouseX + Input.mousePosition.x) / viewScreenPanSpeed, 0.0f, -(viewScreenOldMouseY - Input.mousePosition.y) / viewScreenPanSpeed));
//			// since we use a quick and dirty transform, reset the camera height to what it was
//			viewScreenPanVector.y = transform.position.y;
//			transform.position = viewScreenPanVector;
//		}
//	}
}
