using UnityEngine;
using System.Collections;

public class PotionScript : MonoBehaviour
{
		Transform target;
		public float potionHeal;	
		bool used;
		
		AudioClip potionClip;
		AudioSource potionSource;
		
		public GameObject potionModel;
		
		void Awake ()
		{
				potionClip = Resources.Load ("potion") as AudioClip;
				potionSource = AddAudio (potionClip, false, false, 1.0f);
		}
		
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				target = go.transform;
				used = false;
				
				potionHeal = 3.0f;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (used && !potionSource.isPlaying)
						gameObject.active = false;
		}
		
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player" && !used) {
						used = true;
						potionModel.GetComponent<MeshRenderer> ().enabled = false;
						target.transform.SendMessage ("ApplyPotion", potionHeal, SendMessageOptions.DontRequireReceiver);
						potionSource.Play ();
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
