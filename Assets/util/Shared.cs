using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public enum TeamColor
{
    Rette, Bloo, Griene, Yello
}

public class Shared {
    public static bool InDebug = false;

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
    public static TeamColor playerTeamColor;

    public static DateTime StartDate = DateTime.Now;
    public static DateTime EndDate = StartDate.AddHours(1.0);

	public static bool creator = false;

	public static Vector2 mapLatLon;
	public static Vector2 mapSize = new Vector2(4096, 4096);
	public static int mapZoom = 18;

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
