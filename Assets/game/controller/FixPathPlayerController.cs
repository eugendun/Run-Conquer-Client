using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixPathPlayerController : PlayerController {

	private static List<Vector2> startPoints = new List<Vector2> { new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f) };

//	private static List< List<Vector2> > pathPoints = new List< List<Vector2> > {
//		new List<Vector2> { new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(0.12f, 1.0f), new Vector2(0.12f, 0.0f), new Vector2(0.3f, 0.0f), new Vector2(0.3f, 1.0f) },
//		new List<Vector2> { new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(0.12f, 1.0f), new Vector2(0.12f, 0.0f), new Vector2(0.3f, 0.0f), new Vector2(0.3f, 1.0f) },
//		new List<Vector2> { new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1.2f) },
//		new List<Vector2> { new Vector2(0.5f, 0.5f) }
//	};

	private static List<float> speedList = new List<float> { 0.01f, 0.013f, 0.005f, 0.015f };
	private float speed;
	private GameObject targetMapTile;
	private static int scene = 0;
	private List<float> sceneTimes = new List<float> { 10, 10, 10, 10 };
	private static float passedSceneTime = 0;

	private List<int> lastTiles;

	Material mtl0;
	Material mtl1;
	Material mtl2;

	// Use this for initialization
	void Start () {
		base.Start();
		
		// set initial position dependent on team
		int pathId = Player.Team.Id;
		Vector2 position = startPoints[pathId];
		transform.position = new Vector3(position.x * map.getMapSize().x, 0.0f, position.y * map.getMapSize().y);
		speed = speedList[pathId];

		int N = map.MapTiles.Count;
		lastTiles = new List<int> { N/2 - 10, N/2 + 9 };
		
		mtl0 = Resources.Load<Material> ("materials/teamRed");
		mtl1 = Resources.Load<Material> ("materials/teamBlue");
		mtl2 = Resources.Load<Material> ("materials/teamGreen");
	}

	void ColorMap(int M) {

		int N = map.MapTiles.Count;
		for (int i = 0; i < M; i++) {
			MapTileController mapTileController = map.MapTiles[i].GetComponent<MapTileController>();
			mapTileController.setMaterial(mtl0);
		}
		for (int i = N - M; i < N; i++) {
			MapTileController mapTileController = map.MapTiles[i].GetComponent<MapTileController>();
			mapTileController.setMaterial(mtl1);
		}
		for (int i = 0; i < 2; i++) {
			int id = lastTiles[i];
			MapTileController mapTileController = map.MapTiles[id].GetComponent<MapTileController>();
			mapTileController.setMaterial(mtl2);
			mapTileController.locked = false;
		}
	}

	void changeScene(int id) {
		/*
	if i == 1
		color tiles 0..M with team0
		color tiles N-M..N-1 with team1		with M << N/2
		place each player on any tile with right color
	if i == 2
		color tiles 0..M with team0
		color tiles N-M..N-1 with team1		with M = N/2 - 3
		place each player on any tile with right color
	if i == 3
		color tiles 0..M with team0
		color tiles N-M..N-1 with team1		with M = N/2 - 1
		place player 0 and 1 with same distance from tile N/2 on a right colored tile
		set tile N/2 as target for 0 and 1
		place player 2 on any tile with right color
		*/

		if (id == 1) {
			int M = map.MapTiles.Count/2 - 30;
			ColorMap(M);

		} else if (id == 2) {
			int M = map.MapTiles.Count/2 - 5;
			ColorMap(M);

		} else if (id == 3) {
			int M = map.MapTiles.Count/2;
			ColorMap(M);
		}
	}

	// change player position after scene change
	void changePlayerPosition() {
		if (Player.Team.Id == 0) {
			Vector3 p = map.MapTiles[Random.Range(3, 12)].transform.position;
			p.y = 0;
			transform.position = p;
		} else if (Player.Team.Id == 1) {
			int N = map.MapTiles.Count;
			Vector3 p = map.MapTiles[Random.Range(N - 12, N - 3)].transform.position;
			p.y = 0;
			transform.position = p;
		}

		if (scene == 3) {
			Vector3 middle = map.MapTiles[lastTiles[0]].transform.position;
			if (Player.Team.Id == 0) {
				transform.position = new Vector3(middle.x - 7, 0, middle.z + 3);
			} else if (Player.Team.Id == 1) {
				transform.position = new Vector3(middle.x + 7, 0, middle.z + 3);
			}
		}

		// player 3 has left the map
		if (scene == 1 && Player.Team.Id == 3) {
			transform.position = new Vector3(0, 0, -50);
		}

		// player 2 in the middle on last tiles
		if (scene > 1 && Player.Team.Id == 2) {
			for (int i = 0; i < 2; i++) {
				int id = lastTiles[i];
				MapTileController mapTileController = map.MapTiles[id].GetComponent<MapTileController>();
				mapTileController.setMaterial(renderer.material);
				mapTileController.locked = false;
			}
			Vector3 p = map.MapTiles[lastTiles[1]].transform.position;
			p.y = 0;
			transform.position = p;
		}
	}

	int lastScene = 0;
	int onceInLevel3 = 0;
	void Update () {

		// change scene
		if (Player.Team.Id == 0) {
			passedSceneTime += Time.deltaTime;
			if (passedSceneTime >= sceneTimes[scene]) {
				scene++;
				changeScene(scene);
				passedSceneTime = 0;
			}
		}

		if (scene != lastScene) {
			changePlayerPosition();
			lastScene = scene;
		}

		if (scene < 3) {
			// look for next free map tile
			if (targetMapTile == null || targetMapTile.GetComponent<MapTileController>().team == Player.Team) {
				
				// look either for the nearest or the farthest one
				float MAX = map.getMapSize().x * map.getMapSize().y;
				MAX *= MAX;
				float desiredDistance = 0;
				float bestDistance = MAX;
				if (Random.value < 0.9) {
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

			if (Player.Team.Id < 2 || scene < 1) {
				// walk towards it
				if (targetMapTile != null) {
					Vector3 direction = targetMapTile.transform.position - transform.position;

					if (Player.Team.Id == 3) {
						direction.x = 0;
						direction.z = 1;
					}

					direction.y = 0;
					direction *= speed / direction.magnitude;
					transform.position = new Vector3(transform.position.x + direction.x, 0.0f, transform.position.z + direction.z);
				}
			}
		} else if (Player.Team.Id < 2) {
			// final scene
			GameObject targetMapTile = map.MapTiles[lastTiles[0]];
			Vector3 direction = targetMapTile.transform.position - transform.position;
			direction.y = 0;
			direction *= speed / direction.magnitude;
			transform.position = new Vector3(transform.position.x + direction.x, 0.0f, transform.position.z + direction.z);
		}


		if (scene > 1 && onceInLevel3 < 10 && Player.Team.Id == 0) {
			if (scene == 3) {
				onceInLevel3++;
			}
			for (int i = 0; i < 2; i++) {
				int id = lastTiles[i];
				MapTileController mapTileController = map.MapTiles[id].GetComponent<MapTileController>();
				mapTileController.setMaterial(mtl2);
				mapTileController.locked = false;
			}
		}
	}
}