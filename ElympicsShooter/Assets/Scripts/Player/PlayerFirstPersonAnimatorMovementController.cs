using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerFirstPersonAnimatorMovementController : MonoBehaviour
{
	[SerializeField] private MovementController playerMovementController;

	private readonly int movementSpeedParameterHash = Animator.StringToHash("MovementSpeed");
	private readonly int jumpingTriggerParameterHash = Animator.StringToHash("JumpTrigger");
	private readonly int isGroundedParameterHash = Animator.StringToHash("IsGrounded");

	private Animator firstPersonAnimator = null;

	private void Awake()
	{
		firstPersonAnimator = GetComponent<Animator>();
		playerMovementController.MovementValuesChanged += ProcessMovementValues;
		playerMovementController.PlayerJumped += ProcessJumping;
		playerMovementController.IsGroundedStateUpdate += ProcessIsGroundedStateUpdate;
	}

	private void ProcessIsGroundedStateUpdate(bool isGrounded)
	{
		firstPersonAnimator.SetBool(isGroundedParameterHash, isGrounded);
	}

	private void ProcessJumping()
	{
		firstPersonAnimator.SetTrigger(jumpingTriggerParameterHash);
	}

	private void ProcessMovementValues(Vector3 movementDirection)
	{
		var speed = movementDirection.magnitude;

		firstPersonAnimator.SetFloat(movementSpeedParameterHash, speed);
	}
}