using UnityEngine;
using System.Collections;

public class CameraPong : MonoBehaviour
{
	// GUI
	public static bool showMsg;
	public Texture textbox;
	GUIContent textContent;
	public GUIStyle style;
	float x, y, width, height;
	private int text = 0;
	public float MAX_TIME_TALK, timeAtStartAnimation, MAX_TIME_END, timeEndStart;
	
	// How fast the camera moves
	float moveSpeed = 2.5f;
		
	float damage;
		
	float scoreKala, scoreNomed;
		
	// Audio
	AudioClip keyClip;
	AudioSource keySource;
		
	Transform target;
		
	GameObject[] coins;
	GameObject[] coinsNomed;
		
	int msgCount = 0;
		
	public GameObject gameOver;
		
	void Awake ()
	{
		keyClip = Resources.Load ("get_key") as AudioClip;
		keySource = AddAudio (keyClip, false, false, 1.0f);
				
		coins = GameObject.FindGameObjectsWithTag ("Coin");
		coinsNomed = GameObject.FindGameObjectsWithTag ("CoinNomed");
	}
	
	// Use this for initialization
	void Start ()
	{
		
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		target = go.transform;
		
		this.transform.position = target.position;
		Vector3 playerPos = new Vector3 (this.transform.position.x - 3.65f, this.transform.position.y + 11f, this.transform.position.z);
		transform.position = playerPos;
		transform.localEulerAngles = new Vector3 (90.0f, 0, 0);
				
		scoreKala = 6.0f;
		scoreNomed = 6.0f;
				
		damage = 1.7f;
		msgCount = 0;
				
				
		textContent = new GUIContent ();
		textContent.image = textbox;
		text = 0;
		textContent.text = "\tI killed your mother!";
		
		MAX_TIME_TALK = 3.5f;
				
		MAX_TIME_END = 6.0f;
				
		// Text size
		style.fontSize = 25;
		style.fixedHeight = (Screen.height / 100f) * 20f;
		//	style.fixedWidth = Screen.width;
		style.normal.textColor = Color.white;
		style.normal.background = MakeTex (Screen.width, Screen.height, new Color (0f, 0f, 0f, 0.5f), new RectOffset (), Color.black);
		
	}
		
	bool MaxTalkTimeReached ()
	{
		return (Time.time - timeAtStartAnimation) >= MAX_TIME_TALK; 
	}
		
	bool MaxEndTimeReached ()
	{
		return (Time.time - timeEndStart) >= MAX_TIME_END; 
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if (scoreNomed > 0 && showMsg && MaxTalkTimeReached ()) {
			showMsg = false;
		}
		if (scoreNomed == 0 && showMsg && MaxEndTimeReached ()) {
			Application.LoadLevel ("End_Story");
		}
				
		if (msgCount == 0 && !showMsg && scoreNomed <= 4 && scoreNomed > 2) {
			msgCount++;
			showMsg = true;
			timeAtStartAnimation = Time.time;
			textContent.text = "\tI killed your mother!";
		} else if (msgCount == 1 && !showMsg && scoreNomed <= 2 && scoreNomed > 0) {
			msgCount++;	
			showMsg = true;
			timeAtStartAnimation = Time.time;
			textContent.text = "\tI killed... our mother!";
		} else if (scoreNomed == 0 && !showMsg) {
			showMsg = true;
			timeEndStart = Time.time;
			textContent.text = "\tKala...\n\tI AM YOU!";
			GameObject.FindGameObjectWithTag ("Ball").SendMessage ("StopBall", true, SendMessageOptions.DontRequireReceiver);
			
		}
		
	}
		
	void NomedCoinTrigger (bool b)
	{
		if (scoreNomed > 0.0f) {
			scoreNomed--;
		}
						
		keySource.Play ();
	}
		
	void CoinTrigger (bool b)
	{
		if (scoreKala > 0.0f && scoreNomed != 0) {
			scoreKala--;
			target.transform.SendMessage ("ApplyDamagePong", damage, SendMessageOptions.DontRequireReceiver);
		}
						
		keySource.Play ();
	}
		
	void ResetGame (bool b)
	{
		scoreKala = 6.0f;
		scoreNomed = 6.0f;
		msgCount = 0;
				
		for (int i = 0; i < coins.Length; i++) {
			coins [i].active = true;
			coinsNomed [i].active = true;
		}
				
		GameObject.FindGameObjectWithTag ("Ball").SendMessage ("ResetPosition", true, SendMessageOptions.DontRequireReceiver);
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
		
	void OnGUI ()
	{
		// Make a background box
		if (showMsg) {
			GUI.Box (new Rect (x, Screen.height - (Screen.height / 100f) * 20f, Screen.width, Screen.height), textContent, style);
		}
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
		
	
	void TurnGameOver (bool b)
	{
		gameOver.active = true;
	}
}
