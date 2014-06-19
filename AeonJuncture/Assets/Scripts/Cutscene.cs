using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour
{
	
	public string[] intro;
	public float off;
	public float speed;
		
	GUIContent textContent;
	public GUIStyle style;
		
	void Start ()
	{
		
		// Text size
		style.fontSize = 22;
		style.margin = new RectOffset (2, 2, 2, 2);
		style.fixedHeight = (Screen.height / 100f) * 20f;
		//	style.fixedWidth = Screen.width;
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;
		textContent = new GUIContent ();
				
		intro = new string[31];
		intro [0] = "Our hero, Kala, is a child and lives a simple";
		intro [1] = "and peaceful life in a small village,";
		intro [2] = "called Little Count, in our world, Earth.";
		intro [3] = "Kala lives alone with his mother Dwayna.";
		intro [4] = "Since our hero started forming memories he ";
		intro [5] = "remembers a strange fragment that his mother";
		intro [6] = "protect and preserves with a large ammount of ";
		intro [7] = "passion and dedication. When questioned about it, ";
		intro [8] = "Dwayna always told a story about the spirit, Kala’s";
		intro [9] = " father spirit (Balthazar), that was preserved into ";
		intro [10] = "that fragment. And she always told him that the ";
		intro [11] = "spirit should be kept away from “the wrong hands”...";
		intro [12] = "Kala’s always had a strange sensation about that ";
		intro [13] = "fragment. When approaching the fragment Kala swears ";
		intro [14] = "to be able to hear a cavernous voice saying ";
		intro [15] = "something like: “Take me. Please...”";
		intro [16] = "";
		intro [17] = "";
		intro [18] = "Suddenly one morning, that at first was like the ";
		intro [19] = "others, Kala finds himself lying on the ground. ";
		intro [20] = "When Kala gets up he find that his single mother ";
		intro [21] = "has been slaughtered and the house has been ";
		intro [22] = "completely messed up. When our hero tries to leave";
		intro [23] = "the house he is teleported and stuck in a strange dungeon.";
		intro [24] = "In this very same dungeon he finds a strange, dark creature ";
		intro [25] = "named Sila. Sila is a strange magical being with some";
		intro [26] = "attributes that resembles that of the human females,";
		intro [27] = "but Kala cannot be sure what he/she really his.";
		intro [28] = "";
		intro [29] = "Now our hero needs to solve the mistery.";
		intro [30] = "What happened? Where is he? “Mom?”";

				
		speed = 12;
				
	}
		
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			Application.LoadLevel ("Main_Menu");
		}
	}
	
	public void OnGUI ()
	{
		off -= Time.deltaTime * speed;
		for (int i = 0; i < intro.Length; i++) {
			float roff = (intro.Length * 18) + (i * 25 + off);
			float alph = Mathf.Sin ((roff / Screen.height) * 180 * Mathf.Deg2Rad);
			style.normal.textColor = new Color (1, 1, 1, alph);
			textContent.text = intro [i];
			GUI.Box (new Rect (Screen.width / 4, roff, Screen.width / 2, Screen.height), textContent, style);
			//		GUI.Label (new Rect (Screen.width / 3, roff, Screen.width / 2, 20), intro [i]);
			style.normal.textColor = new Color (1, 1, 1, 1);
		}
	}
}
