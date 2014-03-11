using UnityEngine;
using System.Collections;

public class GoogleMapsTransformation {

	/// <summary>
	///  
	/// </summary>
	/// <returns>(lat, lon of left top corner, width between lat, width between lon)</returns>
	public static Rect getCorners(Vector2 center, int zoom, int mapWidth, int mapHeight) {
		MercatorProjection projection = new MercatorProjection();
		float scale = Mathf.Pow(2, zoom);
		Vector2 centerPx = projection.fromLatLngToPoint(center);
		Vector2 swPoint = new Vector2(centerPx.x - (mapWidth/2) / scale, centerPx.y + (mapHeight/2) / scale);
		Vector2 swLatLon = projection.fromPointToLatLng(swPoint);
//		EditorUtility.DisplayDialog ("SW", swLatLon.x + "   " + swLatLon.y, "Ok");
		Vector2 nePoint = new Vector2(centerPx.x + (mapWidth/2) / scale, centerPx.y - (mapHeight/2) / scale);
		Vector2 neLatLon = projection.fromPointToLatLng(nePoint);
//		EditorUtility.DisplayDialog ("NE", neLatLon.x + "   " + neLatLon.y, "Ok");

		// shrink rect a little bit, because calculation seems to be not exact
		Rect rect = new Rect(swLatLon.x, swLatLon.y, neLatLon.x - swLatLon.x, neLatLon.y - swLatLon.y);
		rect.width  *= 0.6f;
		rect.height *= 0.6f;
		rect.center = center;
		return rect;
	}
}
