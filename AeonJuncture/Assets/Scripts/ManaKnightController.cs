using UnityEngine;
using System.Collections;

public class ManaKnightController : MonoBehaviour
{
		public Transform target;
		public float moveSpeed;
		public float chasingSpeed;
		public float rotationSpeed;
	
		private Transform myTransform;
		
		// Random Movement
		public bool randomMovement = false;
		public Vector3 moveDir = Vector3.zero;
		public float rayRange; 
/*		public Vector3 randVel; // Holds the random velocity
		public float switchDirection = 3;
		public float curTime = 0;
*/
	
		void Awake ()
		{
				myTransform = transform;
		}
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
		
				target = go.transform;
				moveSpeed = 1.0f;
				chasingSpeed = 2.0f;
				rotationSpeed = 0.5f;
				
				// Random movement
				randomMovement = false;
				moveDir = Vector3.zero;
				calculateRandomAxisMovement ();
				rayRange = 0.7f;
				/*		switchDirection = 3;
				curTime = 0;
				SetVel ();
		*/
		}
		
		void FixedUpdate ()
		{	
				if (randomMovement) {
						//	movesRandomlyAndLook (); // Aplica random movement
						Vector3 newRotation = Quaternion.LookRotation (moveDir).eulerAngles;
						newRotation.x = 0;
						newRotation.z = 0;
						transform.rotation = Quaternion.Euler (newRotation);
						this.transform.rigidbody.velocity = moveDir;	
				}
		}

		// Update is called once per frame
		void Update ()
		{
				Debug.DrawLine (target.position, myTransform.position, Color.red);
				float distance = Vector3.Distance (target.transform.position, transform.position);
		
				//	Debug.Log ("DISTANCE: " + distance);
				if (distance >= 3) {
						randomMovement = true;
						// animation.Play ("walk");
						// Moves randomly
						//	calculateRandomMovement ();
						if (Physics.Raycast (transform.position, moveDir, rayRange)) {
								calculateRandomAxisMovement ();
						} else if (this.transform.position.x <= -10.55 ||
								this.transform.position.x >= 10.55 ||
								this.transform.position.z <= -6.8 ||
								this.transform.position.z >= 6.8) {
								moveDir *= -1;
						}
				} else if (distance < 3) {
						randomMovement = false;
						// animation.Play ("run");
						LookAtTarget ();
						// Get a direction vector from us to the target
						myTransform.Translate (Vector3.forward * Time.deltaTime * chasingSpeed);
				} else if (distance < 1) {
						randomMovement = false;
						// animation.Play ("attack");
						// Attack target
						LookAtTarget ();
				}
		}
		
		void calculateRandomAxisMovement ()
		{
				float rand = Random.value;
				if (rand >= 0 && rand <= .25) {
						// Move front
						moveDir = new Vector3 (1, 0, 0);
				} else if (rand > 0.25 && rand <= .5) {
						// Move backwards
						moveDir = new Vector3 (-1, 0, 0);
				} else if (rand > 0.5 && rand <= .75) {
						// Move backwards
						moveDir = new Vector3 (0, 0, 1);
				} else if (rand > 0.75 && rand <= 1.0) {
						// Move right
						moveDir = new Vector3 (0, 0, -1);
				}
		}

		void LookAtTarget ()
		{
				Vector3 newRotation = Quaternion.LookRotation (target.position - transform.position).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Euler (newRotation);
		}
		
	
		/*			void SetVel ()
		{
				if (Random.value > .5) {
						randVel.x = 1 * 2 * Random.value;
				} else {
						randVel.x = -1 * 2 * Random.value;
				}
		
				if (Random.value > .5) {
						randVel.z = 1 * 2 * Random.value;
				} else {
						randVel.z = -1 * 2 * Random.value;
				}
		}
	
			void movesRandomlyAndLook ()
		{
				Vector3 newRotation = Quaternion.LookRotation (randVel).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Euler (newRotation);
				this.transform.rigidbody.velocity = randVel;
		}
void calculateRandomMovement ()
		{
				if (curTime < switchDirection) {
						curTime += 1 * Time.deltaTime;
				
				} else {	
						SetVel ();
				
						if (Random.value > .5) {
								switchDirection += Random.value;
								Debug.Log ("CHANGE");
						} else {
								switchDirection -= Random.value;
								Debug.Log ("CHANGE");
						}
					
						if (switchDirection < 1) {
								switchDirection = 1 + Random.value;
								Debug.Log ("CHANGE");
						}
					
						curTime = 0;
				}
		}
		*/
}
