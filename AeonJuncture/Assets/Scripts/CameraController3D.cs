using UnityEngine;
using System.Collections;

public class CameraController3D : MonoBehaviour
{
	
		// How fast the camera moves
		float moveSpeed = 2.5f;
		
		public GameObject gameOver;
	
		// Use this for initialization
		void Start ()
		{
				this.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
				Vector3 originalPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
				Vector3 playerPos = new Vector3 (originalPos.x, originalPos.y + 6, originalPos.z - 3.5f);
				transform.position = Vector3.Lerp (transform.position, playerPos, Time.deltaTime * moveSpeed);
				transform.localEulerAngles = new Vector3 (60.0f, 0, 0);

		}
		
		void TurnGameOver (bool b)
		{
				gameOver.active = true;
		}

}
