using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    private Player pc;

	//Inspector Elements
	//Debug
	public bool debug;
	public float minAccelerationStep = 1, maxAccelerationStep = 10;
	public float minDecelerationStep = 1, maxDecelerationStep = 10;
	
	//Push elements
	public sbyte currentPushMethod = 0, lastPushMethod = 0;
	public int howManyPart, lastPartNumber;

	//Script Elements
	//Movement elements
	public float maxSpeed;
	public float accelerationStep;
	public float decelerationStep;

	//Push
	public float forceMultiplicator;
	public bool curveUsed;
	//Simplified & Advanced methods
	public float[] pushDelay;
	public float[] pushDecelerationStep;
	//Curve method
	public AnimationCurve pushCurve;

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
		pc.Move(joystick);
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
		StartCoroutine(PushUpdate(direction, force));
	}

	private IEnumerator PushUpdate(Vector3 dir, float force)
	{
		pc.isPushing = true;
		float timeElapsed = 0;
		if (curveUsed)
		{
			while (true)
			{
				GetComponent<Rigidbody>().velocity = -dir * pushCurve.Evaluate(timeElapsed) * force * forceMultiplicator;
				if (timeElapsed >= pushCurve.keys[pushCurve.keys.Length - 1].time) break;
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
		}
		else
		{
			for (int i = 0; i < pushDelay.Length; i++)
			{
				while (true)
				{
					GetComponent<Rigidbody>().velocity = -dir * force * forceMultiplicator;
					try
					{
						force = Mathf.Clamp(force - (pushDecelerationStep[i] / pushDelay[i]), 0, force);
					}
					catch
					{
						Debug.LogError("On controller, about Push fonctions, Delay can't be equal to 0");
						break;
					}
					if (timeElapsed >= pushDelay[i]) break;
					yield return new WaitForEndOfFrame();
					timeElapsed += Time.deltaTime;
				}
			}
		}
		pc.isPushing = false;
	}
}
