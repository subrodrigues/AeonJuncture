using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour
{
		private float range;
		private float attackInterval;
		private float meleeDamage;
		private float nextAttack;
		public bool attack;
	
		void Start ()
		{
				attack = false;
				nextAttack = 0f;
				meleeDamage = 40.0f;
				attackInterval = 0.3f;
				range = 0.45f;
		}
	
		void MeleeAttack ()
		{
				if (Time.time > nextAttack) { // only repeat attack after attackInterval
						nextAttack = Time.time + attackInterval;
						// get all colliders whose bounds touch the sphere
						Collider[] colls = Physics.OverlapSphere (transform.position, range);
						for (int i = 0; i < colls.Length; i++) {
								if (colls [i] && colls [i].tag == "WeakPoint") { // if the object is an enemy Weak Point...
										//		Debug.Log (i);
										// check the actual distance to the melee center
										float dist = Vector3.Distance (colls [i].transform.position, transform.position);
										if (dist <= range) { // if inside the range...
												// apply damage to the hit object
												colls [i].transform.parent.gameObject.SendMessage ("ApplyDamage", meleeDamage);
										}
								} else if (colls [i] && colls [i].tag == "WeakPointSpider") { // if the object is an enemy Weak Point...
										//			Debug.Log (i);
										// check the actual distance to the melee center
										float dist = Vector3.Distance (colls [i].transform.position, transform.position);
										if (dist <= range) { // if inside the range...
												// apply damage to the hit object
												colls [i].transform.parent.parent.gameObject.SendMessage ("ApplyDamage", meleeDamage);
										}
								}
						}
				}
		}
	
		void Update ()
		{
				if (attack) {
						attack = false;
						MeleeAttack ();
				}
		}
		public void setAttack (bool b)
		{
				this.attack = b;
		}
}
