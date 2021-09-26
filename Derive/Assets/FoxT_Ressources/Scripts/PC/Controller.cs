using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fox.Editor;

[RequireComponent(typeof(Rigidbody))]
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
		pc = new Player(GetComponent<Rigidbody>(), in maxSpeed, in accelerationStep, in decelerationStep);
	}

	void Start()
    {
        
    }

    void Update()
    {
		InputHandler();
    }

	private void FixedUpdate()
	{
		if (curves.isPlaying)
		{
			Debug.Log(curves.velocity);
			GetComponent<Rigidbody>().velocity = curves.velocity;
		}
		else pc.Move(joystick);
	}

	private void InputHandler()
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
}
