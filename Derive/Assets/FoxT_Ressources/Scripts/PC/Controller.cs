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

	private Vector2 joystick;
	private void Awake()
	{
		pc = new Player(GetComponent<Rigidbody2D>(), in maxSpeed, in accelerationStep, in decelerationStep);
		
	}

	void Start()
    {
		//InputHandler.Instance.OnMove += pc.Move; 
    }




	private void JoystickTest()
	{ 
		 joystick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (Input.GetButtonDown("Test")) Push(new Vector3(1, -1, 0), 1);
	}

	public void Push(Transform position, float force)
	{
		Vector3 direction = transform.position - position.position;
		Push(direction, force);
	}

	public void Push(Vector3 direction, float force)
	{
		StartCoroutine(curves.Play(-direction, force));
	}

	// New Input system
	public void PerformMove(InputAction.CallbackContext value)
	{
		Vector2 dir = value.ReadValue<Vector2>();
		InputHandler.Instance.CallMove(dir, this);
		pc.Move(dir);

		Debug.Log("Move : " + value.ReadValue<Vector2>());
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
}
