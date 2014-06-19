using UnityEngine;
using System.Collections;

public class HitBump : MonoBehaviour
{
		CharacterController controller;
		public bool hit, runningHit;
		// Transforms to act as start and end markers for the journey.
		public Vector3 target;
	
		float speed;
	
		// Time when the movement started.
		private float startTime;	
		private float MAX_TIME_ANIM;

		Vector3 dir;
	
		void Start ()
		{
				controller = (CharacterController)GetComponent (typeof(CharacterController));
				speed = 3f;
				hit = false;
				MAX_TIME_ANIM = 0.1f;
		}
		
		bool MaxAnimationTimeReached ()
		{
				return (Time.time - startTime) >= MAX_TIME_ANIM; 
		}
		
		void InitHit ()
		{
				hit = true;
				target = transform.position - dir;
				// Keep a note of the time the movement started.
				startTime = Time.time;
		}
	
		void FixedUpdate ()
		{
			
				if (hit) {
//						float step = speed * Time.deltaTime;
//						transform.position = Vector3.MoveTowards (transform.position, target, step);

						Vector3 offset = target - transform.position;
						controller.Move (offset * Time.deltaTime * speed);
				}
				if (MaxAnimationTimeReached ()) {
						//		this.gameObject.rigidbody.AddForce (Vector3.zero);
						//		this.transform.rigidbody.velocity = Vector3.zero;
						hit = false;
				}
		}
		// Follows the target position like with a spring
		void Update ()
		{
		}
		
		void ApplyBumpHit (Vector3 d)
		{
				//	Debug.Log ("BUMP");
				dir = d;
				dir *= -1;
				InitHit ();
		}
}
