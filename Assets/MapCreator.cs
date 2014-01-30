using UnityEngine;
using System.Collections;

public class MapCreator : MonoBehaviour {

	public Transform spawnThis;

	public int x = 5;
	public int y = 5;
	
	public float radius = 0.5f;
	public float margin = 0.1f;
	//public bool useAsInnerCircleRadius = true;
	
	private float offsetX, offsetY;

	private string output = "";


	void Start() {
		//float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		float unitLength = radius + margin;
		
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;
	}


	// Vertices sind immer dann befuellt, wenn man beim Mesh im GUI folgende Dinge umstellt: Normals: None -> apply -> Normals: Import -> apply
	void GenerateTextureCoordinates(Transform mapTile) {
		Mesh smesh = spawnThis.GetComponent<MeshFilter>().sharedMesh;
		Mesh mesh = spawnThis.GetComponent<MeshFilter>().mesh;

		Mesh tileSmesh = mapTile.GetComponent<MeshFilter>().sharedMesh;
		Mesh tileMesh = mapTile.GetComponent<MeshFilter>().mesh;

		Vector2 mapSize = new Vector2((x + 0.5f) * offsetX, y * offsetY);
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

	private int secondsSinceStart = 0;

	void Update() {
		if (secondsSinceStart == 10) {
			for( int i = 0; i < x; i++ ) {
				for( int j = 0; j < y; j++ ) {
					Vector2 hexpos = HexOffset( i, j );
					Vector3 pos = new Vector3(hexpos.x, 0, hexpos.y);
					Transform mapTile = (Transform)Instantiate(spawnThis, pos, Quaternion.AngleAxis(-90, Vector3.right));
					mapTile.localScale = new Vector3(radius, radius, radius);
					mapTile.gameObject.AddComponent<MapRequester>();
					mapTile.gameObject.AddComponent<MapTileController>();
					GenerateTextureCoordinates(mapTile);
				}
			}
		}
		secondsSinceStart++;
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
	
	// Update is called once per frame
	void OnGUI () {
		Rect rect = new Rect(0, 400, 800, 800);
		GUI.Label(rect, output);
	}
}
