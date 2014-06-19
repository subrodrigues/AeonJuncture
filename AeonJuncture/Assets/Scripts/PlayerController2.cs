using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour
{
	// Controlo do Personagem
	CharacterController controller;
	public float runSpeed = 0.00001f;
	public bool shield;
	public int life = 3;
	public static float health;
	public static float maxHealth;
	
	// Camera
	public float moveStepCameraSpeed = 15f;
	public float rotateSpeed = 60f;
	
	// Flags de controlo Quicksand
	public bool touchingQuicksand = false;
	public float minimumAcceleration = 500f;
	public float initialYPosition, initialXPosition, initialZPosition;
	//	public float outOfSandSpeed = 0.04f;
	//	public float buryInSandSpeed = 0.015f;
	private float bellowGround;
	float fallSpeed;
	float fallLimit;
	
	// Variaveis de controlo de Shaking
	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa). You should be able to use LowPassFilter() function instead of avgSamples(). 
	private float lowPassKernelWidthInSeconds = 1.0f;
	private float accelerometerUpdateInterval = 1.0f / 60.0f;
	private float lowPassFilterFactor;
	private Vector3 lowPassValue = Vector3.zero; // should be initialized with 1st sample
	private Vector3 andAcceleration;
	private Vector3 andDeltaAcc;
	
	public GameObject ground;
		
		
	// Attacking control variables
	public bool isAttacking;
	private float fingerStartTime;
	private Vector2 fingerStartPos;
	
	private bool isSwipe;
	private float minSwipeDist;
	private float maxSwipeTime;
	
	private float MAX_TIME_ANIM, HALF_MAX_TIME_ANIM;
	private float timeAtStartOfAnimation;
	private bool isAttackAnimationPlaying;
		
	GameObject[] manaKnight;
	GameObject manaKnightWithKey;
		
	public GameObject life1, life2, life3, shieldObject, attackHit, kalaModel;
		
	public bool hasKey;
		
	// Audio
	AudioClip keyClip, shieldClip;
	AudioSource keySource, shieldSource;
		
	private bool resetPlayer, touchingFall, stayingInTopBridge;
		
	// Game Over
	bool death;
	public float MAX_TIME_GAMEOVER, timeAtStartGameOver;
	
	bool MaxGameOverTimeReached ()
	{
		return (Time.time - timeAtStartGameOver) >= MAX_TIME_GAMEOVER; 
	}
		
	void Awake ()
	{
		keyClip = Resources.Load ("get_key") as AudioClip;
		keySource = AddAudio (keyClip, false, false, 0.9f);
				
		shieldClip = Resources.Load ("kala_shield") as AudioClip;
		shieldSource = AddAudio (shieldClip, false, false, 0.25f);
	}
	
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
		shield = false;
				
				
		// Attacking control
		fingerStartTime = 0.0f;
		fingerStartPos = Vector2.zero;
		isSwipe = false;
		minSwipeDist = 50.0f;
		maxSwipeTime = 0.5f;
		
		isAttacking = false;
		MAX_TIME_ANIM = 0.500f;
		HALF_MAX_TIME_ANIM = MAX_TIME_ANIM / 2.0f;
		kalaModel.animation ["Attack"].speed = 1.37f;
		timeAtStartOfAnimation = 0.0f;
		isAttackAnimationPlaying = false;
				
		manaKnight = GameObject.FindGameObjectsWithTag ("ManaKnight");
		manaKnightWithKey = GameObject.FindGameObjectWithTag ("ManaKnightWithKey");
				
		hasKey = false;
				
		bellowGround = 0.0f;
		resetPlayer = false;
		health = 10.0f;
		maxHealth = 10.0f;
				
				
		touchingFall = false;
		fallSpeed = 0.1f;
		fallLimit = -1.85f;
		stayingInTopBridge = false;
				
		death = false;
		
		MAX_TIME_GAMEOVER = 5.0f;
	}
		
	// Update is called once per frame
	void Update ()
	{
		
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			Application.LoadLevel ("Main_Menu");
		}
		
		if (resetPlayer) {
			resetPlayer = false;
			resetPlayerPos ();
		}
		
		if (shield && !shieldSource.isPlaying) {
			shieldSource.Play ();
		}
		if (!shield && shieldSource.isPlaying)
			shieldSource.Stop ();
						
		if (death && MaxGameOverTimeReached ()) {
			Application.LoadLevel ("Main_Menu");
		}
				
		//CheckAccelerometer (); // Verifica se o jogador se encontra sobre Quicksand e aplica o devido efeito	
		// UpdateMovement ();	
		
		//CheckAccelerometerWindows (); // Verifica se o jogador se encontra sobre Quicksand e aplica o devido efeito
		UpdateMovementKeyboard ();
	}
		
	bool MaxAnimationTimeReached ()
	{
		return (Time.time - timeAtStartOfAnimation) >= MAX_TIME_ANIM; 
	}
		
	bool HalfAnimationTimeReached ()
	{
		return (Time.time - timeAtStartOfAnimation) >= HALF_MAX_TIME_ANIM; 
	}
	
	void UpdateMovement ()
	{
		if (!death) {
			// Se esta a cair
			if (touchingFall) {
				transform.position = new Vector3 (transform.position.x, transform.position.y - fallSpeed, transform.position.z);
				if (transform.position.y <= fallLimit) {
					touchingFall = false;
					ApplyDamage (10.0f);
				}
			}
		
			// Attack animation
			if (isAttacking && !MaxAnimationTimeReached ()) {
				if (!isAttackAnimationPlaying) {
					kalaModel.animation.Play ("Attack");
					isAttackAnimationPlaying = true;
				}
				if (HalfAnimationTimeReached ())
					attackHit.GetComponent<AttackScript> ().setAttack (true);
				
			} else if (isAttacking && MaxAnimationTimeReached ()) { // parou animaçao
				kalaModel.animation.Play ("Idle");
				isAttackAnimationPlaying = false;
				isAttacking = false;
			} else { // No attack animation
				isAttackAnimationPlaying = false;
				isAttacking = false;
			
				if (Input.touchCount > 0) {
					// Check swipe
					for (int i = 0; i < Input.touchCount; i++) {
						Touch touch = Input.touches [i];
						switch (touch.phase) {
						case TouchPhase.Began:
						/* this is a new touch */
							isSwipe = true;
							fingerStartTime = Time.time;
							fingerStartPos = touch.position;
							break;
						
						case TouchPhase.Canceled:
						/* The touch is being canceled */
							isSwipe = false;
							break;
						
						case TouchPhase.Ended:
							float gestureTime = Time.time - fingerStartTime;
							float gestureDist = (touch.position - fingerStartPos).magnitude;
						
							if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
								Vector2 direction = touch.position - fingerStartPos;
								Vector2 swipeType = Vector2.zero;
							
								if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
									// the swipe is horizontal:
									//	swipeType = Vector2.right * Mathf.Sign (direction.x);
									//		if (!isAttacking) {
									isAttacking = true;
									timeAtStartOfAnimation = Time.time;
									//		kalaModel.animation.Play ("Run");
									//		}
								} else {
									// the swipe is vertical:
									//	swipeType = Vector2.up * Mathf.Sign (direction.y);
									//		if (!isAttacking) {
									isAttacking = true;
									timeAtStartOfAnimation = Time.time;
								
								}
							}
						
							break;
						}
					}
				
					// If not attacking, check Mouse and Touch Movement
					if (!isAttacking && Input.touchCount == 1
						&& transform.position.y >= initialYPosition) { // Se ha um click e o jogador se encontra acima do chao
						RaycastHit hit;
					
						shield = false;
						shieldObject.active = false;
					
						Ray ray = Camera.main.ScreenPointToRay (Input.touches [0].position);
						// Se toca no chao
						if (ground.collider.Raycast (ray, out hit, Mathf.Infinity)) {
							// With isometric camera
							//Vector3 target = new Vector3 (hit.point.x - 1.4f, transform.position.y, hit.point.z - 1.4f);
							// With 3d Camera
							Vector3 target = new Vector3 (hit.point.x, initialYPosition, hit.point.z);
						
							MoveTowardsTarget (target);
						
							FaceTouchedPoint (target);
						}
					} else if (!isAttacking && Input.touchCount == 2) { // If not attacking, check Mouse and Touch Movement
						if (!shield) {
							// defend animation
							shieldObject.active = true;
							shield = true;
							kalaModel.animation.Play ("Idle");
						}
					} 
				} else { // If not movement, stay idle
					shield = false;
					shieldObject.active = false;
					if (!kalaModel.animation.IsPlaying ("Hit"))
						kalaModel.animation.Play ("Idle");
				}
			}
		} else { // If death, stay idle
			if (!kalaModel.animation.IsPlaying ("Idle"))
				kalaModel.animation.Play ("Idle");
		}
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
		if (offset.magnitude > .4f) {
			//If we're further away than .1 unit, move towards the target.
			//The minimum allowable tolerance varies with the speed of the object and the framerate. 
			// 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
			offset = offset.normalized + offset.normalized + offset.normalized;
			//normalize it and account for movement speed.
			controller.Move (offset * Time.deltaTime);
			//actually move the character.
			if (!kalaModel.animation.IsPlaying ("Hit"))
				kalaModel.animation.Play ("Walk");
		}
	}
	
	Vector3 LowPassFilter (Vector3 newSample)
	{
		lowPassValue = Vector3.Lerp (lowPassValue, newSample, lowPassFilterFactor);
		
		return lowPassValue;
	}
	
	/*	void CheckAccelerometer ()
		{
				if (touchingQuicksand) {
						andAcceleration = Input.acceleration;
						andDeltaAcc = andAcceleration - LowPassFilter (andAcceleration);

						// Se existe um shaking movement
						if (Mathf.Abs (andDeltaAcc.y) >= .2) {	
								float newYPos = transform.position.y + outOfSandSpeed;
				
								if (newYPos >= initialYPosition) {
										transform.position = new Vector3 (transform.position.x, initialYPosition, transform.position.z);
								} else {
										transform.position = new Vector3 (transform.position.x, newYPos, transform.position.z);
								}
						}

				}   
		}
		
		void CheckAccelerometerWindows ()
		{
				if (touchingQuicksand) {
						// Se existe um shaking movement
						if (Input.GetKey ("s")) {	
								Debug.Log ("SS");
								float newYPos = transform.position.y + outOfSandSpeed;
				
								if (newYPos >= initialYPosition) {
										transform.position = new Vector3 (transform.position.x, initialYPosition, transform.position.z);
								} else {
										transform.position = new Vector3 (transform.position.x, newYPos, transform.position.z);
								}
						}
			
				}   
		}
	*/
	void OnTriggerEnter (Collider other)
	{
//				Debug.Log (other.tag);
		if (other.tag == "TopBridge")
			stayingInTopBridge = true;
						
		if (other.tag == "Fall") {
			touchingFall = true;
		}
	}
		
	void OnTriggerStay (Collider other)
	{
		if (other.tag == "TopBridge") {
			stayingInTopBridge = true;
		}
	}
		
	void OnTriggerExit (Collider other)
	{
		/*		if (other.tag == "Quicksand") {
						touchingQuicksand = false;
				}
		*/
		if (other.tag == "Fall") {
			touchingFall = false;
		}
		if (other.tag == "TopBridge") {
			stayingInTopBridge = false;
		}
	}
		
	public void resetPlayerPos ()
	{
		//	restartLives ();
		transform.position = new Vector3 (initialXPosition, initialYPosition, initialZPosition);
		health = 10.0f;
	}
		
	void UpdateMovementKeyboard ()
	{
		if (!death) {
			// Se esta a cair
			if (touchingFall) {
				transform.position = new Vector3 (transform.position.x, transform.position.y - fallSpeed, transform.position.z);
				if (transform.position.y <= fallLimit) {
					touchingFall = false;
					ApplyDamage (10.0f);
				}
			}
		
			// Attack animation
			if (isAttacking && !MaxAnimationTimeReached ()) {
				if (!isAttackAnimationPlaying) {
					kalaModel.animation.Play ("Attack");
					isAttackAnimationPlaying = true;					
				}
				if (HalfAnimationTimeReached ())
					attackHit.GetComponent<AttackScript> ().setAttack (true);
			} else if (isAttacking && MaxAnimationTimeReached ()) { // parou animaçao
				kalaModel.animation.Play ("Idle");
				isAttackAnimationPlaying = false;
				isAttacking = false;			
			} else { // No attack animation
				isAttackAnimationPlaying = false;
				isAttacking = false;
			
				if (Input.GetMouseButton (0) || Input.GetKeyDown ("a") || Input.GetKey ("d")) {
					// Check swipe
					if (Input.GetKeyDown ("a")) {
						isAttacking = true;
						timeAtStartOfAnimation = Time.time;
					}
								
				
					// If not attacking, check Mouse and Touch Movement
					if (!isAttacking && Input.GetMouseButton (0)
						&& transform.position.y >= initialYPosition) { // Se ha um click e o jogador se encontra acima do chao
						RaycastHit hit;
					
						shield = false;
						shieldObject.active = false;
					
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						// Se toca no chao
						if (ground.collider.Raycast (ray, out hit, Mathf.Infinity)) {
							// With 3d Camera
							Vector3 target = new Vector3 (hit.point.x, initialYPosition, hit.point.z);
						
							MoveTowardsTarget (target);
						
							FaceTouchedPoint (target);
						}

					} else if (!isAttacking && Input.GetKeyDown ("d")) { // If not attacking, check Mouse and Touch Movement
						if (!shield) {
							// defend animation
							shieldObject.active = true;
							shield = true;
							kalaModel.animation.Play ("Idle");
						}
					} 
				} else { // If not movement, stay idle
					shield = false;
					shieldObject.active = false;
					if (!kalaModel.animation.IsPlaying ("Hit"))
						kalaModel.animation.Play ("Idle");
				}
			}
		} else { // If death, stay idle
			if (!kalaModel.animation.IsPlaying ("Idle"))
				kalaModel.animation.Play ("Idle");
		}
	}
	
	void ApplyDamage (float damage)
	{
		if (life == 0 && !death) {
			timeAtStartGameOver = Time.time;
			GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("TurnGameOver", true, SendMessageOptions.DontRequireReceiver);
			death = true;
		}
		if (transform.position.x < 11.9f) {		
			health -= damage;
			kalaModel.animation.Play ("Hit");
			if (health <= 0.0f) {
				health = 10.0f;
				
				if (life >= 0) {
					life -= 1;
				}
				resetPlayer = true;
				UpdateLives ();
			}
		}
	}
	
	void ApplyPotion (float heal)
	{
		if (health < 10.0f) {
			health += heal;
		}
		if (health > 10.0f) {
			health = 10.0f;
		}
	}
		
/*		void KeyTrigger (bool key)
		{
				keySource.Play ();
				hasKey = key;
				GameObject keyObject = GameObject.FindGameObjectWithTag ("Key");
				keyObject.transform.position = transform.position;
				keyObject.GetComponent<MeshRenderer> ().enabled = false;
		}
*/	
	void UpdateLives ()
	{
		if (life == 0) {
			life1.active = false;
			life2.active = false;
			life3.active = false;
		}
		if (life == 1) {
			life2.active = false;
			life3.active = false;
		}
		if (life == 2) {
			life3.active = false;
		}
			
	}
		
	/*	void restartLives ()
		{
				life = 3;
				life1.active = true;
				life2.active = true;
				life3.active = true;
		}
	*/	
	public int getLives ()
	{
		return life;
	}
		
	AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol)
	{
		AudioSource newAudio = gameObject.AddComponent<AudioSource> ();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	
	/*	public bool HasKey ()
		{
				return hasKey;
		}
	*/
}
