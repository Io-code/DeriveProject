using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	//Movement elements
	private Rigidbody rb;

	private float maxSpeed;
	private float currentSpeed;
	private float accelerationStep;
	private float decelerationStep;

	private Vector2 lastDirection;

	public Player(Rigidbody _rb, in float _speed, in float _accelerationStep, in float _decelartionStep)
	{
		rb = _rb;
		maxSpeed = _speed *100;
		accelerationStep = _accelerationStep / 10;
		decelerationStep = _decelartionStep / 10;
	}

	public void Move( Vector2 direction)
	{
		Debug.Log("try To Move");
		float acceleration = -decelerationStep;
		if (direction != Vector2.zero)
		{
			acceleration = accelerationStep;
			lastDirection = direction;
		}

		try
		{
			currentSpeed = Mathf.Clamp(currentSpeed + (maxSpeed * Time.deltaTime / acceleration), 0, maxSpeed);
		}
		catch
		{
			Debug.LogError("One of Playable Characters have Acceleration Step or Deceleration Step variable equal to 0, it's impossible.");
			currentSpeed = maxSpeed;
		}
		Debug.Log(currentSpeed);
		rb.velocity = lastDirection.normalized * currentSpeed * Time.deltaTime;
	}
}
