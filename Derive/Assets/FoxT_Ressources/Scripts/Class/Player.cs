using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {PUSH, SWIM, FREE}
public class Player
{
	//Movement elements
	private Rigidbody2D rb;

	public float maxSpeed;
	public float currentSpeed;
	private float accelerationStep;
	private float decelerationStep;

	public Vector2 lastNonNullDirection;

	public PlayerState currentState;

	public Player(Rigidbody2D _rb, in float _speed, in float _accelerationStep, in float _decelartionStep)
	{
		rb = _rb;
		maxSpeed = _speed * 100;
		accelerationStep = _accelerationStep / 10;
		decelerationStep = _decelartionStep / 10;
		currentState = PlayerState.FREE;
	}

	public void Move( Vector2 direction)
	{
		if (currentState == PlayerState.PUSH)
		{
			rb.velocity = direction * Time.fixedDeltaTime;
			return;
		}
		float acceleration = -decelerationStep;
		if (direction != Vector2.zero)
		{
			acceleration = accelerationStep;
			lastNonNullDirection = direction.normalized;
		}

		try
		{
			currentSpeed = Mathf.Clamp(currentSpeed + (maxSpeed * Time.fixedDeltaTime / acceleration), 0, maxSpeed);
		}
		catch
		{
			Debug.LogError("One of Playable Characters have Acceleration Step or Deceleration Step variable equal to 0, it's impossible.");
			currentSpeed = maxSpeed;
		}

		rb.velocity = lastNonNullDirection.normalized * currentSpeed * Time.deltaTime;
	}
	public IEnumerator Deceleration()
	{
		while (true)
		{
			yield return new WaitForFixedUpdate();
			Move(Vector2.zero);
			if (currentSpeed <= 0) break;
		}
	}

	public IEnumerator Acceleration()
	{
		while (true)
		{
			yield return new WaitForFixedUpdate();
			Move(lastNonNullDirection);
			if (currentSpeed >= maxSpeed) break;
		}
	}
}
