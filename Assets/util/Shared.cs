using UnityEngine;
using System.Collections;

public class Shared {
	public static string playerName = "Johannes";	// DEBUG. TODO: Adjust for other player deployment
	public static bool creator = false;
	public static int teamId = 1;
	public static string gameName = "Mainz";		// DEBUG. TODO: Set name via create-menu

	public static Vector2 mapLatLon;
	public static Vector2 mapSize = new Vector2(4096, 4096);
	public static int mapZoom = 18;

	public static float playTime = 13;		// in seconds

	public static string[] teamNames = new string[] { "Rette", "Bloo", "Griene", "Yello" };

	public static int winningTeamId;

	private static LocationRequester locationRequester;
	public static LocationRequester LocationRequester { get { return locationRequester; } }

	public static Texture2D iconTexture;

	private static bool initialized = false;

	public static void Initialize(MonoBehaviour behaviourScript) {
		if (!initialized) {
			initialized = true;

			iconTexture = Resources.Load<Texture2D>("textures/iTunesArtwork");
			locationRequester = new LocationRequester(behaviourScript);
		}
	}


	//####################################################################################################
	//###  Styles  #######################################################################################
	//####################################################################################################

	public static GUIStyle ButtonStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.button);
			style.fontSize = 48;
			return style;
		}
	}
	
	public static GUIStyle LabelStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.fontSize = 48;
			style.alignment = TextAnchor.MiddleCenter;
			return style;
		}
	}
	
	public static GUIStyle ListLabelStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.fontSize = 48;
			style.alignment = TextAnchor.MiddleLeft;
			return style;
		}
	}
	
	public static GUIStyle TitleStyle {
		get {
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.fontSize = 64;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			return style;
		}
	}
}
