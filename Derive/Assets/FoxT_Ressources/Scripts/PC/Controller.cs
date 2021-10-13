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
	// On Floor
	public float maxSpeed;
	public float accelerationStep;
	public float decelerationStep;
	private Coroutine decelerationCoroutine, accelerationCoroutine;

	//On Water
	public float swimSpeed;
	private Vector3 laderPosition;

	//Push
	public CurveOptions curves = new CurveOptions();
	private Coroutine pushCoroutine;


	private void Awake()
	{
		pc = new Player(GetComponent<Rigidbody2D>(), in maxSpeed, in accelerationStep, in decelerationStep, (byte)GameObject.FindGameObjectsWithTag("Player").Length);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.collider.tag == "Water")
		{
			if (pc.currentState != PlayerState.SWIM)
			{
				pc.ChangeState(PlayerState.SWIM);
				// EventSystem
				// Changer Animaiton
			}
		}
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
		InputHandler.Instance.CallMove(dir, this);
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
			InputHandler.Instance.CallInteract(this);
	}

	public void PerformAttack(InputAction.CallbackContext value)
	{
		if (value.started)
			InputHandler.Instance.CallAttack(this);
	}

	private IEnumerator PushController()
	{
		pc.ChangeState(PlayerState.PUSH);
		while (curves.isPlaying)
		{
			pc.Move(curves.velocity);
			yield return new WaitForFixedUpdate();
		}
		pc.Move(Vector2.zero);
		pc.ChangeState(PlayerState.FREE);
	}

	public IEnumerator Swim()
	{
		while (pc.currentState == PlayerState.SWIM)
		{
			yield return new WaitForFixedUpdate();
			pc.Move(((transform.position - laderPosition) * 100).normalized * swimSpeed);
		}
	}
}
