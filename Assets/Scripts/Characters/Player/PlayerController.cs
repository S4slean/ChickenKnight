using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
	[Header("References")]
	public Animator anim;
	public SpriteRenderer sprite;

	[Header("Attack")]
	[Range(0.01f, 1f)] public float attackBufferDelay = 0.1f;

	[Header("States")]
	public int attacks;

	private float _attackTimeTracker;

	protected override void Update()
	{
		MoveHorizontal(Input.GetAxis("Horizontal"));
		if (Input.GetButtonDown("Jump"))
		{
			Jump();
			anim.SetTrigger("jumpTrigger");
		}

		if (Input.GetButtonDown("Attack"))
		{
			Attack();
		}

		base.Update();

		ClearAttackBuffer();

		anim.SetInteger("attacks", attacks);
		anim.SetBool("onGround", onGround);
		anim.SetBool("isMoving", isMoving);
		anim.SetBool("isFalling", rb.velocity.y < 0 ? true : false);

		if (rb.velocity.x > 0) sprite.flipX = false;
		else if (rb.velocity.x < 0) sprite.flipX = true;

	}

	private void ClearAttackBuffer()
	{
		if (attacks > 0)
		{
			_attackTimeTracker += Time.deltaTime;
			if (_attackTimeTracker >= attackBufferDelay)
			{
				attacks = 0;
				_attackTimeTracker = 0;
				canMove = true;
			}
		}
	}

	public void Attack()
	{
		attacks++;
		_attackTimeTracker = 0;
	}
}
