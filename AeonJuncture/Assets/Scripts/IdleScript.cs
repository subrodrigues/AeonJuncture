﻿using UnityEngine;
using System.Collections;

public class IdleScript : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!transform.animation.IsPlaying ("idle"))
						transform.animation.Play ("idle");
		}
}
