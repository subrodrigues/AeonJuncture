using UnityEngine;
using System.Collections;

public class CameraPongMultiplayer : MonoBehaviour
{
	// GUI
	public Texture textbox;
	GUIContent textContent;
	public GUIStyle style;
	float x, y, width, height;
	private int text = 0;
	public float MAX_TIME_TALK, timeAtStartAnimation, MAX_TIME_END, timeEndStart;
	
	// How fast the camera moves
	float moveSpeed = 2.5f;
	
	float damage;
	
	public static float scoreP1, scoreP2;
	
	// Audio
	AudioClip keyClip;
	AudioSource keySource;
	
	Transform targetP1, targetP2;
	
	GameObject[] coinsP1;
	GameObject[] coinsP2;
	
	public GameObject p1Won, p2Won, p1, p2;
	
	void Awake ()
	{
		keyClip = Resources.Load ("get_key") as AudioClip;
		keySource = AddAudio (keyClip, false, false, 1.0f);
		
		coinsP1 = GameObject.FindGameObjectsWithTag ("CoinP1");
		coinsP2 = GameObject.FindGameObjectsWithTag ("CoinP2");
	}
	
	// Use this for initialization
	void Start ()
	{
		
		GameObject go = GameObject.FindGameObjectWithTag ("Player1");
		targetP1 = go.transform;
		GameObject go2 = GameObject.FindGameObjectWithTag ("Player2");
		targetP2 = go2.transform;
		
		this.transform.position = targetP1.position;
		Vector3 playerPos = new Vector3 (this.transform.position.x - 3.65f, this.transform.position.y + 11f, this.transform.position.z);
		transform.position = playerPos;
		transform.localEulerAngles = new Vector3 (90.0f, 0, 0);
		
		scoreP1 = 6.0f;
		scoreP2 = 6.0f;
		
		damage = 1.7f;
		
		MAX_TIME_END = 6.0f;
		

	}

	
	bool MaxEndTimeReached ()
	{
		return (Time.time - timeEndStart) >= MAX_TIME_END; 
	}
	
	// Update is called once per frame
	void Update ()
	{	
	
		/*	if (scoreNomed == 0 && showMsg && MaxEndTimeReached ()) {
			Application.LoadLevel ("End_Story");
		}

		 if (GAMEOVER) {
			timeEndStart = Time.time;
			textContent.text = "\tKala...\n\tI AM YOU!";
			GameObject.FindGameObjectWithTag ("Ball").SendMessage ("StopBall", true, SendMessageOptions.DontRequireReceiver);
			
		}
		*/	
	}
	
	void P2CoinTrigger (bool b)
	{
		if (scoreP2 > 0.0f) {
			scoreP2--;
			targetP2.transform.SendMessage ("ApplyDamagePongP2", damage, SendMessageOptions.DontRequireReceiver);
			
		}
		if (scoreP2 == 0.0f) {
			//	GameObject.FindGameObjectWithTag ("Ball").SendMessage ("StopBall", true, SendMessageOptions.DontRequireReceiver);
			ResetGame ();
		}
		
		keySource.Play ();
	}
	
	void P1CoinTrigger (bool b)
	{
		if (scoreP1 > 0.0f) {
			scoreP1--;
			targetP1.transform.SendMessage ("ApplyDamagePongP1", damage, SendMessageOptions.DontRequireReceiver);
		}
				
		if (scoreP1 == 0.0f) {
			//		GameObject.FindGameObjectWithTag ("Ball").SendMessage ("StopBall", true, SendMessageOptions.DontRequireReceiver);
			ResetGame ();
		}
				
		keySource.Play ();
	}
	
	void ResetGame ()
	{
		targetP1.transform.SendMessage ("ResetPlayer1", true, SendMessageOptions.DontRequireReceiver);
		targetP2.transform.SendMessage ("ResetPlayer2", true, SendMessageOptions.DontRequireReceiver);
		
		scoreP1 = 6.0f;
		scoreP2 = 6.0f;
		
		GameObject.FindGameObjectWithTag ("Ball").SendMessage ("ResetPosition", true, SendMessageOptions.DontRequireReceiver);
		for (int i = 0; i < coinsP1.Length; i++) {
			coinsP1 [i].active = true;
			coinsP2 [i].active = true;
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

	
	
	void TurnPlayer1Won (bool b)
	{
		if (b)
			p1Won.active = true;
		else {
			p2Won.active = true;
		}
	}
}
