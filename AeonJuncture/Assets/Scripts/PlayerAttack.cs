using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
		public bool isAttacking;
		private float fingerStartTime;
		private Vector2 fingerStartPos;
	
		private bool isSwipe;
		private float minSwipeDist;
		private float maxSwipeTime;
		
		private float MAX_TIME_ANIM;
		private float timeAtStartOfAnimation;
		private bool isAttackAnimationPlaying;
		
		bool MaxAnimationTimeReached ()
		{
				return (Time.time - timeAtStartOfAnimation) >= MAX_TIME_ANIM; 
		}
	
		// Use this for initialization
		void Start ()
		{
				fingerStartTime = 0.0f;
				fingerStartPos = Vector2.zero;
				isSwipe = false;
				minSwipeDist = 50.0f;
				maxSwipeTime = 0.5f;
				
				isAttacking = false;
				MAX_TIME_ANIM = 0.5f;
				timeAtStartOfAnimation = 0.0f;
				isAttackAnimationPlaying = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (isAttacking && !MaxAnimationTimeReached ()) {
						if (!isAttackAnimationPlaying) {
								animation.Play ("run");
								isAttackAnimationPlaying = true;
						}
				} else {
						animation.Play ("idle");
						isAttackAnimationPlaying = false;
						isAttacking = false;
						
						if (Input.touchCount > 0) {
								for (int i = 0; i < Input.touchCount; i++) {
										Touch touch = Input.touches [i];
										switch (touch.phase) {
										case TouchPhase.Began:
										/* this is a new touch */
												isSwipe = true;
												fingerStartTime = Time.time;
												fingerStartPos = touch.position;
												break;
					
										case TouchPhase.Canceled:
										/* The touch is being canceled */
												isSwipe = false;
												break;
					
										case TouchPhase.Ended:
												float gestureTime = Time.time - fingerStartTime;
												float gestureDist = (touch.position - fingerStartPos).magnitude;
						
												if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
														Vector2 direction = touch.position - fingerStartPos;
														Vector2 swipeType = Vector2.zero;
						
														if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
																// the swipe is horizontal:
																//	swipeType = Vector2.right * Mathf.Sign (direction.x);
																//		if (!isAttacking) {
																isAttacking = true;
																timeAtStartOfAnimation = Time.time;
																//		animation.Play ("run");
																//		}
														} else {
																// the swipe is vertical:
																//	swipeType = Vector2.up * Mathf.Sign (direction.y);
																//		if (!isAttacking) {
																isAttacking = true;
																timeAtStartOfAnimation = Time.time;
																//		animation.Play ("run");
																//		}
														}
						
														//	controller.Swipe (swipeType);
												}
						
												break;
										}
								}
						}
				}
		}
}
