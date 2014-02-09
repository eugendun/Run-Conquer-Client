using UnityEngine;
using System.Collections;
using System.Text;
using AssemblyCSharp;
using SimpleJSON;

public class PlayerController : MonoBehaviour
{
    private static string serverIp = "192.168.178.25";
    private static string serverPort = "3010";
    private static string serverPrefixUrl = string.Format("http://{0}:{1}", serverIp, serverPort);
    private static float syncRate = 0.5f;

    private PlayerModel player;

    // Use this for initialization
    void Start()
    {
        player = new PlayerModel(1);
        Debug.Log(JsonSerializer.Serialize(player));
        var uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
        Debug.Log(uniqDeviceId);
//        GetPlayerPosition();
//        PutPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform!=null){
            player.Position = new Vector2{
                x = transform.position.x,
                y = transform.position.z
            };
        }
    }

    // $.ajax({url:"http://localhost:3010/api/player/1", type:'PUT', contentType:'application/json; charset=utf-8', data:'{"Id":1,"Location":{"Geography":"WellKnownText":"POINT (13 13)"}}'})
    // $.getJSON("http://localhost:3010/api/player/1", function(data){console.log(data);})   

    public void GetPlayerPosition()
    {
        string getUrl = string.Format("{0}/api/Player/Getplayer/{1}", serverPrefixUrl, player.Id);
        WWW request = new WWW(getUrl);
        while (!request.isDone)
        {
            // do nothing if request is not completed
        }
        byte[] response = request.bytes;
        string content = Encoding.ASCII.GetString(response);
        var json = JSON.Parse(content);
        int id = json ["Id"].AsInt;
        var Position = json ["Position"];
        float x = Position ["x"].AsFloat;
        float y = Position ["y"].AsFloat;
        if (transform != null)
        {
            transform.position = new Vector3{
                x = x,
                y = 0f,
                z = y
            };
        }
        Invoke("GetPlayerPosition", syncRate);
    }

    public void PutPlayerPosition()
    {
        string putUrl = string.Format("{0}/api/Player/PutPlayer/{1}", serverPrefixUrl, player.Id);
        byte[] postData = Encoding.ASCII.GetBytes(player.ToJsonString());
        Hashtable head = new Hashtable();
        head.Add("Type", "PUT");
        head.Add("Content-Type", "application/binary; charset=utf-8");
        WWW request = new WWW(putUrl, postData, head);
        while(!request.isDone){}
        Invoke("PutPlayerPosition", syncRate);
    }
}
