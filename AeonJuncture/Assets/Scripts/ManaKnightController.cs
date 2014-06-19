using UnityEngine;
using System.Collections;

public class ManaKnightController : MonoBehaviour
{
		public Transform target;
		public float moveSpeed;
		public float chasingSpeed;
		public float rotationSpeed;
	
		private Transform myTransform;
		
		// Attack
		public bool attackPlayer;
		public float damage = 2.0f;
		
		// Random Movement
		public bool knightIsStopped = false;
		public Vector3 moveDir = Vector3.zero;
		public float rayRange; 
		public float fieldOfViewRange;
		public bool chaseMode;
		public bool rotatePositionAnimation;
		public float MAX_TIME_ROTATE;
		private float timeAtStartAnimation;
		private Vector3 previousPosition;
		private int previousPositionCount;
		
		public float MAX_TIME_ATTACK, HALF_MAX_TIME_ATTACK, MAX_TIME_DEAD;
		private float timeAtStartAttack;
		private bool isAttackingAnimation;
		private bool halfAttackReached;
		private bool isCalculatingRandomMovement;
		
		// Mana Knight Tracer
		private bool manaKnightTracer;
		public float MAX_TIME_TRACER;
		public float MAX_TIME_STUN;
		private float timeAtStartTracer, timeAtStartStun;
		private bool stun;
	
		// Audio
		AudioClip moanClip;
		AudioClip walkClip;
		AudioClip stunClip;
		AudioSource moanSource;
		AudioSource walkSource;
		AudioSource stunSource;


		public float health;
		
		public GameObject manaKnight, stunShield, weakPointShield;
	
		public GameObject explosion;
		
		private bool isDead;
		private float timeAtStartDead;
		
		bool hitIsPlaying, playingEndDefense;
		
		void Awake ()
		{
				//	myTransform = transform;
				walkClip = Resources.Load ("knight_walking") as AudioClip;
				moanClip = Resources.Load ("knight_moan") as AudioClip;
				stunClip = Resources.Load ("knight_stun") as AudioClip;
				walkSource = AddAudio (walkClip, true, true, 0.09f);
				moanSource = AddAudio (moanClip, false, false, 0.4f);
				stunSource = AddAudio (stunClip, true, true, 0.7f);
				//	audioEngine = AddAudio (clipEngine, true, true, 0.4);
		}
		
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
		
				health = 100.0f;
				walkSource.Play ();
		
				target = go.transform;
				moveSpeed = 1.333f;
				chasingSpeed = 2.4f;
				rotationSpeed = 3.333f;
				
				// Random movement
				knightIsStopped = false;
				moveDir = Vector3.zero;
				calculateRandomAxisMovement ();
				rayRange = 0.6f;
				previousPosition = transform.position;
				previousPositionCount = 0;
				
				fieldOfViewRange = 100f;
				chaseMode = false;
				rotatePositionAnimation = false;
				MAX_TIME_ROTATE = 0.9f;
				attackPlayer = false;
				
				MAX_TIME_ATTACK = 1.082f;
				HALF_MAX_TIME_ATTACK = MAX_TIME_ATTACK / 2.0f;
				isAttackingAnimation = false;
				halfAttackReached = false;
			
				MAX_TIME_TRACER = 10.0f;
				MAX_TIME_STUN = 4.0f;
				MAX_TIME_DEAD = 0.5f;
			
				manaKnightTracer = false;
				stun = false;
				isCalculatingRandomMovement = false;
				
