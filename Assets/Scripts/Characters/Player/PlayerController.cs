using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
	protected override void Update()
	{
		MoveHorizontal(Input.GetAxis("Horizontal"));
		if(Input.GetButtonDown("Jump")) Jump();

		base.Update();
	}
}
