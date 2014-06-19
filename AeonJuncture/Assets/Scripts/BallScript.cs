using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
		private Vector3 direction;
		private float speed;
		public Vector3 Velocity;
		public Vector3 initialPosition;
			
		public Transform shield, shieldNomed;
		
		AudioClip bounceClip;
		AudioSource bounceSource;
		
		void Awake ()
		{
				bounceClip = Resources.Load ("ball_bounce") as AudioClip;
				bounceSource = AddAudio (bounceClip, false, false, 0.8f);
		}
	
		// Use this for initialization
		void Start ()
		{
				this.direction = new Vector3 (1.0f, 0.0f, 1.0f).normalized;
				this.speed = 0.1f;
				
				initialPosition = this.transform.position;
				// rigidbody.AddForce (Vector3.forward * 10);
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (this.transform.position.y != 0.37f)
						this.transform.position = new Vector3 (this.transform.position.x, 0.37f, this.transform.position.z);
										
		}
	
		void OnTriggerEnter (Collider collision)
		{
		
				if (collision.gameObject.tag == "Shield") {
						bounceSource.Play ();
						// Acerta no meio
						if (shield.TransformDirection (Vector3.forward).z >= -0.3f && shield.TransformDirection (Vector3.forward).z <= 0.3f) {
								rigidbody.velocity = -Vector3.right * 10;
						} else if (shield.TransformDirection (Vector3.forward).z < -0.3f) {
								rigidbody.velocity = new Vector3 (-6, 0, -4f);
						} else if (shield.TransformDirection (Vector3.forward).z > 0.3f) {
								rigidbody.velocity = new Vector3 (-6, 0, 4f);
						}
				}
		
				if (collision.gameObject.tag == "NomedShield") {
						bounceSource.Play ();
						// Acerta no meio
						if (shield.TransformDirection (Vector3.forward).z >= -0.3f && shield.TransformDirection (Vector3.forward).z <= 0.3f) {
								rigidbody.velocity = Vector3.right * 10;
						} else if (shield.TransformDirection (Vector3.forward).z < -0.3f) {
								rigidbody.velocity = new Vector3 (6, 0, 4f);
						} else if (shield.TransformDirection (Vector3.forward).z > 0.3f) {
								rigidbody.velocity = new Vector3 (6, 0, -4f);
						}
				}
				
				if (collision.gameObject.tag == "Wall") {
						bounceSource.Play ();
				}
		}
		
		public void ResetPosition (bool b)
		{
				this.direction = new Vector3 (1.0f, 0.0f, 1.0f).normalized;
				this.speed = 0.1f;
				this.rigidbody.velocity = Vector3.zero;
				this.transform.position = initialPosition;
		}
		
		public void StopBall (bool b)
		{
				//this.speed = 0.1f;
				this.GetComponent<SphereCollider> ().enabled = false;
				this.rigidbody.velocity = Vector3.zero;
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
}
