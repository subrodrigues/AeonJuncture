using UnityEngine;
using System.Collections;

public class SwampScript : MonoBehaviour
{

		public Transform target;
		public float damage;
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
				
				damage = 0.2f;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player") {
						target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
				}
		}
		
		void OnTriggerStay (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player")
						target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		
		}
}
