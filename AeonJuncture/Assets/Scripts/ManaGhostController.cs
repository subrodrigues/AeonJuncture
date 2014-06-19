using UnityEngine;
using System.Collections;

public class ManaGhostController : MonoBehaviour
{
		public Transform target;
		public float moveSpeed;
		public float rotationSpeed;
		public float damage = 1.0f;
		private Vector3 initialPosition;
	
		private Transform myTransform;
		public GameObject ghostModel;
		
		public float rayRange; 
		bool startChasing, endChasing;
	
		void Awake ()
		{
				myTransform = transform;
				initialPosition = transform.position;
		}
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				
				damage = 1.0f;
		
				target = go.transform;
				moveSpeed = 2.0f;
				rotationSpeed = 2.667f;
				rayRange = 0.7f;
				
				startChasing = false;
				endChasing = true;
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
				if (distance >= 2.5) {
						startChasing = false;
						
						if (!endChasing) {
								endChasing = true;
								ghostModel.animation.Play ("EndChasing");
						}
						if (endChasing && !ghostModel.animation.IsPlaying ("EndChasing"))
								ghostModel.animation.Play ("IdleMask");
				}
				if (distance < 2.5 && distance >= 0.2) {
						endChasing = false;
						// animation.Play ("run");
						LookAtTarget ();
						
						if (!startChasing) {
								startChasing = true;
								ghostModel.animation.Play ("InitChasing");
						}
						if (startChasing && !ghostModel.animation.IsPlaying ("InitChasing"))
								ghostModel.animation.Play ("ChasingMask");
								
						RaycastHit hit;
						if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, rayRange) 
								&& (hit.transform.tag != "Player") && (hit.transform.tag != "ManaKnight")
								&& (hit.transform.tag != "ManaKnightWithKey")
								&& (hit.transform.tag != "Wall")
								&& (hit.transform.tag != "AttackHit")
								&& (hit.transform.tag != "Quicksand")) { // Se houver uma barreira na direcçao, muda de direccao
								// Stop
						} else {
								// Get a direction vector from us to the target
								myTransform.Translate (Vector3.forward * Time.deltaTime * moveSpeed);
						}
				} else if (distance < 0.2) {
						// animation.Play ("attack");
						// Attack target
						LookAtTarget ();
						
						target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
						transform.position = initialPosition;
				}
		}
}
