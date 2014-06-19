using UnityEngine;
using System.Collections;

public class InitTriggerTalk2 : MonoBehaviour
{
		GameObject sila;
	
		// Use this for initialization
		void Start ()
		{
				sila = GameObject.FindGameObjectWithTag ("Sila");
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnTriggerEnter (Collider other)
		{
				//	Debug.Log (other.gameObject.tag);
				if (other.gameObject.tag == "Player") {
						sila.transform.SendMessage ("SetInitHint", true, SendMessageOptions.DontRequireReceiver);
				}
		}
	
		void OnTriggerExit (Collider other)
		{
				if (other.gameObject.tag == "Player") {
						sila.transform.SendMessage ("SetInitHint", false, SendMessageOptions.DontRequireReceiver);
				}
		}
}
