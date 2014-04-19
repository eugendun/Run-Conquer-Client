using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Text;

public class MeController : PlayerController
{
    // Update is called once per frame
    public new void Update()
    {
        if (Shared.LocationRequester == null)
        {
            return;
        }
        Vector2 location = Shared.LocationRequester.GetLocation();

        Rect latLonBounds = map.LatLonBounds;

        // calculate value between (0, 0) and (1, 1) in normalized map-space (lat -> y, lon -> x)
        Vector2 v = new Vector2();
        v.x = (location.y - (latLonBounds.center.y - latLonBounds.height / 2)) / latLonBounds.height;
        v.y = (location.x - (latLonBounds.center.x - latLonBounds.width / 2)) / latLonBounds.width;

        // transform into world-space
        Vector2 mapSize = map.getMapSize();
        v.x *= mapSize.x;
        v.y *= mapSize.y;

        if (!Shared.InDebug)
        {
            Player.Position.x = v.x;
            Player.Position.y = v.y; 
        }

        transform.position = new Vector3(Player.Position.x, 0.0f, Player.Position.y);

        base.Update();
    }
}
