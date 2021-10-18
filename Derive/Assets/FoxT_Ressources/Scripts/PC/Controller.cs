using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fox.Editor;
using UnityEngine.InputSystem;


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
	private Vector3 laderPosition, endLaderPosition;
	private Coroutine swimCoroutine, encoderCoroutine;

	//Push
	public CurveOptions curves = new CurveOptions();
	private Coroutine pushCoroutine;

	//Arduino
	private ReadEncoder encoderValues;
	private LaderManager laders;

    private void OnEnable()
    {
		ShipEvent.OnExitPlayer += EnterWater;
    }

    private void OnDisable()
    {
		ShipEvent.OnExitPlayer -= EnterWater;
	}

    private void Awake()
	{
		pc = new Player(GetComponent<Rigidbody2D>(), in maxSpeed, in accelerationStep, in decelerationStep, (byte)GameObject.FindGameObjectsWithTag("Player").Length);
		try
		{
			encoderValues = GameObject.Find("Uduino").GetComponent<ReadEncoder>();
			laders = GameObject.Find("Uduino").GetComponent<LaderManager>();
		}
		catch
		{
			Debug.LogError("Player " + pc.playerNumber + " Can't Find Uduino GameObject or scripts inside, swim can't run");
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "EditorOnly")
		{
			if (pc.currentState != PlayerState.SWIM)
			{
				pc.ChangeState(PlayerState.SWIM);
				laderPosition = laders.LaderPosition(transform.position);
				endLaderPosition = laders.EndLaderPosition();
				StartCoroutine(Swim());
				// Changer Animaiton
			}
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.tag == "EditorOnly")
		{
			if (pc.currentState == PlayerState.SWIM)
			{
				pc.ChangeState(PlayerState.FREE);
				StopCoroutine(encoderCoroutine);
				//StopCoroutine(swimCoroutine);
				//Animation
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

	public void EnterWater(GameObject PLAYER)
    {
		// something
    }

	// New Input system
	public void PerformMove(InputAction.CallbackContext value)
	{
		if (pc.currentState != PlayerState.FREE) return;
		Vector2 dir = value.ReadValue<Vector2>();
		InputHandler.Instance.CallMove(dir, this);
		if (dir == Vector2.zero)
		{
			if (accelerationCoroutine != null) StopCoroutine(accelerationCoroutine);
			decelerationCoroutine = StartCoroutine(pc.Deceleration());
		}
		else
		{
			pc.lastNonNullDirection = dir;
			if (decelerationCoroutine != null) StopCoroutine(decelerationCoroutine);
			accelerationCoroutine = StartCoroutine(pc.Acceleration());
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
		float encoderValue;
		encoderCoroutine = StartCoroutine(encoderValues.SwimRead(pc.playerNumber));
		pc.currentSpeed = 0;
		while (!V3MoreOrLess(transform.position, laderPosition, 0.2f) == true)
		{
			yield return new WaitForFixedUpdate();
			encoderValue = encoderValues.swimSpeed;
			pc.Move(((laderPosition - transform.position) * 100).normalized * swimSpeed * encoderValue);
		}
		transform.position = endLaderPosition;
		pc.Move(Vector2.zero);
		//StopCoroutine(encoderCoroutine);
	}

	private bool V3MoreOrLess(Vector3 vectorCompared, Vector3 vectorComparer, float value)
	{
		if (MoreOrLess(vectorCompared.x, vectorComparer.x, value) && MoreOrLess(vectorCompared.y, vectorComparer.y, value) && MoreOrLess(vectorCompared.z, vectorComparer.z, value))
		{
			return true;
		}
		return false;
	}

	private bool MoreOrLess(float compared, float comparer, float value)
	{
		if (compared <= comparer + value && compared >= comparer - value) return true;
		else return false;
	}
}
