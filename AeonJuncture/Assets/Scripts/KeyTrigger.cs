using UnityEngine;
using System.Collections;

public class KeyTrigger : MonoBehaviour
{
		Transform target;
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player") {
						target.transform.SendMessage ("KeyTrigger", true, SendMessageOptions.DontRequireReceiver);
						gameObject.active = false;
				}
		}
}
