using UnityEngine;
using System.Collections;

public class NomedScript : MonoBehaviour
{
		public GameObject nomedModel, ball;
		public float initialYPosition;
		
		//Speed of the enemy
		public float movementSpeed;
		//Smoothing/damping of movement
		float damping;
		
		float changeThresholdRatio, lastThresholdChange, changeXMovRatio, xMovementSpeed, lastChangeXMovement;
		public float thresholdRatio;
	
		// Use this for initialization
		void Start ()
		{
				initialYPosition = transform.position.y;
				
				movementSpeed = 9f;
				damping = 0.5f;
				
				changeThresholdRatio = 1.0f;
				lastThresholdChange = Time.time;
				thresholdRatio = Random.value * 1.0f;
				
				changeXMovRatio = 2.5f;
				xMovementSpeed = (Random.value * 2.0f) + 0.8f; 
				lastChangeXMovement = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				if (!nomedModel.animation.IsPlaying ("TauntNomed"))
						nomedModel.animation.Play ("TauntNomed");
			
				if (this.transform.position.y != initialYPosition) {
						this.transform.position = new Vector3 (this.transform.position.x, initialYPosition, this.transform.position.z);
				}
				
				if (Time.time > lastThresholdChange + changeThresholdRatio) {
						thresholdRatio = Random.value * 1.0f;
						lastThresholdChange = Time.time;
				}
				
				if (Time.time > lastChangeXMovement + changeXMovRatio) {
						xMovementSpeed = (Random.value * 2.0f) + 0.8f; 
						lastChangeXMovement = Time.time;
				}
				
				// If ball is above enemy, move up
				if (ball.transform.position.z > transform.position.z + damping + thresholdRatio) {
						transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
				}
				// If ball is below enemy, move down
				else if (ball.transform.position.z < transform.position.z - damping - thresholdRatio) {
						transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
				}
				//Upper and lower bounds of movement
				if (transform.position.z <= -10f) {
						transform.position = new Vector3 (transform.position.x, 0.0f, -10f);
				} else if (transform.position.z >= 10f) {
						transform.position = new Vector3 (transform.position.x, 0.0f, 10f);
				}
				
				// Move in X
				if (ball.transform.position.x > 1.0f && this.transform.position.x < -0.8f) { // Move forward
						transform.Translate (Vector3.forward * xMovementSpeed * Time.deltaTime);
				} else if (ball.transform.position.x <= 1.5f && transform.position.x > -3.7f) { // Move backward
						transform.Translate (Vector3.back * xMovementSpeed * Time.deltaTime);
				}
		}
}
