using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Shared {
    // server connection
    private const string ServerIp = "h2231364.stratoserver.net";
    private const string ServerPort = "9013";
    //private const string ServerIp = "localhost";
    //private const string ServerPort = "3010";
    private static readonly string ServerPrefixUrl = string.Format("http://{0}:{1}/api", ServerIp, ServerPort);
    public static Hashtable _headers = new Hashtable
	{
	    {"Type", "PUT"},
	    {"Content-Type", "application/json; charset=utf-8"}
	};

    public static GameInstanceModel gameInstance = null;
    public static PlayerModel player = null;

	public static string playerName = "Johannes";	// DEBUG. TODO: Adjust for other player deployment
	public static bool creator = false;
	public static int teamId = 0;
	public static string gameName = "Mainz";		// DEBUG. TODO: Set name via create-menu

	public static Vector2 mapLatLon;
	public static Vector2 mapSize = new Vector2(4096, 4096);
	public static int mapZoom = 18;

	public static float playTime = 10;		// in seconds

	public static string[] teamNames = new string[] { "Rette", "Bloo", "Griene", "Yello" };

	private static LocationRequester locationRequester;
	public static LocationRequester LocationRequester { get { return locationRequester; } }

	private static bool initialized = false;

	public static void Initialize(MonoBehaviour behaviourScript) {
		if (!initialized) {
			initialized = true;

			locationRequester = new LocationRequester(behaviourScript);
		}
	}

    public static string GetApiCallUrl(string apiCall)
    {
        return string.Format("{0}/{1}", ServerPrefixUrl, apiCall);
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
