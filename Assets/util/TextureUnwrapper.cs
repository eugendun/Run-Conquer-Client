using UnityEngine;
using System.Collections;

public class TextureUnwrapper {

	public static void unwrapUV(GameObject gameObject, Vector2 scale, Vector2 offset) {
		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

		Vector2[] uv = new Vector2[mesh.vertexCount];
		for (int i = 0; i < mesh.vertexCount; i++) {
			Vector3 vertexPos = gameObject.transform.localToWorldMatrix * new Vector4(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z, 1.0f);
			uv[i] = new Vector2(vertexPos.x * scale.x + offset.x, vertexPos.z * scale.y + offset.y);
		}
		
		mesh.uv = uv;
	}
}
