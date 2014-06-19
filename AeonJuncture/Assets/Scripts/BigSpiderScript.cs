using UnityEngine;
using System.Collections;

public class BigSpiderScript : MonoBehaviour
{
		public float health;
		public Transform target, targetDoor;
	
		public float rayRange; 
		float damage = 3;
		bool isAttacking, attackSent, tauntIsPlaying;
		float attackStartPlaying, delayAnimation;
		
	
		float tauntRate, lastTaunt;
	
		// Audio
		AudioClip laughClip, deathClip, hitClip;
		AudioSource laughSource, deathSource, hitSource;
		
		void Awake ()
		{
				laughClip = Resources.Load ("big_spider_laugh") as AudioClip;
				laughSource = AddAudio (laughClip, false, false, 0.9f);
		
				deathClip = Resources.Load ("big_spider_death") as AudioClip;
				deathSource = AddAudio (deathClip, false, false, 1.0f);
				
				hitClip = Resources.Load ("big_spider_hit") as AudioClip;
				hitSource = AddAudio (hitClip, false, false, 1.0f);
		}

		// Use this for initialization
		void Start ()
		{
				GameObject goDoor = GameObject.FindGameObjectWithTag ("WebDoor");
				targetDoor = goDoor.transform;
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
				
				rayRange = 1.5f;
				damage = 3.0f;
				isAttacking = false;
				attackSent = false;
				delayAnimation = 0.4f;
				
				tauntRate = 6.0f;
				lastTaunt = Time.time;
				tauntIsPlaying = false;
				animation ["taunt"].speed = 0.35f;
				animation ["hit2"].speed = 0.40f;
				animation ["death1"].speed = 0.4f;
				
				health = 100.0f;
		}
	
		// Update is called once per frame
		void Update ()
		{
				//	Debug.DrawLine (target.position, transform.position, Color.red);
		
				if (health > 0) {		
						if (tauntIsPlaying && !animation.IsPlaying ("taunt") && !animation.IsPlaying ("hit2")) {
								tauntIsPlaying = false;
								animation.Play ("idle");
								lastTaunt = Time.time;
						} else if (!tauntIsPlaying && Time.time > lastTaunt + tauntRate) { // Taunt time
								animation.Play ("taunt");
								laughSource.Play ();
								tauntIsPlaying = true;
						} 
		
						if (!tauntIsPlaying && !animation.IsPlaying ("taunt") && !animation.IsPlaying ("hit2")) { // not taunting behaviour
								float distance = Vector3.Distance (target.transform.position, transform.position);
		
								LookAtTarget ();
								if (distance >= 6) {
										animation.Play ("idle");
								} else if (distance < 6 && distance >= 1.8) {
						
										RaycastHit hit;
										if (transform.position.x != target.position.x)
												animation.Play ("walk");
										else
												animation.Play ("idle");
								
										transform.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);		
										//	Vector3 position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
										//	transform.Translate (position * Time.deltaTime);				
								} else if (distance <= 1.8) {
										if (!isAttacking) {
												isAttacking = true;
												attackStartPlaying = Time.time;
												animation.Play ("attack1");
										}
										//target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
								}
				
								if (!attackSent && isAttacking && Time.time > attackStartPlaying + delayAnimation) {
										attackSent = true;
										target.transform.SendMessage ("ApplyBumpHit", transform.TransformDirection (Vector3.forward), SendMessageOptions.DontRequireReceiver);
										target.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
								}
				
								if (!animation.IsPlaying ("attack1") && isAttacking) {
										isAttacking = false;
										attackSent = false;
								}
						}
				}
		}
		
		void ApplyDamage (float dmg)
		{
				if (health > 0 && tauntIsPlaying && animation.IsPlaying ("taunt")) { // if enemy still alive (don't kick a dead dog!)
						health = health - dmg; // apply the damage...
						
						animation.Stop ("taunt");
						animation.Play ("hit2");
						hitSource.Play ();
			
						// <- enemy can emit some sound here with audio.Play();
						if (health <= 0) { // if health has gone...
								//	Debug.Log ("Big Spider Killed!");

								animation.Play ("death1");
								deathSource.Play ();
								targetDoor.transform.SendMessage ("StartOpening", true, SendMessageOptions.DontRequireReceiver);
				
						}
				}
		}
		
		void LookAtTarget ()
		{
				Vector3 newRotation = Quaternion.LookRotation (target.position - transform.position).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Euler (newRotation);
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
