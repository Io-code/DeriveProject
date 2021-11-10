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
	public Vector2 lastNonNullDirection;
	private float angle;
	private float accelerationStep;
	private float decelerationStep;

	public byte playerNumber;
	public PlayerState currentState;

	public bool onWater;
	private Animator anim;
	public string[] animationState; // 0 = RUN, 1 = THROW, 2 = SLASH, 3 = THROW, 4 IDLE
	public string currentAnimationState;
	public bool animationBlocked;

	public Player(Rigidbody2D _rb, Animator _anim, in float _speed, in float _accelerationStep, in float _decelartionStep, in byte _playerNumber, in string[] _animationState)
	{
		rb = _rb;
		anim = _anim;
		maxSpeed = _speed * 100;
		accelerationStep = _accelerationStep / 10;
		decelerationStep = _decelartionStep / 10;
		playerNumber = _playerNumber;
		currentState = PlayerState.FREE;
		animationState = _animationState;
	}

	public void Move(Vector2 direction)
	{
		if (currentState == PlayerState.SWIM) rb.transform.position -= new Vector3(0, 0, 0f);
		if (currentState != PlayerState.FREE)
		{
			rb.velocity = direction * Time.deltaTime;
			if (rb.velocity != Vector2.zero)
			{
				if (currentState == PlayerState.SWIM) ChangeAnimationState(animationState[3]);
			}
			if (animationBlocked) rb.velocity = Vector2.zero;
			return;
		}
		if (currentState == PlayerState.SWIM) rb.transform.position -= new Vector3(0, 0, 9.2f);
		if (animationBlocked)
		{
			Debug.Log("YA");
			rb.velocity = Vector2.zero;
			return;
		}
		float acceleration = -decelerationStep;
		if (direction != Vector2.zero)
		{
			acceleration = accelerationStep;
			lastNonNullDirection = direction.normalized;
			angle = Mathf.Atan2(lastNonNullDirection.y, lastNonNullDirection.x * -1) * Mathf.Rad2Deg - 90;
			if (angle < 0) angle += 360;
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
		anim.transform.localEulerAngles = new Vector3(0, angle, 0);

		if (rb.velocity != Vector2.zero) ChangeAnimationState(animationState[0]);
		else ChangeAnimationState(animationState[4]);
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

	public void ChangeState(PlayerState newState)
	{
		if (newState != currentState) currentState = newState;
	}

	public void CancelCurrentSpeed()
	{
		currentSpeed = 0;
	}

	public void ChangeAnimationState(string newState)
	{
		if (newState == currentAnimationState || animationBlocked) return;
		Debug.Log(newState + " " + animationBlocked);
		currentAnimationState = newState;
		anim.Play(newState);
	}
}
