using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
	[Header("References")]
	public Animator anim;
	public SpriteRenderer sprite;

	protected override void Update()
	{
		MoveHorizontal(Input.GetAxis("Horizontal"));
		if (Input.GetButtonDown("Jump"))
		{
			Jump();
			anim.SetTrigger("jumpTrigger");
		}
		base.Update();


		anim.SetBool("onGround", onGround);
		anim.SetBool("isMoving", isMoving);
		anim.SetBool("isFalling", rb.velocity.y < 0 ? true : false);

		if (rb.velocity.x > 0) sprite.flipX = false;
		else if (rb.velocity.x < 0) sprite.flipX = true;
		
	}
}
