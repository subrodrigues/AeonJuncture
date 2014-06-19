using UnityEngine;
using System.Collections;

public class WebBullet : MonoBehaviour
{
		public Transform target;
		Vector3 initialTargetPosition;
		public float speed;
		public float damage = 2.0f;
		
		// Use this for initialization
		void Start ()
		{
				target = GameObject.FindGameObjectWithTag ("Player").transform;
				initialTargetPosition = new Vector3 (target.position.x, target.position.y, target.position.z);
				speed = 8.0f;
				LookAtTarget ();
		}
		
	
		// Update is called once per frame
		void Update ()
		{
				damage = 2.0f;
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, initialTargetPosition, step); 
				
				if (transform.position == initialTargetPosition)
						Destroy (gameObject);
		}
		
		void LookAtTarget ()
		{
				Vector3 newRotation = Quaternion.LookRotation (target.position - transform.position).eulerAngles;
				transform.rotation = Quaternion.Euler (newRotation);
		}
		
		void OnTriggerEnter (Collider other)
		{
				if (other.tag == "Shield") {
						Destroy (gameObject);
				} else if (other.tag == "Player") {
						target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
						Destroy (gameObject);
				}
				
		}
	
		void OnTriggerExit (Collider other)
		{
				if (other.tag == "Player") {
				}
		}
}
