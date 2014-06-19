using UnityEngine;
using System.Collections;

public class PlayGameScript : MonoBehaviour
{
	public bool isQuitButton, loadDungeon1, loadDungeon2, loadFinalBoss, levelSelection, backToMain, multiplayer;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseEnter ()
	{
		renderer.material.color = Color.green;
	}
	void OnMouseExit ()
	{
		renderer.material.color = Color.white;
	}
		
	void OnMouseUp ()
	{
		if (isQuitButton) {
			Application.Quit ();
		} else if (loadDungeon1) {
			Application.LoadLevel ("Dungeon1_Quicksand");
		} else if (loadDungeon2) {
			Application.LoadLevel ("Dungeon2_ManaSwamp");
		} else if (loadFinalBoss) {
			Application.LoadLevel ("Final_Boss_Intro");
		} else if (levelSelection) {
			CameraChangeScript.levelSelection = true;
		} else if (backToMain) {
			CameraChangeScript.backToMain = true;
		} else if (multiplayer) {
			Application.LoadLevel ("Final_Challenge_Multiplayer");
		} else {
			Application.LoadLevel ("Intro_Story");
		}
	}
}
