﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CollisionFlag
{
	public bool left;
	public bool right;
	public bool above;
	public bool below;

	public void ResetFlags()
	{
		left = right = above = below = false;
	}
}

public class Character : MonoBehaviour
{
	[Header("References")]
	public Transform self;
	public Rigidbody2D rb;
	public CapsuleCollider2D box2D;


	[Header("Movement Stats")]
	public AnimationCurve accelerationCurve;
	[Range(0, 1000)] public float maxSpeed;
	[Range(0, 10)] public float timeToMaxSpeed;
	public AnimationCurve fallCurve;
	[Range(0.05f, 10)] public float timeToMaxFallSpeed = 0.5f;
	public float fallSpeed = 9.81f;

	[Header("Jump Stats")]
	public AnimationCurve jumpCurve;
	[Range(0, 10)] public float jumpDuration;
	[Range(0, 10)] public int nbrOfJumps = 1;

	[Header("Collision Detection")]
	[Range(0, 0.5f)] public float detectionRange = 0.05f;
	public LayerMask collisionMask;

	[Header("States")]
	public bool canMove = true;
	public bool isMoving = false;
	public bool onGround = false;
	public bool wasOnGround = false;
	public bool canJump = true;
	public bool isJumping = false;
	public CollisionFlag collisionFlags;

	private float _fallTimeTracker;
	private float _accelerationTimetracker;
	private float _jumpTimeTracker;
	private int _availableJumps;
	private Vector2 _movementDir;


	protected virtual void Start()
	{

		if (jumpDuration <= 0) Debug.LogWarning("jumpDuration is equal to zero !");
		if (timeToMaxSpeed <= 0) Debug.LogWarning("time to maxSpeed is equal to zero !");
		self = transform;
		rb = GetComponent<Rigidbody2D>();
		collisionFlags.ResetFlags();
		_accelerationTimetracker = 0;
		_jumpTimeTracker = 0;
		_availableJumps = nbrOfJumps;
	}

	protected virtual void Update()
	{
		if (rb.velocity.x == 0) isMoving = false;

		DetectCollision();
		ApplyFallSpeed();
		HandleJump();

		rb.velocity = _movementDir;

		wasOnGround = onGround;
	}


	private void DetectCollision()
	{
		if (rb.velocity.y <= 0)
		{
			collisionFlags.above = false;
			collisionFlags.below = CheckGround();
		}
		else if (rb.velocity.y > 0)
		{
			collisionFlags.below = false;
			collisionFlags.above = ChekRoof();
		}

		onGround = collisionFlags.below;
		if (onGround && !wasOnGround)
		{
			_availableJumps = nbrOfJumps;
			_fallTimeTracker = 0;
		}

		CheckSides();
	}
	private void CheckSides()
	{
		bool detection = Physics2D.BoxCast(self.position + (Vector3.up * box2D.size.y / 2) + Vector3.right * (box2D.size.x / 2) * Mathf.Sign(rb.velocity.x),
			new Vector2(0.01f, box2D.size.y / 2),
			0,
			Vector2.right * Mathf.Sign(rb.velocity.x),
			detectionRange,
			collisionMask);

		if (rb.velocity.x > 0)
		{
			collisionFlags.left = false;
			collisionFlags.right = detection;
		}
		else if (rb.velocity.x < 0)
		{
			collisionFlags.right = false;
			collisionFlags.left = detection;
		}
	}
	private bool ChekRoof()
	{
		return Physics2D.BoxCast(self.position + Vector3.up * box2D.size.y, new Vector2(box2D.size.x / 2.1f, 0.01f), 0, Vector2.up, detectionRange, collisionMask);
	}
	private bool CheckGround()
	{
		return Physics2D.BoxCast(self.position, new Vector2(box2D.size.x / 2.1f, 0.01f), 0, Vector2.down, detectionRange, collisionMask);
	}

	public virtual void MoveHorizontal(float axis)
	{
		if (!canMove) return;
		if (!isMoving)
		{
			isMoving = true;
			_accelerationTimetracker = 0;
		}

		_accelerationTimetracker += Time.deltaTime;
		_movementDir = Vector2.right * axis * maxSpeed *  accelerationCurve.Evaluate(_accelerationTimetracker);
	}
	public virtual void Jump()
	{
		if (canJump && _availableJumps > 0)
		{
			isJumping = true;
			_availableJumps--;
			_jumpTimeTracker = 0;
			_fallTimeTracker = timeToMaxFallSpeed;
		}
	}
	private void HandleJump()
	{
		if (isJumping)
		{
			_jumpTimeTracker += Time.deltaTime;
			_movementDir.y = jumpCurve.Evaluate(_jumpTimeTracker / jumpDuration) * fallSpeed;

			if (_jumpTimeTracker > jumpDuration)
			{
				isJumping = false;
				_jumpTimeTracker = 0;
			}
		}
	}
	private void ApplyFallSpeed()
	{
		if(!onGround && !isJumping)
		{
			_fallTimeTracker += Time.deltaTime;
			_movementDir += Vector2.down * -fallCurve.Evaluate(_fallTimeTracker/timeToMaxFallSpeed) * fallSpeed;
		}

	}
}
