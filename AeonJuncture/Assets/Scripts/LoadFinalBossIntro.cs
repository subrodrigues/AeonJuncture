using UnityEngine;
using System.Collections;

public class LoadFinalBossIntro : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player") {
						Application.LoadLevel ("Final_Boss_Intro");
			
				}
		
		}
}
