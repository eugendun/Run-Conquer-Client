using UnityEngine;
using System.Collections;

public class MenuWaitingRoom : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO if not creator, wait for event to start game (maybe just a state inside shared e.g.)
        // FIXME
        //		if (Shared.gameInstance.gameStarted) {
        //			Application.LoadLevel("game");
        //		}
    }

    void Next()
    {
        Application.LoadLevel("game");
    }

    void OnGUI()
    {

        // title
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Waiting Room", Shared.TitleStyle);

        GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.1f), "Player", Shared.ListLabelStyle);
        GUI.Label(new Rect(Screen.width * 0.6f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.1f), "Team", Shared.ListLabelStyle);

        GUI.Label(new Rect(0, Screen.height * 0.20f, Screen.width, Screen.height * 0.1f), "_________________________________________________________", Shared.LabelStyle);

        // players
        // TODO list each player from gameInstance

        // next
        if (Shared.creator) {
            if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.8f, Screen.width * 0.6f, Screen.height * 0.1f), "Next", Shared.ButtonStyle)) {
                Next();
            }
        }
    }
}
