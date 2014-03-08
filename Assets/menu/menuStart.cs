using UnityEngine;
using System.Collections;

public class menuStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (GUI.Button(new Rect(0, 0, 1080, 800), "Create Game")) {
			Application.LoadLevel("game");
		}
	}
}
