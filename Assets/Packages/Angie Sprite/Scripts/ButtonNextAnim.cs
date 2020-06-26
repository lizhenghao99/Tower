
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AppAdvisory.AngieSprite
{
	public class ButtonNextAnim : MonoBehaviour 
	{
		public Animator anim;
		public static Action actualAction = Action.Idle;
		int actualAnim = (int)Action.Idle;
		public Text actText;

		void Start()
		{
			actText.text = actualAction.ToString();
			PlayAnimation();
		}

		public void Reset()
		{
			actualAnim = 1;
			NextAnimation ();
		}


		public void NextAnimation()
		{ 

			switch(((int)actualAnim))
			{

			case 0:
				actualAction = Action.Idle;
				actualAnim = (int)actualAction;
				break;
			case 1:
				actualAction = Action.Walk;
				actualAnim = (int)actualAction;
				break;
			case 2:
				actualAction = Action.Run;
				actualAnim = (int)actualAction;
				break;

				//case 3:
				//	actualAction = Action.Melee;
				//	actualAnim = (int)actualAction;
				//	break;
			case 3:
				actualAction = Action.Shoot;
				actualAnim = (int)actualAction;
				break;
				//case 5:
				//	actualAction = Action.Jump;
				//	actualAnim = (int)actualAction;
				//	break;
				//case 6:
				//	actualAction = Action.SpinAttack;
				//	actualAnim = (int)actualAction;
				//	break;
			case 4:
				actualAction = Action.Flinch;
				actualAnim = (int)actualAction;
				break;
			case 5:
				actualAction = Action.Death;
				actualAnim = (int)actualAction;
				break;                                     
			default:
				actualAction = Action.Idle;
				actualAnim = (int)actualAction;
				break;
			}

			actText.text = actualAction.ToString();
			anim.Play(actualAction.ToString());
		}

		public void Next()
		{
			Debug.Log("Next");
			actualAnim++;
			NextAnimation ();
		}

		public void Back()
		{
			Debug.Log("Back");
			actualAnim--;
			NextAnimation ();
		}

		public void PlayAnimation()
		{
			anim.Play(actualAction.ToString());
		}

		public IEnumerator BackToIdle() 
		{
			yield return new WaitForSeconds(.7f);
			anim.Play(Action.Idle.ToString());
		}
	}
}