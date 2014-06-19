using UnityEngine;
using System.Collections;

public class CameraChangeScript : MonoBehaviour
{
	Vector3 target;
	public static bool levelSelection, backToMain;
		
	// Use this for initialization
	void Start ()
	{
		levelSelection = false;
		backToMain = false;
				
		target = new Vector3 (-90f, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) { 
			Application.Quit ();
		}
		
		if (levelSelection) {
			transform.RotateAround (transform.position, Vector3.left, Time.deltaTime * 120f);
			if (transform.rotation.eulerAngles.x <= 270.0f) {
				levelSelection = false;
			}
		}
		if (backToMain) {
			transform.RotateAround (transform.position, Vector3.right, Time.deltaTime * 120f);
			Debug.Log (transform.rotation.eulerAngles.x);
			if (transform.rotation.eulerAngles.x >= 358.0f || transform.rotation.eulerAngles.x <= 5.0f) {
				transform.rotation = new Quaternion (0.0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
				backToMain = false;
			}
		}
	}
}
