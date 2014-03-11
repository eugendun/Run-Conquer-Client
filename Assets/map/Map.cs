using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public Transform spawnThis;

	public int tilesX = 5;
	public int tilesY = 5;

	public const float TILE_RADIUS = 1.5f;
	public const float TILE_MARGIN = 0.05f;
	//public bool useAsInnerCircleRadius = true;
	
	private float offsetX, offsetY;
	private Vector2 mapSize;

	private string output = "";
	private bool initialized = false;

	private List<GameObject> mapTiles;
	public List<GameObject> MapTiles { get { return mapTiles; } }
	
	private Rect latLonBounds;
	public Rect LatLonBounds { get { return latLonBounds; } }

	private bool started = false;
	private bool waitingCreate = false;

	// Google Map parameter
	private Vector2 center;
	private int zoom;
	private Vector2 size;

	protected void Start() {
		started = true;

		//float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		float unitLength = TILE_RADIUS + TILE_MARGIN;
		
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;

		mapSize = new Vector2((tilesX + 0.5f) * offsetX, tilesY * offsetY);
		mapTiles = new List<GameObject>();

		// perform a late create
		if (waitingCreate) {
			Create(center, zoom, size);
		}
	}


	public void Create(Vector2 center, int zoom, Vector2 size) {
		this.center = center;
		this.zoom = zoom;
		this.size = size;

		if (started) {
			
			// create requester for the map
			gameObject.AddComponent<MapRequester>();
			MapRequester mapRequester = gameObject.GetComponent<MapRequester>();

			for( int i = 0; i < tilesX; i++ ) {
				for( int j = 0; j < tilesY; j++ ) {
					Vector2 hexpos = HexOffset( i, j );
					Vector3 pos = new Vector3(hexpos.x, -1, hexpos.y);
					Transform mapTile = (Transform)Instantiate(spawnThis, pos, Quaternion.AngleAxis(-90, Vector3.right));
					mapTile.localScale = new Vector3(TILE_RADIUS, TILE_RADIUS, TILE_RADIUS);

					// add map tile as listener to map requester to get notified, when map has been loaded
					mapTile.gameObject.AddComponent<MapTileController>();
					mapRequester.addListener(mapTile.gameObject.GetComponent<MapTileController>());

					// create uv coordinates all over the map
					TextureUnwrapper.unwrapUV(mapTile.gameObject, new Vector2(1 / mapSize.x, 1 / mapSize.y), new Vector2(0, 0));
					
					// add to list
					mapTiles.Add(mapTile.gameObject);
				}
			}

			// request map and calculate its bounds
			mapRequester.Request(center, zoom, size);
			latLonBounds = GoogleMapsTransformation.getCorners(center, zoom, (int)size.x, (int)size.y);
			output += "\nmap bounds = (" + latLonBounds.x + ", " + latLonBounds.y + ", " + latLonBounds.width + ", " + latLonBounds.height + ")";
		} else {
			waitingCreate = true;
		}
	}
	
	
//	protected void GenerateTextureCoordinates(Transform mapTile) {
//		Mesh smesh = spawnThis.GetComponent<MeshFilter>().sharedMesh;
//		Mesh mesh = spawnThis.GetComponent<MeshFilter>().mesh;
//
//		Mesh tileSmesh = mapTile.GetComponent<MeshFilter>().sharedMesh;
//		Mesh tileMesh = mapTile.GetComponent<MeshFilter>().mesh;
//
//		Vector2[] uv = new Vector2[mesh.vertexCount];
//		float f = 0.01f;
//		//mesh.vertices[2] = new Vector3(0.01f, 0.02f, 0.3f);
//		for (int i = 0; i < mesh.vertexCount; i++) {
//			Vector3 vertexPos = mapTile.localToWorldMatrix * new Vector4(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z, 1.0f);
//			uv[i] = new Vector2(vertexPos.x / mapSize.x, vertexPos.z / mapSize.y);
//			//uv[i] = Random.insideUnitCircle;
//		}
//
//		tileMesh.uv = uv;
//	}
	
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
//		Rect rect = new Rect(400, 500, 800, 800);
//		GUI.Label(rect, output);
	}
}
