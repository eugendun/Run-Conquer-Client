using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class MapTileController : MonoBehaviour, MapListener {


	private int counter = 0;
	private bool flipping = false;
	private Quaternion targetRotation;
	private float flipStartTime;
	private float flipDuration = 0.5f;

	public TeamModel team;
	public PlayerController owner;
	public List<PlayerController> occupants = new List<PlayerController>();

	// Use this for initialization
	void Start () {
	}

	public void mapDidLoad(Texture2D texture) {
		renderer.material.mainTexture = texture;
		renderer.material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		// evaluate owner
		if (occupants.Count > 0 && !occupants.Contains(owner)) {
			owner = occupants[0];
		}

		if (owner != null && team != owner.Player.Team) {
			team = owner.Player.Team;
			renderer.material = owner.TeamObject.renderer.material;
			Flip();
		}
	}

	public void Flip() {
		if (!flipping) {
			flipping = true;
			Transform transform = GetComponent<Transform>();
			targetRotation = transform.rotation * Quaternion.AngleAxis(180.0f, Vector3.right);
			flipStartTime = Time.realtimeSinceStartup;
			InvokeRepeating("RotateSmooth", 0, 0.04f);
		}
	}

	private void RotateSmooth() {
		Transform transform = GetComponent<Transform>();
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (Time.realtimeSinceStartup - flipStartTime) / flipDuration);
		if (Quaternion.Angle(targetRotation, transform.rotation) < 0.05) {
			flipping = false;
			CancelInvoke();
		}
	}
}
