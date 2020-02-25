using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[Header("References")]
	public Transform self;
	public Rigidbody rb;

	[Header("Movement Stats")]
	public AnimationCurve acceleration;
	public float maxSpeed;
}
