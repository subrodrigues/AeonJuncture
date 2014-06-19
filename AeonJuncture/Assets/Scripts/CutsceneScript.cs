using UnityEngine;
using System.Collections;

public class CutsceneScript : MonoBehaviour
{
		public static bool shallNotPass;
		public Texture textbox;
		GUIContent textContent;
		public GUIStyle style;
		float x, y, width, height;
		private int text = 0;		
		
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				textContent = new GUIContent ();
				text = 0;
				x = 10f;
				y = 10f;
				textContent.image = textbox;
				
		
				// Text size
				//	style.fontSize = 25;
				//	style.fixedHeight = (Screen.height / 100f) * 20f;
				//	style.fixedWidth = Screen.width;
				//	style.normal.textColor = Color.white;
				//	style.normal.background = MakeTex (Screen.width, Screen.height, new Color (0f, 0f, 0f, 0.5f), new RectOffset (), Color.black);
		}
	
		void OnGUI ()
		{
				// Make a background box
				if (GUI.Button (new Rect (x, y, Screen.width / 10, Screen.height / 6), textContent)) {
						if (this.tag == "IntroductionStory")
								Application.LoadLevel ("Dungeon1_Quicksand");
						else if (this.tag == "EndStory")
								Application.LoadLevel ("Main_Menu");		
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
		
		void OnMouseDown ()
		{
				Debug.Log ("PRESSED");
		}
}
