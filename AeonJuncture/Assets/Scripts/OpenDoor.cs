using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
		public Vector3 target;
		public float speed;
		public bool open;
		private bool playingSound;
		
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
				target = new Vector3 (transform.position.x, 2.0f, transform.position.z);
				speed = 0.3f;
				open = false;
				playingSound = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (open) {
						if (!playingSound) {
								doorSource.Play ();
								playingSound = true;
						}
						float step = speed * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, target, step);
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
		
		void StartOpening (bool b)
		{
				open = b;
		}
}
