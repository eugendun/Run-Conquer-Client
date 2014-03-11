using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MapTileController : MonoBehaviour, MapListener {


	private int counter = 0;
	private bool flipped = false;
	private Quaternion targetRotation;
	private float flipStartTime;
	private float flipDuration = 0.5f;

	public TeamModel team;

	// Use this for initialization
	void Start () {
	}

	public void mapDidLoad(Texture2D texture) {
		renderer.material.mainTexture = texture;
	}
	
	// Update is called once per frame
//	void Update () {
//		if (counter > 200) {
//			if (Random.Range(0.0f, 1.0f) > 0.95f || flipped) {
//				Flip();
//				flipped = !flipped;
//			}
//			counter = 0;
//		}
//
//		counter++;
//	}

	public void Flip() {
		Transform transform = GetComponent<Transform>();
		targetRotation = transform.rotation * Quaternion.AngleAxis(180.0f, Vector3.right);
		flipStartTime = Time.realtimeSinceStartup;
		InvokeRepeating("RotateSmooth", 0, 0.04f);
	}

	private void RotateSmooth() {
		Transform transform = GetComponent<Transform>();
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (Time.realtimeSinceStartup - flipStartTime) / flipDuration);
		if (Quaternion.Angle(targetRotation, transform.rotation) < 0.05) {
			CancelInvoke();
		}
	}
}
