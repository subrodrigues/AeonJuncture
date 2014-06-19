using UnityEngine;
using System.Collections;

public class SilaTalking : MonoBehaviour
{
		public Transform kala;
		public bool greetDialog;
		public Texture textbox;
		GUIContent textContent;
		public GUIStyle style;
		float x, y, width, height;
		private int text = 0;
		
		private string initHintText;	
		private bool showInitHint;
		
		//Texture2D textCoiso;
	
		// Use this for initialization
		void Start ()
		{
				GameObject player = GameObject.FindGameObjectWithTag ("Player");
				kala = player.transform;

				//	y = 0;
				//	GUI.skin.textArea = style;
				//	str = GUILayout.TextArea(str);

				greetDialog = false;
				textContent = new GUIContent ();
				text = 0;
				textContent.text = "\toh... hello! Welcome to Avery young fella... \n\tI'm here to help you... maybe?";
				textContent.image = textbox;
				
				showInitHint = false;
				initHintText = "\tby the way... avoid facing the Mana Knights... But if you need my help point them! \n\tAnd if you find yourself stuck in Quicksand... try to \"shake\" things a bit...";
			
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
				if (distance <= 1.2f) {
						greetDialog = true;
				} else {
						greetDialog = false;
				}
		}
	
		void OnGUI ()
		{
				// GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), textCoiso, ScaleMode.ScaleToFit, true, 10.0f);

				// Make a background box
				if (greetDialog) {
						GUI.Box (new Rect (x, Screen.height - (Screen.height / 100f) * 20f, Screen.width, Screen.height), textContent, style);
				}
				if (showInitHint) {
						textContent.text = initHintText;
						GUI.Box (new Rect (x, Screen.height - (Screen.height / 100f) * 20f, Screen.width, Screen.height), textContent, style);
				}
				/*	if (GUI.Button (new Rect (20, 40, 80, 20), "Nothing")) {
		}
	*/
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
		
		void SetInitHint (bool init)
		{
				if (init) {
						showInitHint = true;
				} else
						showInitHint = false;
		}
}