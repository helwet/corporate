using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CameraDistance {
	focussed, normal, far
}
public class CameraController : MonoBehaviour {
	public GridLayout gridLayout;
	public float moveSpeed;
	public float zoomSpeed;
	public float minZoomDist;
	public float maxZoomDist;
	public CameraDistance cameraDistance = CameraDistance.normal;

	public static CameraController main;
	private Camera cam;

	void Start() {
		main = this;
	}
	void Update() {
		Move();
		Zoom();
	}
	void Move() {
		float xInput = Input.GetAxis("Horizontal");
		float zInput = Input.GetAxis("Vertical");

		Vector3 dir = transform.forward * zInput + transform.right * xInput;
		transform.position += dir * moveSpeed * Time.deltaTime;
	}
	void Zoom() {
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
		float dist = Vector3.Distance(transform.position, cam.transform.position);
		if (dist < minZoomDist && scrollInput > 0.0f)
			return;
		else if (dist > maxZoomDist && scrollInput < 0.0f)
			return;
		cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
	}
	private Ray ray;

	void startSelecting(){

	}
	public Vector3Int lastClickedTile;
	List<GameObject> selected;
	List<string> searchedTags;
	string searchedTag = "";
	void click() {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pz.z = 0;

		// convert mouse click's position to Grid position
		//GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
		lastClickedTile = gridLayout.WorldToCell(pz);



		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		int i = 0;
		while (i < hits.Length) {
			RaycastHit hit = hits[i];
			Debug.Log(hit.collider.gameObject.name);
			JObject j = JObject.Parse(hit.collider.gameObject.tag);
			if (
				hit.collider.gameObject.tag == "selectable"
			
			){			
				Selectable selectable = hit.collider.gameObject.GetComponent<Selectable>();
				if(){

				}
			}
			i++;
		}
	}

		public void FocusOnPosition(Vector3 pos) {
		transform.position = pos;
	}
}