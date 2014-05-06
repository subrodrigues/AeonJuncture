using UnityEngine;
using System.Collections;

public class CameraController3D : MonoBehaviour
{
	
		// How fast the camera moves
		float moveSpeed = 1.5f;
	
		// Use this for initialization
		void Start ()
		{
				// Set the initial position of the camera.
				// Right now we don't actually need to set up any other variables as
				// we will start with the initial position of the camera in the scene editor
				// If you want to create cameras dynamically this will be the place to
				// set the initial transform.positiom.x/y/z
		
				this.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				Vector3 originalPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
				Vector3 playerPos = new Vector3 (originalPos.x, originalPos.y + 6, originalPos.z - 3.5f);
				transform.position = Vector3.Lerp (transform.position, playerPos, Time.deltaTime * moveSpeed);
				transform.localEulerAngles = new Vector3 (60.0f, 0, 0);
		
				/*	// Left
		if ((Input.GetKey (KeyCode.LeftArrow))) {
			transform.Translate ((Vector3.left * cameraVelocity) * Time.deltaTime);
		}
		// Right
		if ((Input.GetKey (KeyCode.RightArrow))) {
			transform.Translate ((Vector3.right * cameraVelocity) * Time.deltaTime);
		}
		// Up
		if ((Input.GetKey (KeyCode.UpArrow))) {
			transform.Translate ((Vector3.up * cameraVelocity) * Time.deltaTime);
		}
		// Down
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate ((Vector3.down * cameraVelocity) * Time.deltaTime);
		}
	*/
		}
	
		void OnGUI ()
		{
				if (GUI.Button (new Rect (10, 10, 100, 50), "RESET")) {
						GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().resetPlayerPos ();
				}
		}
}
