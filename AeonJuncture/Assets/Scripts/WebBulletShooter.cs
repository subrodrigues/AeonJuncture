using UnityEngine;
using System.Collections;

public class WebBulletShooter : MonoBehaviour
{
		float lastFired;
		float fireRate; // In seconds
		float delayAnimation;
		public WebBullet webBullet;
		float giggleRate, lastGiggle;
		public Transform target;
		
		// Audio
		AudioClip hissClip, giggleClip;
		AudioSource hissSource, giggleSource;
	
		void Awake ()
		{
				hissClip = Resources.Load ("spider_hiss") as AudioClip;
				hissSource = AddAudio (hissClip, false, false, 0.5f);
				
				giggleClip = Resources.Load ("spider_giggle") as AudioClip;
				giggleSource = AddAudio (giggleClip, false, false, 0.5f);
		}
		
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
				
				lastFired = 0.0f;
				fireRate = 2.5f + (Random.value * 3.5f); // In seconds
				delayAnimation = 0.2f;
				
				giggleRate = 2.5f + (Random.value * 3.5f);
				
				lastGiggle = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
				//	Debug.DrawLine (target.position, transform.position, Color.red);
				
				float distance = Vector3.Distance (target.transform.position, transform.position);
				if (distance <= 5) {
						if (Time.time > lastFired + fireRate) {
								//fire WebBullet!
								transform.parent.gameObject.animation.Play ("attack2");
								hissSource.Play ();
						}
						if (Time.time > lastFired + fireRate + delayAnimation) {
								lastFired = Time.time;
								fireRate = 2.5f + (Random.value * 3.5f);
								Instantiate (webBullet, transform.position, transform.rotation);
						} else if (!transform.parent.gameObject.animation.isPlaying) {
								transform.parent.gameObject.animation.Play ("idle");
						} else if (Time.time > lastGiggle + giggleRate) {
								lastGiggle = Time.time;
								giggleRate = 2.5f + (Random.value * 3.5f);
						
								giggleSource.Play ();
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
}
