using UnityEngine;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {

	public Transform spawnThis;
	public Vector2 center;
	public int zoom;
	public Vector2 size;

	public int x = 5;
	public int y = 5;

	public const float TILE_RADIUS = 1.5f;
	public const float TILE_MARGIN = 0.05f;
	//public bool useAsInnerCircleRadius = true;
	
	private float offsetX, offsetY;
	private Vector2 mapSize;

	private string output = "";
	private bool initialized = false;

	private List<GameObject> mapTiles;
	public List<GameObject> MapTiles {
		get { return mapTiles; }
	}

	void Start() {
		//float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		float unitLength = TILE_RADIUS + TILE_MARGIN;
		
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;

		mapSize = new Vector2((x + 0.5f) * offsetX, y * offsetY);
		mapTiles = new List<GameObject>();
	}
	
	
	void GenerateTextureCoordinates(Transform mapTile) {
		Mesh smesh = spawnThis.GetComponent<MeshFilter>().sharedMesh;
		Mesh mesh = spawnThis.GetComponent<MeshFilter>().mesh;

		Mesh tileSmesh = mapTile.GetComponent<MeshFilter>().sharedMesh;
		Mesh tileMesh = mapTile.GetComponent<MeshFilter>().mesh;

		Vector2[] uv = new Vector2[mesh.vertexCount];
		float f = 0.01f;
		//mesh.vertices[2] = new Vector3(0.01f, 0.02f, 0.3f);
		for (int i = 0; i < mesh.vertexCount; i++) {
			output += mesh.vertices[i].ToString() + "\n";
			output += f + "\n";
			Vector3 vertexPos = mapTile.localToWorldMatrix * new Vector4(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z, 1.0f);
			uv[i] = new Vector2(vertexPos.x / mapSize.x, vertexPos.z / mapSize.y);
			//uv[i] = Random.insideUnitCircle;
		}

		tileMesh.uv = uv;
	}

	void Update() {
		if (!initialized) {
			for( int i = 0; i < x; i++ ) {
				for( int j = 0; j < y; j++ ) {
					Vector2 hexpos = HexOffset( i, j );
					Vector3 pos = new Vector3(hexpos.x, 0, hexpos.y);
					Transform mapTile = (Transform)Instantiate(spawnThis, pos, Quaternion.AngleAxis(-90, Vector3.right));
					mapTile.localScale = new Vector3(TILE_RADIUS, TILE_RADIUS, TILE_RADIUS);
					mapTile.gameObject.AddComponent<MapRequester>();
					mapTile.gameObject.GetComponent<MapRequester>().center = center;
					mapTile.gameObject.GetComponent<MapRequester>().zoom = zoom;
					mapTile.gameObject.GetComponent<MapRequester>().size = size;
					mapTile.gameObject.AddComponent<MapTileController>();
					GenerateTextureCoordinates(mapTile);

					// add to list
					mapTiles.Add(mapTile.gameObject);
				}
			}

			initialized = true;
		}
	}
	
	Vector2 HexOffset( int x, int y ) {
		Vector2 position = Vector2.zero;
		
		if( y % 2 == 0 ) {
			position.x = x * offsetX;
			position.y = y * offsetY;
		} else {
			position.x = ( x + 0.5f ) * offsetX;
			position.y = y * offsetY;
		}

		position.x += 0.5f * offsetX;
		position.y += 0.5f * offsetY;
		
		return position;
	}
	/*
	Mesh HexMesh() {
		GameObject gameObject = new GameObject("Hex");
		gameObject.AddComponent("MeshFilter");
		gameObject.AddComponent("MeshRenderer");

		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = [Vector3(0,0,0), Vector3(0,1,0), Vector3(1, 1, 0)];
		mesh.uv = [Vector2 (0, 0), Vector2 (0, 1), Vector2 (1, 1)];
		mesh.triangles = [0, 1, 2];
	}*/

	public Vector2 getMapSize() {
		return mapSize;
	}
	
	// Update is called once per frame
	void OnGUI () {
//		Rect rect = new Rect(0, 400, 800, 800);
//		GUI.Label(rect, output);
	}
}
