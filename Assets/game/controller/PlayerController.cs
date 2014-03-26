using System.Collections;
using System.Text;
using AssemblyCSharp;
using SimpleJSON;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class PlayerController : MonoBehaviour {
	//private const string ServerIp = "192.168.178.25";
	// http://h2231364.stratoserver.net:9013 remote server!!!
	//protected const string ServerIp = "localhost";
	//protected const string ServerPort = "3010";
	protected const string ServerIp = "h2231364.stratoserver.net";
	protected const string ServerPort = "9013";
	protected static readonly string ServerPrefixUrl = string.Format ("http://{0}:{1}", ServerIp, ServerPort);
        protected const float SyncRate = 0.3f;
	protected GameObject teamObject;
	
	public Map map;
	
	public GameObject TeamObject {
		get {
			return teamObject;
		}
		set {
			teamObject = value;
			
			// set team material
			if (teamObject != null && Player != null && Player.Team != null) {
				string materialName = "";
				Texture2D tex = null;
				switch (Player.Team.internalId) {
				case 0:
					materialName = "teamRed";
					tex = Resources.Load<Texture2D> ("textures/icon_0");
					break;
				case 1:
					materialName = "teamBlue";
					tex = Resources.Load<Texture2D> ("textures/icon_1");
					break;
				case 2:
					materialName = "teamGreen";
					tex = Resources.Load<Texture2D> ("textures/icon_2");
					break;
				default:
					materialName = "teamSilver";
					tex = Resources.Load<Texture2D> ("textures/icon_3");
					break;
				}

				teamObject.renderer.material = Resources.Load<Material> ("materials/" + materialName);
				renderer.material.mainTexture = tex;
			}
		}
	}

	private readonly Hashtable _headers = new Hashtable
    {
        {"Type", "PUT"},
        {"Content-Type", "application/json; charset=utf-8"}
    };

	private PlayerModel _player;
	public PlayerModel Player { get { return _player; } set { _player = value; TeamObject = teamObject; } }


	public string output = "";


	// Use this for initialization
	public void Start ()
	{
		string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
		_player = new PlayerModel (uniqDeviceId.GetHashCode ());
        Shared.gameInstance.Players.Add(_player);
		// set texture coords
		TextureUnwrapper.unwrapUV (gameObject, new Vector2 (1, 1), new Vector2 (0.5f, 0.5f));

		// call setter for team object (in case it has been called before Start()) to choose team color
		TeamObject = teamObject;

		transform.localScale = transform.localScale * 1.3f;
//		teamObject.tr localScale = Vector3.one;

		// FIXME
//				StartCoroutine(SyncPosition());
	}


	// Update is called once per frame
	public void Update ()
	{
			if (transform != null) {
					_player.Position = new Vector2 {
	                x = transform.position.x,
	                y = transform.position.z
	            };
			}
	}

	protected IEnumerator SyncPosition ()
	{
		while(true) {
        	PutPosition();
			yield return new WaitForSeconds(SyncRate);
		}
	}

	protected void PutPosition()
	{
	    string url = ServerPrefixUrl + "/api/Player/PutPlayer";
	    byte[] data = Encoding.ASCII.GetBytes(ToJson());
	    WWW webClient = new WWW(url, data, _headers);
	    //yield return webClient;
	    while (!webClient.isDone) {
                // wait until request is done
	    }
	}

    public string ToJson()
    {
        var jsonPlayer = new JSONClass();
        jsonPlayer["Id"].AsInt = _player.Id;

        var jsonPos = new JSONClass();
        jsonPos["x"].AsFloat = transform.position.x;
        jsonPos["y"].AsFloat = transform.position.z;
        jsonPlayer.Add("Position", jsonPos);
        return jsonPlayer.ToString();
    }

//	public void OnGUI ()
//	{
//			GUIStyle style = new GUIStyle (GUI.skin.label);
//			style.fontSize = 16;
//			GUI.Label (new Rect (10, 100, 1000, 900), output, style);
//			GUI.Label (new Rect (10, 300, 700, 200), "device id: ." + _player.Id + ".", style);
//	}
}