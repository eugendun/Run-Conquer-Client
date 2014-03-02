using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameController : MonoBehaviour {

	public MapCreator MapCreator;

	private GameInstanceModel GameModel;
	private List<PlayerController> Opponents;
	private MeController Me;

	// Use this for initialization
	void Start () {
		// TODO
		// create map with coordinates from game model
		// read player (opponents and me) models from game model and create controllers

		// DEBUG
		// create me game object

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}