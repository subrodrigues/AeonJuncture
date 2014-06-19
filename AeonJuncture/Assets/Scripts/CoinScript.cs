using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour
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
						if (this.transform.tag == "CoinNomed")
								target.transform.SendMessage ("NomedCoinTrigger", true, SendMessageOptions.DontRequireReceiver);
						else if (this.transform.tag == "Coin")
								target.transform.SendMessage ("CoinTrigger", true, SendMessageOptions.DontRequireReceiver);
						gameObject.active = false;
				}
		}
}
