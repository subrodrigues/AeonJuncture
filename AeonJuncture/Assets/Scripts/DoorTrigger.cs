using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
		Transform target;
		
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Door");
				target = go.transform;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player" 
						&& GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().HasKey ()) {
						target.transform.SendMessage ("StartOpening", true, SendMessageOptions.DontRequireReceiver);
						gameObject.active = false;
				}
		}
}
