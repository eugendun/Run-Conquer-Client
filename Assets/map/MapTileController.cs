using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class MapTileController : MonoBehaviour, MapListener {


	private bool flipping = false;
	private Quaternion targetRotation;
	private float flipStartTime;
	private float flipDuration = 0.5f;

	public TeamModel team;
	public PlayerController owner;
	public List<PlayerController> occupants = new List<PlayerController>();

	private Texture texture;

	// Use this for initialization
	void Start () {
	}

	public void mapDidLoad(Texture2D texture) {
		renderer.material.mainTexture = texture;
		renderer.material.color = Color.white;
		this.texture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		// evaluate owner
		if (occupants.Count > 0 && !occupants.Contains(owner)) {
			owner = occupants[0];
		}

		if (owner != null && team != owner.Player.Team) {
			team = owner.Player.Team;
            //renderer.material = new Material(owner.TeamObject.renderer.material);
			renderer.material.mainTexture = texture;
			Flip();
		}
	}

	public void Flip() {
		if (!flipping) {
			flipping = true;
			Transform transform = GetComponent<Transform>();

			// flip to backside to have right uv coordinates when flipped back
			transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);

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
