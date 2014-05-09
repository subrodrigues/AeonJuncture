using UnityEngine;
using System.Collections;

public class ManaGhostController : MonoBehaviour
{
		public Transform target;
		public float moveSpeed;
		public float rotationSpeed;
	
		private Transform myTransform;
	
		void Awake ()
		{
				myTransform = transform;
		}
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
		
				target = go.transform;
				moveSpeed = 1.97f;
				rotationSpeed = 2.0f;
		}
	
		void LookAtTarget ()
		{
				Vector3 newRotation = Quaternion.LookRotation (target.position - transform.position).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Euler (newRotation);
		}
	
		// Update is called once per frame
		void Update ()
		{
				Debug.DrawLine (target.position, myTransform.position, Color.red);
				float distance = Vector3.Distance (target.transform.position, transform.position);
		
				//		Debug.Log ("DISTANCE: " + distance);
				if (distance >= 3) {
						// animation.Play ("walk");
						// Moves randomly
				}
				if (distance < 3) {
						// animation.Play ("run");
						LookAtTarget ();
						// Get a direction vector from us to the target
						myTransform.Translate (Vector3.forward * Time.deltaTime * moveSpeed);
				} else if (distance < 1) {
						// animation.Play ("attack");
						// Attack target
						LookAtTarget ();
				}
		}
}
