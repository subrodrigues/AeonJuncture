using UnityEngine;
using System.Collections;

public class DeathScript : MonoBehaviour
{
		public Transform target;
		public float damage;

		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
		
				damage = 10.0f;
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
}
