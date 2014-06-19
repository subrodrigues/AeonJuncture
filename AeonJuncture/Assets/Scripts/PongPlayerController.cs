using UnityEngine;
using System.Collections;

public class PongPlayerController : MonoBehaviour
{
	// Controlo do Personagem
	CharacterController controller;
	public bool shield, resetPlayer;
	public int life = 3;
	public static float health;
	public static float maxHealth;
		
	// Camera
	public float moveStepCameraSpeed = 15f;
	public float rotateSpeed = 60f;
		
	public GameObject ground, cameraPong, kalaModel;
	public GameObject life1, life2, life3;

	public float initialYPosition, initialXPosition, initialZPosition;
		
	// Game Over
	bool death;
	public float MAX_TIME_GAMEOVER, timeAtStartGameOver;
	
	bool MaxGameOverTimeReached ()
	{
		return (Time.time - timeAtStartGameOver) >= MAX_TIME_GAMEOVER; 
	}
	
	// Use this for initialization
	void Start ()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		controller = (CharacterController)GetComponent (typeof(CharacterController));

		initialYPosition = transform.position.y;
		initialXPosition = transform.position.x;
		initialZPosition = transform.position.z;
		
		shield = true;
		resetPlayer = false;
		life = 3;
		health = 10.0f;
		maxHealth = 10.0f;
				
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
		
		if (this.transform.position.y != initialYPosition) {
			this.transform.position = new Vector3 (this.transform.position.x, initialYPosition, this.transform.position.z);
		}
				
		if (death && MaxGameOverTimeReached ()) {
			Application.LoadLevel ("Main_Menu");
		}
		
		// UpdateMovement ();
		UpdateMovementKeyboard ();
	}
		
	void UpdateMovement ()
	{
		if (!death) {
			if (Input.touchCount > 0) {
				RaycastHit hit;
					
				//	shield = false;
				//	shieldObject.active = false;
					
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
							
			} else { // If not movement, stay idle
				kalaModel.animation.Play ("Idle");
			}
		} else { // If death, stay idle
			if (!kalaModel.animation.IsPlaying ("Idle"))
				kalaModel.animation.Play ("Idle");
		}
	}
		
	void UpdateMovementKeyboard ()
	{
		if (!death) {
			if (Input.GetMouseButton (0)) {
				RaycastHit hit;
			
				//	shield = false;
				//	shieldObject.active = false;
			
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				// Se toca no chao
				if (ground.collider.Raycast (ray, out hit, Mathf.Infinity)) {
					// With isometric camera
					//Vector3 target = new Vector3 (hit.point.x - 1.4f, transform.position.y, hit.point.z - 1.4f);
					// With 3d Camera
					Vector3 target = new Vector3 (hit.point.x, initialYPosition, hit.point.z);
				
					MoveTowardsTarget (target);
				
					FaceTouchedPoint (target);
				}
			
			} else { // If not movement, stay idle
				kalaModel.animation.Play ("Idle");
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
			if (this.transform.position.x >= 0.0f) {
				//If we're further away than .1 unit, move towards the target.
				//The minimum allowable tolerance varies with the speed of the object and the framerate. 
				// 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
				offset = offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized;
				//normalize it and account for movement speed.
				controller.Move (offset * Time.deltaTime);
				//actually move the character.
				kalaModel.animation.Play ("Walk");
			} else if (offset.x > 0) {
				offset = offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized + offset.normalized;
				//normalize it and account for movement speed.
				controller.Move (offset * Time.deltaTime);
				//actually move the character.
				kalaModel.animation.Play ("Walk");
			}
		}
	}
		
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
	
	void ApplyDamagePong (float damage)
	{
		health -= damage;
		if (life == 0 && health <= 0.0f && !death) {
			timeAtStartGameOver = Time.time;
			GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("TurnGameOver", true, SendMessageOptions.DontRequireReceiver);
			death = true;
		}
				
		if (health <= 0.0f) {
			health = 10.0f;
			
			if (life >= 0) {
				life -= 1;
			}
			resetPlayer = true;
			UpdateLives ();
		}
				
				
	}
		
	public void resetPlayerPos ()
	{
		//	restartLives ();
		transform.position = new Vector3 (initialXPosition, initialYPosition, initialZPosition);
		cameraPong.SendMessage ("ResetGame", true, SendMessageOptions.DontRequireReceiver);
	}
}
