using UnityEngine;
using System.Collections;

public class BridgeScript : MonoBehaviour
{
		public float standingTime;
		public float finalPosition, initialPosition;
		
		public Vector3 target, initialTarget;
		public float speed, closingSpeed;
		public bool open;
		private bool playingSound;
		float openTime;
	
		AudioClip doorClip;
		AudioSource doorSource;
		
		void Awake ()
		{
				doorClip = Resources.Load ("door_open") as AudioClip;
				doorSource = AddAudio (doorClip, false, false, 1.0f);
		}

		// Use this for initialization
		void Start ()
		{
				if (this.tag == "Bridge1") {
						standingTime = 9.5f;
				}
				if (this.tag == "Bridge2") {
						standingTime = 8.0f;
				}
				if (this.tag == "Bridge3") {
						standingTime = 6.5f;
				}
				if (this.tag == "Bridge4") {
						standingTime = 5.0f;
				}
				if (this.tag == "Bridge5") {
						standingTime = 3.5f;
				}
				
				finalPosition = -4.7f;
				initialPosition = transform.position.y;
				
				target = new Vector3 (transform.position.x, finalPosition, transform.position.z);
				initialTarget = new Vector3 (transform.position.x, initialPosition, transform.position.z);
				speed = 7f;
				closingSpeed = 7f;
				open = false;
				playingSound = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				bool timeEnded = Time.time > openTime + standingTime;
		
				if (open && !timeEnded) {
						if (!playingSound) {
								doorSource.Play ();
								playingSound = true;
						}
						float step = speed * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, target, step);
				}
				if (open && timeEnded) {
						float step = closingSpeed * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, initialTarget, step);
				}
				
				if (transform.position.y == initialPosition) {
						open = false;
						playingSound = false;
						LaunchTriggerInitialPosition ();
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
	
		void StartLaunchingBridge (bool b)
		{
				if (open == false) {
						openTime = Time.time;
						open = b;
				}
		}
		
		void LaunchTriggerInitialPosition ()
		{
				if (this.tag == "Bridge1") {
						GameObject.FindGameObjectWithTag ("BridgeTrigger1").transform.SendMessage ("InitialTriggerPosition", true, SendMessageOptions.DontRequireReceiver);
				}
				if (this.tag == "Bridge2") {
						GameObject.FindGameObjectWithTag ("BridgeTrigger2").transform.SendMessage ("InitialTriggerPosition", true, SendMessageOptions.DontRequireReceiver);
				}
				if (this.tag == "Bridge3") {
						GameObject.FindGameObjectWithTag ("BridgeTrigger3").transform.SendMessage ("InitialTriggerPosition", true, SendMessageOptions.DontRequireReceiver);
				}
				if (this.tag == "Bridge4") {
						GameObject.FindGameObjectWithTag ("BridgeTrigger4").transform.SendMessage ("InitialTriggerPosition", true, SendMessageOptions.DontRequireReceiver);
				}
				if (this.tag == "Bridge5") {
						GameObject.FindGameObjectWithTag ("BridgeTrigger5").transform.SendMessage ("InitialTriggerPosition", true, SendMessageOptions.DontRequireReceiver);
				}
		}
		
}
