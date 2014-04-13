using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KIPlayerController : PlayerController {

	private static List<Vector2> startPoints = new List<Vector2> { new Vector2(0.5f, 0.5f), new Vector2(0.4f, 0.5f), new Vector2(0.55f, 0.6f), new Vector2(0.45f, 0.45f) };
	private static List<float> speedList = new List<float> { 0.002f, 0.0028f, 0.0035f, 0.0026f };

	private GameObject targetMapTile;
	private float speed;

	// Use this for initialization
	public new void Start () {
		base.Start();

		// set initial position dependent on team
		Vector2 position = startPoints[Player.Team.Id];
		transform.position = new Vector3(position.x * map.getMapSize().x, 0.0f, position.y * map.getMapSize().y);
		speed = speedList[Player.Team.Id];
	}
	
	public new void Update () {

		// look for next free map tile
		if (targetMapTile == null || targetMapTile.GetComponent<MapTileController>().team == Player.Team) {

			// look either for the nearest or the farthest one
			float MAX = map.getMapSize().x * map.getMapSize().y;
			MAX *= MAX;
			float desiredDistance = 0;
			float bestDistance = MAX;
			if (Random.value < 0.15) {
				desiredDistance = MAX;
				bestDistance = 0;
			}

			if (map.MapTiles != null) {
				foreach (GameObject mapTile in map.MapTiles) {
					float distance = (mapTile.transform.position - transform.position).sqrMagnitude;
					float a = Mathf.Abs(desiredDistance - distance);
					float b = Mathf.Abs(desiredDistance - bestDistance);
					if (mapTile.GetComponent<MapTileController>().team != Player.Team && a < b) {
						targetMapTile = mapTile;
						bestDistance = distance;
					}
				}
			}
		}

		// walk towards it
		if (targetMapTile != null) {
			Vector3 direction = targetMapTile.transform.position - transform.position;
			direction.y = 0;
			direction *= speed / direction.magnitude;
			transform.position = new Vector3(transform.position.x + direction.x, 0.0f, transform.position.z + direction.z);
		}
	}
}
