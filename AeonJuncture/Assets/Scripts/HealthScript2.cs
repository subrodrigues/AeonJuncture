using UnityEngine;
using System.Collections;

public class HealthScript2 : MonoBehaviour
{
	
		public GUIStyle progress_empty;
		public GUIStyle progress_full;
	
		//current progress
		public float barDisplay;
	
		Vector2 pos = new Vector2 (10.0f, 50.0f);
		Vector2 size = new Vector2 (250.0f, 50.0f);
	
		public Texture2D emptyTex;
		public Texture2D fullTex;
		
		void Start ()
		{
				useGUILayout = false;
		}
	
		void OnGUI ()
		{
				//draw the background:
				GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y), emptyTex, progress_empty);
		
				GUI.Box (new Rect (pos.x, pos.y, size.x, size.y), fullTex, progress_full);
		
				//draw the filled-in part:
				GUI.BeginGroup (new Rect (0, 0, (size.x * barDisplay) - 30.0f, size.y));
		
				//	Debug.Log (size.x * barDisplay);
				GUI.Box (new Rect (0, 0, size.x, size.y), fullTex, progress_full);
		
				GUI.EndGroup ();
				GUI.EndGroup ();
		}
	
		void Update ()
		{
				//the player's health
				barDisplay = PlayerController2.health / PlayerController2.maxHealth;
				//		Debug.Log ("DISPLAY: " + barDisplay);
		}
	
}
