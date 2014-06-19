using UnityEngine;
using System.Collections;

public class CoinScriptMultiplayer : MonoBehaviour
{

		Transform target;
	
		// Use this for initialization
		void Start ()
		{
				GameObject go = GameObject.FindGameObjectWithTag ("MainCamera");
				target = go.transform;
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Ball") {
						gameObject.active = false;
			
						if (this.transform.tag == "CoinP2")
								target.transform.SendMessage ("P2CoinTrigger", true, SendMessageOptions.DontRequireReceiver);
						else if (this.transform.tag == "CoinP1")
								target.transform.SendMessage ("P1CoinTrigger", true, SendMessageOptions.DontRequireReceiver);
				}
		}
}
