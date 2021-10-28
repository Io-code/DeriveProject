using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fox.Editor;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    public Player pc;
	//Inspector Elements
	//Debug
	public bool debug;
	public float minAccelerationStep = 1, maxAccelerationStep = 10;
	public float minDecelerationStep = 1, maxDecelerationStep = 10;

	//Script Elements
	//Movement elements
	public float maxSpeed;
	public float accelerationStep;
	public float decelerationStep;

	//Push
	public CurveOptions curves = new CurveOptions();
	private Coroutine pushCoroutine;

	private Vector2 lastNonNullDirection;
	private Coroutine decelerationCoroutine, accelerationCoroutine;

	private bool moveFunctionCall;
	private void Awake()
	{
		pc = new Player(GetComponent<Rigidbody2D>(), in maxSpeed, in accelerationStep, in decelerationStep);
		
	}

	public void Push(Transform position, float force)
	{
		Vector3 direction = transform.position - position.position;
		Push(-direction, force);
	}

	public void Push(Vector3 direction, float force)
	{
		StartCoroutine(curves.Play(-direction, force));
		if (pushCoroutine != null)
        {
			StopCoroutine(pushCoroutine);
        }
		pushCoroutine = StartCoroutine(PushController());
	}

	// New Input system
	public void PerformMove(InputAction.CallbackContext value)
	{
		Vector2 dir = value.ReadValue<Vector2>();
		InputHandler.CallMove(dir, this);
		lastNonNullDirection = dir;
		if (dir == Vector2.zero)
		{
			decelerationCoroutine = StartCoroutine(pc.Deceleration());
			if (accelerationCoroutine != null) StopCoroutine(accelerationCoroutine);
		}
		else
		{
			pc.lastNonNullDirection = dir;
			accelerationCoroutine = StartCoroutine(pc.Acceleration());
			if (decelerationCoroutine != null) StopCoroutine(decelerationCoroutine);
		}
	}

	public void PerformInteract(InputAction.CallbackContext value)
    {
		if(value.started)
			InputHandler.CallInteract(this);
	}

	public void PerformAttack(InputAction.CallbackContext value)
	{
		if (value.started)
			InputHandler.CallAttack(this);
	}

	private IEnumerator PushController()
	{
		pc.currentState = PlayerState.PUSH;
		while (curves.isPlaying)
		{
			pc.Move(curves.velocity);
			yield return new WaitForFixedUpdate();
		}
		pc.Move(Vector2.zero);
		pc.currentState = PlayerState.FREE;
	}
}
