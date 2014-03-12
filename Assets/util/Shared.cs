﻿using UnityEngine;
using System.Collections;

public class Shared {
	public static string playerName = "Johannes";	// DEBUG. TODO: Adjust for other player deployment
	public static bool creator = false;
	public static int teamId = 0;
	public static string gameName = "Mainz";		// DEBUG. TODO: Set name via create-menu

	public static Vector2 mapLatLon;
	public static Vector2 mapSize = new Vector2(4096, 4096);
	public static int mapZoom = 17;

	private static LocationRequester locationRequester;
	public static LocationRequester LocationRequester { get { return locationRequester; } }

	private static bool initialized = false;

	public static void Initialize(MonoBehaviour behaviourScript) {
		if (!initialized) {
			initialized = true;

			locationRequester = new LocationRequester(behaviourScript);
		}
	}


	//####################################################################################################
	//###  Styles  #######################################################################################
	//####################################################################################################

	public static GUIStyle ButtonStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.button);
			style.fontSize = 24;
			return style;
		}
	}
	
	public static GUIStyle LabelStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.fontSize = 24;
			style.alignment = TextAnchor.MiddleCenter;
			return style;
		}
	}
}
