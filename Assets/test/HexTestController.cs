using UnityEngine;
using System.Collections;

public class HexTestController : MonoBehaviour, MapListener {

	private Map map;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<Map>();
		map = gameObject.GetComponent<Map>();
		map.tilesX = 20;
		map.tilesY = 20;

		map.listener = this;
		map.Create(Shared.mapLatLon, Shared.mapZoom, Shared.mapSize);
	}
	
	public void mapDidLoad(Texture2D texture) {
		foreach (GameObject mapTile in map.MapTiles) {
			Material templateMaterial = Resources.Load<Material> ("materials/teamSilver");
			Material material = new Material(templateMaterial);
			float color = 1;
			material.color = new Color(color, color, color);
			mapTile.renderer.material = material;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
