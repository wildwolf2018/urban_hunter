using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {
	public int crouchShootState;
	public int crouchPunchState;
	public int shootState;
	public int shootDigonalState;
	public int shootUpState;

	void Awake()
	{
		crouchShootState = Animator.StringToHash ("BaseLayer.crouch_shoot");
		crouchPunchState = Animator.StringToHash ("BaseLayer.crouch_punch");
		shootState = Animator.StringToHash ("BaseLayer.shoot_horizontal");
		shootDigonalState = Animator.StringToHash ("BaseLayer.shoot_diagonal");
		shootUpState = Animator.StringToHash ("BaseLayer.shoot_up");
	}
}
