using UnityEngine;
using System.Collections;

public class EndCutscene : MonoBehaviour
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
		
		intro = new string[75];
		intro [0] = "Kala’s mother, Dwayna, was once a powerfull sorceress";
		intro [1] = "in the world of Avery. There she met and married a king ";
		intro [2] = "named Balthazar, she had a very happy life until the king";
		intro [3] = "started to slowly changes into a different man. King Balthazar";
		intro [4] = "was turning into a cold man led by greed and power. ";
		intro [5] = "Balthazar slowly was pushing people away from him - something was";
		intro [6] = "happening but Dwayna had no idea about what was going on.";
		intro [7] = "";
		intro [8] = "";
		intro [9] = "King Balthazar was also changing in appearance. What was once";
		intro [10] = "a tall strong man was turning into a old, crippled and ill hermit.";
		intro [11] = "It was not until too late that Dwayna found that a Greed Succubus";
		intro [12] = "was seducing the king for pure pleasure.";
		intro [13] = "";
		intro [14] = "The Greed Succubus is a creature that seduces men and feeds";
		intro [15] = "with the desire of the powerfull and wealth men.";
		intro [16] = "";
		intro [17] = "";
		intro [18] = "Dwayna was able to break the enchantment, but it was too late";
		intro [19] = "and the King was already too weak. Balthazar collapsed and"; 
		intro [20] = "become bedridden. Dwayna tried her best to help, but it was late...";
		intro [21] = "King Balthazar has been completed consumed by the Greed Succubus.";
		intro [22] = "";
		intro [23] = "";
		intro [24] = "Not too long after Balthazar died Dwayna found that she was pregnant.";
		intro [25] = "And that was it! She decided that Avery was no place for her firstborn."; 
		intro [26] = "Dwayna was completely full of Avery, magic and mana. She decided to get ";
		intro [27] = "all the mana from Avery and contain and lock it in a fragment. ";
		intro [28] = "Then she created a Jinn named Sila and associated Sila with this fragment."; 
		intro [29] = "";
		intro [30] = "Sila is a jinn created to protect the mana in this fragment if something";
		intro [31] = "ever happens to it. Because in the wrong hands this fragment ";
		intro [32] = "provides a source of unbelievable evil.";
		intro [33] = "";
		intro [34] = "";
		intro [35] = "Then Dwayna left Avery to have a peacefull and simple life on Earth.";
		intro [36] = "";
		intro [37] = "";
		intro [38] = "In that same day, at present, a few hours before Kala found himself";
		intro [39] = "lying on the ground. What happened was that Kala finally had";
		intro [40] = "courage to pick the fragment, but it fell from his hands as ";
		intro [41] = "soon as he touched it… Kala sensed a strange stroke going up his";
		intro [42] = "shoulders and it was inevitable.";
		intro [43] = "At that same very moment the mana was released and absorved";
		intro [44] = "by Kala’s body. It was near impossible for this tiny body to";
		intro [45] = "contain all this mana, so Kala went berserk ";
		intro [46] = "and destroyed everything around him…"; 
		intro [47] = "Just after he saw his mothers body lying on the ground something happened… ";
		intro [48] = "";
		intro [49] = "Kala was fighting back and denying all of this.";
		intro [50] = "";
		intro [51] = "It was at this time that Kala divided himself into 2 forms. ";
		intro [52] = "Now a dark form of Kala filled with anger and power emerged!";
		intro [53] = "";
		intro [54] = "This new Dark Kala opened a rift and got into Avery!";
		intro [55] = "";
		intro [56] = "";
		intro [57] = "After waking up and try to leave the house, ";
		intro [58] = "Kala entered that very same rift closing it.";
		intro [59] = "";
		intro [60] = "";
		intro [61] = "Not too long after this events Sila was also summoned";
		intro [62] = "and appeared to led Kala into this journey.";
		intro [63] = "";
		intro [64] = "";
		intro [65] = "Since time flows different in Avery, Dark Kala was already";
		intro [66] = "an adult when Kala arrived at Avery.";
		intro [67] = "";
		intro [68] = "";
		intro [69] = "Dark Kala (aka Dark Lord Nomed) used this fragment to build ";
		intro [70] = "a sword and turned himself into an eremite haunted";
		intro [71] = "by the ghosts of the past.";
		intro [72] = "";
		intro [73] = "";
		intro [74] = "THE END";
		
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
			float roff = (intro.Length * 8) + (i * 25 + off);
			float alph = Mathf.Sin ((roff / Screen.height) * 180 * Mathf.Deg2Rad);
			style.normal.textColor = new Color (1, 1, 1, alph);
			textContent.text = intro [i];
			GUI.Box (new Rect (Screen.width / 4, roff, Screen.width / 2, Screen.height), textContent, style);
			//		GUI.Label (new Rect (Screen.width / 3, roff, Screen.width / 2, 20), intro [i]);
			style.normal.textColor = new Color (1, 1, 1, 1);
		}
	}
}