				isDead = false;
				playingEndDefense = false;
		}
		
		bool MaxAnimationTimeReached ()
		{
				return (Time.time - timeAtStartAnimation) >= MAX_TIME_ROTATE; 
		}
		
		bool MaxAttackAnimationTimeReached ()
		{
				return (Time.time - timeAtStartAttack) >= MAX_TIME_ATTACK; 
		}
		bool HalfAttackAnimationTimeReached ()
		{
				return (Time.time - timeAtStartAttack) >= HALF_MAX_TIME_ATTACK; 
		}
	
		bool MaxTracerAnimationTimeReached ()
		{
				return (Time.time - timeAtStartTracer) >= MAX_TIME_TRACER; 
		}
		
		bool MaxStunAnimationTimeReached ()
		{
				return (Time.time - timeAtStartStun) >= MAX_TIME_STUN; 
		}
		
		bool MaxDeadAnimationTimeReached ()
		{
				return (Time.time - timeAtStartDead) >= MAX_TIME_DEAD; 
		}
	
		// Update is called once per frame
		void Update ()
		{	
				if (isDead) {
						if (MaxDeadAnimationTimeReached () && !moanSource.isPlaying) {
								stun = false;
								stunShield.active = false;
								stunSource.Stop ();
								weakPointShield.active = false;
								this.GetComponent<BoxCollider> ().enabled = false;
								this.GetComponent<SphereCollider> ().enabled = false;
						}
				} else if (!manaKnight.animation.IsPlaying ("hit")) {
						//	manaKnight.animation.Play ("idle");
						
						if (stun && !manaKnight.animation.IsPlaying ("initDefense")) { // Terminou animaçao, fica a defender
								manaKnight.animation.Play ("defense");
						}
 
			
						if (transform.position.y != 0.6892f) {
								transform.position = new Vector3 (transform.position.x, 0.6892f, transform.position.z);
						}

						if (manaKnightTracer) {
								gameObject.GetComponent<TrailRenderer> ().enabled = true; 
						} else
								gameObject.GetComponent<TrailRenderer> ().enabled = false; 
						
						
						if (previousPosition == transform.position && !knightIsStopped) { // Is stuck
								previousPositionCount++;
						} else
								previousPositionCount = 0;
						
						//		Debug.Log ("STUCK: " + previousPositionCount);
						if (previousPositionCount == 10) { // if stuck
								previousPositionCount = 0;
								calculateRandomAxisMovement (); // Calcula nova direcçao
								knightIsStopped = true;
						} 

						if (!chaseMode && knightIsStopped && rotatePositionAnimation) { // Animaçao de rodar
								Quaternion neededRotation = Quaternion.LookRotation (moveDir);  
								transform.rotation = Quaternion.RotateTowards (transform.rotation, neededRotation, Time.deltaTime * 150f);     
						}
		
						RaycastHit hit;
				
						Vector3 fwd = transform.TransformDirection (Vector3.forward);
						Vector3 direction = GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position;

						Debug.DrawLine (target.position, transform.position, Color.red);
						float distance = Vector3.Distance (target.transform.position, transform.position);
		
						if (!stun) {
								if (chaseMode && distance < 3) { // So segue se estiver dentro de raio (< 3)
										//	animation.Play ("idle");
										//	LookAtTarget ();
										//	Quaternion rot = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
										LookAtTarget ();
										//		rot.x = 0;
										//			rot.z = 0;
										//	transform.rotation = rot;
								
										if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, rayRange) 
												&& (hit.transform.tag != "Player") && (hit.transform.tag != "Ghost")) { // Se houver uma barreira na direcçao, muda de direccao
												calculateRandomAxisMovement (); // Calcula nova direcçao
												knightIsStopped = true;
												chaseMode = false;
										} else {
												if (distance < 0.55) { // Se estiver proximo ataca
														if (!isAttackingAnimation) {
																// animation.Play ("attack");
																isAttackingAnimation = true;
																manaKnight.animation.Play ("attack");
																timeAtStartAttack = Time.time;
														}
														attackPlayer = true;
												} else {
														attackPlayer = false;
														transform.position += transform.forward * Time.deltaTime * chasingSpeed;
												}
										}
								} else { // Se nao esta em chase mode ou sai do range
										chaseMode = false;
										if (!manaKnight.animation.isPlaying)
												manaKnight.animation.Play ("idle");
					
										// Se estiver dentro do range, conseguir ver o player e houver um raycast a tocar no player
										if (distance < 3 && ((Vector3.Angle (direction, transform.forward)) < fieldOfViewRange)
												&& (Physics.Raycast (transform.position, direction, out hit, 3)) && (hit.transform.tag == "Player")) {
												//	Debug.Log ("Can see player");
												//	LookAtTarget ();
												chaseMode = true;
												knightIsStopped = false;
										} else if (Physics.Raycast (transform.position, moveDir, out hit, rayRange) 
												&& (hit.transform.tag != "Player") && (hit.transform.tag != "Ghost")) { // Se houver uma barreira na direcçao, muda de direccao
												calculateRandomAxisMovement (); // Calcula nova direcçao
												knightIsStopped = true;
										} else if (knightIsStopped && direction == moveDir) { // Se nao ha barreira na direcçao e ja rodou, move-se
												knightIsStopped = false;
										} else if (knightIsStopped && !rotatePositionAnimation) { // Se nao houver barreira e estiver parado, começa a rodar para a nova direcçao
												rotatePositionAnimation = true;
												timeAtStartAnimation = Time.time;
										}
								}
				
								if (knightIsStopped) // Se Knight esta parado, retira toda a velocidade restante
										this.transform.rigidbody.velocity = Vector3.zero;
								else
										this.transform.rigidbody.velocity = moveDir;	
		
								if (!knightIsStopped && !chaseMode) { // Random Movement
										rotatePositionAnimation = false;
										Vector3 newRotation = Quaternion.LookRotation (moveDir).eulerAngles;
										newRotation.x = 0;
										//	newRotation.y = 0;
										newRotation.z = 0;
										transform.rotation = Quaternion.Euler (newRotation);
										this.transform.rigidbody.velocity = moveDir;	
								}
						}
		
						if (stun && MaxStunAnimationTimeReached () && !playingEndDefense) {
								stun = false;
								playingEndDefense = true;
								stunShield.active = false;
								stunSource.Stop ();
								
								manaKnight.animation.Stop ("defense");
								manaKnight.animation.Play ("endDefense");
								weakPointShield.active = false;
								
								if (!walkSource.isPlaying)
										walkSource.Play ();
						} else if (stun && MaxStunAnimationTimeReached () && !manaKnight.animation.IsPlaying ("endDefense")) {
								playingEndDefense = false;
								manaKnight.animation.Play ("idle");
						}
		
						// Desactiva o Tracer
						if (manaKnightTracer && MaxTracerAnimationTimeReached ()) { 
								manaKnightTracer = false;
						}
		
						// Acabou a animaçao da rotaçao. Activa o movimento
						if (knightIsStopped && MaxAnimationTimeReached () && rotatePositionAnimation) { // Verifica se animaçao terminou para mover o knight
								knightIsStopped = false;
								rotatePositionAnimation = false;
						}
		
						// Se chegou a metade da animaçao, envia o bump
						if (isAttackingAnimation && !halfAttackReached && HalfAttackAnimationTimeReached ()) {
								halfAttackReached = true;
								target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
								target.transform.SendMessage ("ApplyBumpHit", transform.TransformDirection (Vector3.forward), SendMessageOptions.DontRequireReceiver);
						}
		
						// Acabou a animaçao do ataque.
						if (isAttackingAnimation && MaxAttackAnimationTimeReached ()) {
								isAttackingAnimation = false;
								halfAttackReached = false;
								attackPlayer = false;
								//	target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
						}	
		
						previousPosition = transform.position;
				}
		}
		
		void calculateRandomAxisMovement ()
		{
				if (!isCalculatingRandomMovement) {
						isCalculatingRandomMovement = true;
						float rand = Random.value;
						if (rand >= 0 && rand <= .25) {
								// Move front
								Vector3 front = new Vector3 (1, 0, 0);
								if (moveDir != front)
										moveDir = front;
								else
										moveDir = new Vector3 (-1, 0, 0);
						} else if (rand > 0.25 && rand <= .5) {
								// Move backwards
								Vector3 back = new Vector3 (-1, 0, 0);
								if (moveDir != back)
										moveDir = back;
								else
										moveDir = new Vector3 (1, 0, 0);
						} else if (rand > 0.5 && rand <= .75) {
								// Move left
								Vector3 left = new Vector3 (0, 0, 1);
								if (moveDir != left)
										moveDir = left;
								else
										moveDir = new Vector3 (0, 0, -1);
						} else if (rand > 0.75 && rand <= 1.0) {
								// Move right
								Vector3 right = new Vector3 (0, 0, -1);
								if (moveDir != right)
										moveDir = right;
								else
										moveDir = new Vector3 (0, 0, 1);
						}
						isCalculatingRandomMovement = false;
				}
		}

		void LookAtTarget ()
		{
				Vector3 newRotation = Quaternion.LookRotation (target.position - transform.position).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Euler (newRotation);
		}
		
		void SetInitTracer (bool init)
		{
				if (init && !manaKnightTracer && !chaseMode && !attackPlayer) {
						manaKnightTracer = true;
						stun = true;
						timeAtStartTracer = Time.time;
						timeAtStartStun = Time.time;
						stunShield.active = true;
						walkSource.Stop ();
						stunSource.Play ();
						
						weakPointShield.active = true;
						manaKnight.animation.Play ("initDefense");
				}/* else
						manaKnightTracer = false;
						*/
		}
	
		void ApplyDamage (float dmg)
		{
				if (health > 0 && stun) { // if enemy still alive (don't kick a dead dog!)
						health = health - dmg; // apply the damage...
			
						moanSource.Play ();
						manaKnight.animation.Play ("hit");
			
						// <- enemy can emit some sound here with audio.Play();
						if (health <= 0) { // if health has gone...
								//		Debug.Log ("Mana Knight Killed!");
								if (this.tag == "ManaKnightWithKey") {
										Debug.Log ("KEY");
										GameObject key = GameObject.FindGameObjectWithTag ("Key");
										key.transform.position = transform.position;
										key.GetComponent<MeshRenderer> ().enabled = true;
								}
								explosion.active = true;
								Instantiate (explosion, transform.position, transform.rotation);
								isDead = true;
								timeAtStartDead = Time.time;
								manaKnight.animation.Play ("death");
						}
				}
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
