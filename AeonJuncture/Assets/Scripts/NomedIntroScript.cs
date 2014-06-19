using UnityEngine;
using System.Collections;

public class NomedIntroScript : MonoBehaviour
{
		public bool startChallenge;
		public Texture textbox;
		GUIContent textContent;
		public GUIStyle style;
		float x, y, width, height;
		private int text = 0;
	
		public Vector3 target;
		public Transform kala;
	
		public float speed;
		public bool rotate;
		private bool playingSound;
	
		AudioClip laughClip;
		AudioSource laughSource;
		
		bool laughing;
		
		public GameObject nomedModel;
	
		void Awake ()
		{
				laughClip = Resources.Load ("nomed_laugh") as AudioClip;
				laughSource = AddAudio (laughClip, false, false, 1.0f);
		}
	
		// Use this for initialization
		void Start ()
		{
				GameObject player = GameObject.FindGameObjectWithTag ("Player");
				kala = player.transform;
				
				target = new Vector3 (0, 180f, 0);
				rotate = false;
				playingSound = false;
				
				laughing = false;
				startChallenge = false;
				textContent = new GUIContent ();
				text = 0;
				textContent.text = "\tSo... do you want to know what happened?\n\tIf you are able to defeat me, then truth will be told.\n\tWalk to the blue marker when ready to start the challenge...";
				textContent.image = textbox;
		
				// Text size
				style.fontSize = 25;
				style.fixedHeight = (Screen.height / 100f) * 20f;
				//	style.fixedWidth = Screen.width;
				style.normal.textColor = Color.white;
				style.normal.background = MakeTex (Screen.width, Screen.height, new Color (0f, 0f, 0f, 0.5f), new RectOffset (), Color.black);
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				float distance = Vector3.Distance (kala.transform.position, transform.position);
				if (distance <= 1.5f) {
						if (NomedTauntTrigger.shallNotPass) {
								NomedTauntTrigger.kill = true;
						}
								
						startChallenge = true;
						laughing = false;
				} else {
						startChallenge = false;
						laughing = true;
				}
		
				if (!laughing) {
						nomedModel.animation.Play ("IdleNomed");
				} else if (!startChallenge) {				
						if (!laughSource.isPlaying && !nomedModel.animation.IsPlaying ("TauntNomed")) {
								if (nomedModel.animation.IsPlaying ("IdleNomed")) {
										nomedModel.animation.Stop ("IdleNomed");
								}
								laughSource.Play ();
								nomedModel.animation.Play ("TauntNomed");
						}
				}

				
				if (rotate) {
						if (!playingSound) {
								playingSound = true;
								laughing = true;
						}

						transform.RotateAround (transform.position, Vector3.up, Time.deltaTime * 120f);
						
						if (transform.rotation.eulerAngles.y >= 180.0f) {
								rotate = false;
						}
				}
		}
		void OnGUI ()
		{
				// GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), textCoiso, ScaleMode.ScaleToFit, true, 10.0f);
		
				// Make a background box
				if (startChallenge) {
						GUI.Box (new Rect (x, Screen.height - (Screen.height / 100f) * 20f, Screen.width, Screen.height), textContent, style);
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
	
		void StartTaunting (bool b)
		{
				rotate = b;
		}
		
		private Texture2D MakeTex (int width, int height, Color textureColor, RectOffset border, Color bordercolor)
		{
				int widthInner = width;
				width += border.left;
				width += border.right;
		
				Color[] pix = new Color[ width * (height + border.top + border.bottom)];
				for (int i = 0; i < pix.Length; i++) {
						if (i < (border.bottom * width))
								pix [i] = bordercolor;
						else if (i >= ((border.bottom * width) + (height * width)))  //Border To
								pix [i] = bordercolor;
						else { //Center of Texture
								if ((i % width) < border.left) // Border left
										pix [i] = bordercolor;
								else if ((i % width) >= (border.left + widthInner)) //Border right
										pix [i] = bordercolor;
								else
										pix [i] = textureColor;    //Color texture
						}
				}   
		
				Texture2D result = new Texture2D (width, height + border.top + border.bottom);
		
				result.SetPixels (pix);      		
				result.Apply ();	
		
				return result;
		}
}
