using System.Collections;
using System.Text;
using AssemblyCSharp;
using SimpleJSON;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class PlayerController : MonoBehaviour
{
    //private const string ServerIp = "192.168.178.25";
    private const string ServerIp = "localhost";
    private const string ServerPort = "3010";
    private const float SyncRate = 0.5f;
    private static readonly string ServerPrefixUrl = string.Format("http://{0}:{1}", ServerIp, ServerPort);

	public string output = "";

    private readonly Hashtable _headers = new Hashtable
    {
        {"Type", "PUT"},
        {"Content-Type", "application/json; charset=utf-8"}
    };

    private PlayerModel _player;
	public PlayerModel Player { get { return _player; } }

    // Use this for initialization
    public void Start()
    {
        string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
        _player = new PlayerModel(uniqDeviceId.GetHashCode());

		CreatePlayer();

		// choose player icon by player ID
		Texture2D tex = null;
		if (_player.Id == -1440738664 || _player.Id == -18546110) {
			tex = Resources.Load<Texture2D>("textures/icon_johannes");
		} else if (_player.Id == 1234 /* TODO Eugen's ID */) {
			tex = Resources.Load<Texture2D>("textures/icon_eugen");
		} else if (_player.Id == 5678 /* TODO Duong's ID */) {
			tex = Resources.Load<Texture2D>("textures/icon_duong");
		} else {
			tex = Resources.Load<Texture2D>("textures/icon_default");
		}
		renderer.material.mainTexture = tex;

		// set texture coords
		TextureUnwrapper.unwrapUV(gameObject, new Vector2(1, 1), new Vector2(0.5f, 0.5f));

        //GetPosition();
        //PostPosition();
    }

    private void CreatePlayer()
    {
        string url = ServerPrefixUrl + "/api/Player/PostPlayer";
        byte[] postData = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(_player));
        var webClient = new WWW(url, postData, _headers);
        while(!webClient.isDone) {
            // do nothing
        }
        if(!string.IsNullOrEmpty(webClient.error)) {
            Debug.Log(webClient.error);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(transform != null) {
            _player.Position = new Vector2 {
                x = transform.position.x,
                y = transform.position.z
            };
        }
    }

    public void GetPosition()
    {
        string getUrl = string.Format("{0}/api/Player/Getplayer/{1}", ServerPrefixUrl, _player.Id);
        var request = new WWW(getUrl);
        while(!request.isDone) {
            // do nothing if request is not completed
        }
        byte[] response = request.bytes;
        string content = Encoding.ASCII.GetString(response);
        JSONNode json = JSON.Parse(content);
        int id = json["Id"].AsInt;
        JSONNode position = json["Position"];
        float x = position["x"].AsFloat;
        float y = position["y"].AsFloat;
        if(transform != null) {
            transform.position = new Vector3 {
                x = x,
                y = 0f,
                z = y
            };
        }
        Invoke("GetPosition", SyncRate);
    }

    public void PostPosition()
    {
        string url = string.Format("{0}/api/Player/PutPlayer/{1}", ServerPrefixUrl, _player.Id);
        byte[] postData = Encoding.ASCII.GetBytes(_player.ToJsonString());
        var request = new WWW(url, postData, _headers);
        while(!request.isDone) {
        }
        Invoke("PostPosition", SyncRate);
    }

	public void OnGUI() {
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.fontSize = 16;
		GUI.Label(new Rect(10, 100, 1000, 900), output, style);
		GUI.Label(new Rect(10, 300, 700, 200), "device id: ." + _player.Id + ".", style);
	}
}