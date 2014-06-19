using UnityEngine;
using System.Collections;

public class BridgeTrigger : MonoBehaviour
{

		GameObject go;
		Transform target;
		float initialPositionY, pressedPositionY;
	
		// Use this for initialization
		void Start ()
		{
				if (this.tag == "BridgeTrigger1") {
						GameObject go = GameObject.FindGameObjectWithTag ("Bridge1");
						target = go.transform;
				}
				if (this.tag == "BridgeTrigger2") {
						GameObject go = GameObject.FindGameObjectWithTag ("Bridge2");
						target = go.transform;
				}
				if (this.tag == "BridgeTrigger3") {
						GameObject go = GameObject.FindGameObjectWithTag ("Bridge3");
						target = go.transform;
				}
				if (this.tag == "BridgeTrigger4") {
						GameObject go = GameObject.FindGameObjectWithTag ("Bridge4");
						target = go.transform;
				}
				if (this.tag == "BridgeTrigger5") {
						GameObject go = GameObject.FindGameObjectWithTag ("Bridge5");
						target = go.transform;
				}
				
				initialPositionY = transform.position.y;
				pressedPositionY = 0.2016709f;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		
		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.gameObject.tag == "Player") {
						this.transform.position = new Vector3 (transform.position.x, pressedPositionY, transform.position.z);
						target.transform.SendMessage ("StartLaunchingBridge", true, SendMessageOptions.DontRequireReceiver);
				}
		}
		
		void InitialTriggerPosition (bool b)
		{
				this.transform.position = new Vector3 (transform.position.x, initialPositionY, transform.position.z);
		}
}
