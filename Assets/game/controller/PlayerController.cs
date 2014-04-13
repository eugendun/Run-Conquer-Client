using AssemblyCSharp;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class PlayerController : MonoBehaviour
{
    protected GameObject teamObject;

    public Map map;

    public GameObject TeamObject
    {
        get
        {
            return teamObject;
        }
        set
        {
            teamObject = value;

            // set team material
            if (teamObject != null && Player != null && Player.Team != null)
            {
                string materialName = "";
                Texture2D tex = null;
                switch (Player.Team.internalId)
                {
                    case 0:
                        materialName = "teamRed";
                        tex = Resources.Load<Texture2D>("textures/icon_0");
                        break;
                    case 1:
                        materialName = "teamBlue";
                        tex = Resources.Load<Texture2D>("textures/icon_1");
                        break;
                    case 2:
                        materialName = "teamGreen";
                        tex = Resources.Load<Texture2D>("textures/icon_2");
                        break;
                    default:
                        materialName = "teamSilver";
                        tex = Resources.Load<Texture2D>("textures/icon_3");
                        break;
                }

                teamObject.renderer.material = Resources.Load<Material>("materials/" + materialName);
                renderer.material.mainTexture = tex;
            }
        }
    }

    private PlayerModel _player;
    public PlayerModel Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
            //TeamObject = teamObject;
        }
    }

    // Use this for initialization
    public void Start()
    {
        //string uniqDeviceId = SystemInfo.deviceUniqueIdentifier;
        //_player = new PlayerModel(uniqDeviceId.GetHashCode());
        //Shared.gameInstance.Players.Add(_player);

        // set texture coords
        TextureUnwrapper.unwrapUV(gameObject, new Vector2(1, 1), new Vector2(0.5f, 0.5f));

        // call setter for team object (in case it has been called before Start()) to choose team color
        //TeamObject = teamObject;

        transform.localScale = transform.localScale * 1.3f;
        //		teamObject.tr localScale = Vector3.one;

        // FIXME
        //				StartCoroutine(SyncPosition());
    }

    // Update is called once per frame
    public void Update()
    {
        if (Player != null)
        {
            transform.position = new Vector3(Player.Position.x, 0.0f, Player.Position.y);
        }
    }
}