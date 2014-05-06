using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
		// Controlo do Personagem
		CharacterController controller;
		public float runSpeed = 0.5f;
	
		// Camera
		public float moveStepCameraSpeed = 15f;
		public float rotateSpeed = 60f;
	
		// Flags de controlo Quicksand
		public bool touchingQuicksand = false;
		public float minimumAcceleration = 500f;
		public float initialYPosition, initialXPosition, initialZPosition;
		public float outOfSandSpeed = 0.04f;
		public float buryInSandSpeed = 0.015f;
	
		// Variaveis de controlo de Shaking
		// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa). You should be able to use LowPassFilter() function instead of avgSamples(). 
		private float lowPassKernelWidthInSeconds = 1.0f;
		private float accelerometerUpdateInterval = 1.0f / 60.0f;
		private float lowPassFilterFactor;
		private Vector3 lowPassValue = Vector3.zero; // should be initialized with 1st sample
		private Vector3 andAcceleration;
		private Vector3 andDeltaAcc;
	
		public GameObject ground;

		// Use this for initialization
		void Start ()
		{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
				lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
				controller = (CharacterController)GetComponent (typeof(CharacterController));
		
				initialYPosition = transform.position.y;
				initialXPosition = transform.position.x;
				initialZPosition = transform.position.z;
				touchingQuicksand = false;
		}
	
		void UpdateMovement ()
		{
				// Keyboard Movement
				/*	float z = -Input.GetAxis ("Horizontal");
		float x = Input.GetAxis ("Vertical");
		
		Vector3 inputVec = new Vector3 (x, 0, z) * runSpeed;
		
		controller.Move (inputVec * Time.deltaTime);
		*/
		
				// Mouse and Touch Movement
				if (Input.GetMouseButton (0) && transform.position.y >= initialYPosition) { // Se ha um click e o jogador se encontra acima do chao
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (ground.collider.Raycast (ray, out hit, Mathf.Infinity)) {
								// With isometric camera
								//Vector3 target = new Vector3 (hit.point.x - 1.4f, transform.position.y, hit.point.z - 1.4f);
								// With 3d Camera
								Vector3 target = new Vector3 (hit.point.x, initialYPosition, hit.point.z);
			
								MoveTowardsTarget (target);
			
								FaceTouchedPoint (target);
						}
				} else
						animation.Play ("idle");
				
				// Se esta na Quicksand
				if (touchingQuicksand) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - buryInSandSpeed, transform.position.z);
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				CheckAccelerometer (); // Verifica se o jogador se encontra sobre Quicksand e aplica o devido efeito
			
				UpdateMovement ();	
		}
	
		void FaceTouchedPoint (Vector3 target)
		{
				float distance = Vector3.Distance (transform.position, target);
				//	Debug.Log ("DISTANCIA: " + distance);
				if (distance > 0.25) {
						Vector3 targetDir = target - transform.position;
						float step = rotateSpeed * Time.deltaTime;
						float moveStep = moveStepCameraSpeed * Time.deltaTime;
						Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
						transform.rotation = Quaternion.LookRotation (newDir);
				}
		}
	
		void MoveTowardsTarget (Vector3 target)
		{
				Vector3 offset = target - transform.position;
				//Get the difference
				if (offset.magnitude > .1f) {
						//If we're further away than .1 unit, move towards the target.
						//The minimum allowable tolerance varies with the speed of the object and the framerate. 
						// 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
						offset = offset.normalized + offset.normalized;
						//normalize it and account for movement speed.
						controller.Move (offset * Time.deltaTime);
						//actually move the character.
						animation.Play ("walk");
				}
		}
	
	
		Vector3 LowPassFilter (Vector3 newSample)
		{
				lowPassValue = Vector3.Lerp (lowPassValue, newSample, lowPassFilterFactor);
		
				return lowPassValue;
		}
	
		void CheckAccelerometer ()
		{
				if (touchingQuicksand) {
						andAcceleration = Input.acceleration;
						andDeltaAcc = andAcceleration - LowPassFilter (andAcceleration);

						/*	if (Mathf.Abs (andDeltaAcc.x) >= .3) {
				// Do something
			}
		*/	
						// Se existe um shaking movement
						if (Mathf.Abs (andDeltaAcc.y) >= .2) {	
								float newYPos = transform.position.y + outOfSandSpeed;
				
								if (newYPos >= initialYPosition) {
										transform.position = new Vector3 (transform.position.x, initialYPosition, transform.position.z);
								} else {
										transform.position = new Vector3 (transform.position.x, newYPos, transform.position.z);
								}
						}
	
						/*	if (Mathf.Abs (andDeltaAcc.z) >= .3) {

				// Do something
			}    
		*/
				}   
		}
	
		void OnTriggerEnter (Collider other)
		{
				Debug.Log (other.tag);
				if (other.tag == "Quicksand") {
						touchingQuicksand = true;
						//	transform.position = new Vector3 (transform.position.x, transform.position.y - 0.15f, transform.position.z);
				}
		}
		
		void OnTriggerExit (Collider other)
		{
				if (other.tag == "Quicksand") {
						touchingQuicksand = false;
				}
		}
		
		public void resetPlayerPos ()
		{
				transform.position = new Vector3 (initialXPosition, initialYPosition, initialZPosition);
		}
}
