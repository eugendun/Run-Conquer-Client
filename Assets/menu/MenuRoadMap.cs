using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuRoadMap : MonoBehaviour {

	private List<Vector3> locations = new List<Vector3> { new Vector3(-60, 5, 20), new Vector3(-28, 5, 25), new Vector3(8, 5, 13), new Vector3(65, 5, -10) };
	private int positionIndex = 0;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		transform.position = locations[positionIndex];
		target = transform.position;
		positionIndex++;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			if (positionIndex >= locations.Count) {
				Application.LoadLevel("game");
			} else {
				target = locations[positionIndex];
				positionIndex++;
			}
		}
		
		Vector3 direction = target - transform.position;
		transform.position = transform.position + (direction * 0.02f);
	}
}
