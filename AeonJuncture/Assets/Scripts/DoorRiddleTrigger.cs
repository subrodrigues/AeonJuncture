using UnityEngine;
using System.Collections;

public class DoorRiddleTrigger : MonoBehaviour
{
	
		GameObject go;
		Transform target;
		float initialPositionY, pressedPositionY;
	
		// Use this for initialization
		void Start ()
		{
				if (this.tag == "DoorTrigger1") {
						GameObject go = GameObject.FindGameObjectWithTag ("Door1");
						target = go.transform;
				}
				if (this.tag == "DoorTrigger2") {
						GameObject go = GameObject.FindGameObjectWithTag ("Door2");
						target = go.transform;
				}
				if (this.tag == "DoorTrigger3") {
						GameObject go = GameObject.FindGameObjectWithTag ("Door3");
						target = go.transform;
				}
		
				initialPositionY = transform.position.y;
				pressedPositionY = 0.2016709f;
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player") {
						this.transform.position = new Vector3 (transform.position.x, pressedPositionY, transform.position.z);
						target.transform.SendMessage ("StartOpening", true, SendMessageOptions.DontRequireReceiver);
				}
		}
	
		void InitialTriggerPosition (bool b)
		{
				this.transform.position = new Vector3 (transform.position.x, initialPositionY, transform.position.z);
		}
}
