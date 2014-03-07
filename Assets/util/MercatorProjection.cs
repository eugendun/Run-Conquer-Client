using UnityEngine;
using System.Collections;

public class MercatorProjection {

	private const int MERCATOR_RANGE = 256;

	private Vector2 pixelOrigin;
	private float pixelsPerLonDegree;
	private float pixelsPerLonRadian;

	private float bound(float value, float opt_min, float opt_max) {
		value = Mathf.Max(value, opt_min);
		value = Mathf.Min(value, opt_max);
		return value;
	}
	
	private float degreesToRadians(float deg) {
		return deg * (Mathf.PI / 180);
	}
	
	private float radiansToDegrees(float rad) {
		return rad / (Mathf.PI / 180);
	}
	
	
	
	public MercatorProjection() {
		this.pixelOrigin = new Vector2( MERCATOR_RANGE / 2, MERCATOR_RANGE / 2);
		this.pixelsPerLonDegree = MERCATOR_RANGE / 360.0f;
		this.pixelsPerLonRadian = MERCATOR_RANGE / (2 * Mathf.PI);
	}
	
	public Vector2 fromLatLngToPoint(Vector2 latLng) {
		Vector2 point = Vector2.zero;
		
		Vector2 origin = this.pixelOrigin;
		point.x = origin.x + latLng.y * this.pixelsPerLonDegree;
		// NOTE(appleton): Truncating to 0.9999 effectively limits latitude to
		// 89.189.  This is about a third of a tile past the edge of the world tile.
		float siny = bound(Mathf.Sin(degreesToRadians(latLng.x)), -0.9999f, 0.9999f);
		point.y = origin.y + 0.5f * Mathf.Log((1 + siny) / (1 - siny)) * -this.pixelsPerLonRadian;
		return point;
	}
	
	public Vector2 fromPointToLatLng(Vector2 point) {
		Vector2 origin = this.pixelOrigin;
		float lng = (point.x - origin.x) / this.pixelsPerLonDegree;
		float latRadians = (point.y - origin.y) / -this.pixelsPerLonRadian;
		float lat = radiansToDegrees(2 * Mathf.Atan(Mathf.Exp(latRadians)) - Mathf.PI / 2);
		return new Vector2(lat, lng);
	}
	
	//pixelCoordinate = worldCoordinate * Math.pow(2,zoomLevel)
}
