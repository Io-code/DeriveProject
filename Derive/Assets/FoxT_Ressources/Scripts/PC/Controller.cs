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
	public int pcNumber;
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

	//Divers
	private Animator anim;
	private string currentAnimationState;
	public string[] animationState;

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
		animationState = new string[] { "RUN", "THROW", "RAME", "SWIM", "IDLE" };
		try
		{
			anim = GetComponentInChildren<Animator>();
			encoderValues = GameObject.Find("Uduino").GetComponent<ReadEncoder>();
			laders = GameObject.Find("Uduino").GetComponent<LaderManager>();
			pc = new Player(GetComponent<Rigidbody2D>(), anim, in maxSpeed, in accelerationStep, in decelerationStep, (byte)pcNumber, in animationState);
		}
		catch
		{
			Debug.LogError("Player " + pc.playerNumber + " Can't Find Uduino GameObject or scripts inside, swim can't run");
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			GetComponent<Rigidbody2D>().isKinematic = true;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			GetComponent<Rigidbody2D>().isKinematic = false;
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
		pc.onWater = true;
	}

	// New Input system
	public void PerformMove(InputAction.CallbackContext value)
	{
		if (pc.currentState != PlayerState.FREE) return;
		Vector2 dir = value.ReadValue<Vector2>();
		InputHandler.CallMove(dir, this);
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
		if (value.started)
			InputHandler.CallInteract(this);
	}

	public void PerformAttack(InputAction.CallbackContext value)
	{
		if (value.started)
			InputHandler.CallAttack(this);
	}

	private IEnumerator PushController()
	{
		Debug.Log("Push");
		pc.ChangeState(PlayerState.PUSH);
		while (curves.isPlaying)
		{
			Physics2D.IgnoreLayerCollision(6, 7, true);
			pc.Move(curves.velocity);
			yield return new WaitForFixedUpdate();
		}
		pc.Move(Vector2.zero);
		if (pc.onWater)
		{
			pc.ChangeState(PlayerState.SWIM);
			laderPosition = laders.LaderPosition(transform.position);
			endLaderPosition = laders.EndLaderPosition();
			StartCoroutine(Swim());
		}
		else pc.ChangeState(PlayerState.FREE);

		Physics2D.IgnoreLayerCollision(6, 7, false);
	}

	public IEnumerator Swim()
	{
		float encoderValue;
		encoderCoroutine = StartCoroutine(encoderValues.SwimRead(pc.playerNumber));
		pc.currentSpeed = 0;
		while (Mathf.Abs(laderPosition.x - transform.position.x) > 0.2f || Mathf.Abs(laderPosition.y - transform.position.y) > 0.2f /*!V3MoreOrLess(transform.position, laderPosition, 0.2f) == true*/)
		{
			yield return new WaitForFixedUpdate();
			if (pcNumber == 1) encoderValue = encoderValues.fpSwimSpeed;
			else encoderValue = encoderValues.spSwimSpeed;
			pc.Move(((laderPosition - transform.position) * 10000000).normalized * swimSpeed * encoderValue);
		}
		transform.position = new Vector3(endLaderPosition.x, endLaderPosition.y, transform.position.z);
		pc.Move(Vector2.zero);
		pc.onWater = false;
		pc.ChangeState(PlayerState.FREE);
		pc.ChangeAnimationState(animationState[4]);
		StopCoroutine(encoderCoroutine);
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

	public void ChangeAnimationState(string newState)
	{
		if (pc.currentAnimationState == newState)
		{
			return;
		}
		float delay = 0;
		if (newState == animationState[1]) delay = 0.3f;
		else if (newState == animationState[2]) delay = 0.4f;
		pc.ChangeAnimationState(newState);
		pc.animationBlocked = true;
		StartCoroutine(AnimationDelay(delay));
	}
	private IEnumerator AnimationDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		pc.currentAnimationState = animationState[4];
		pc.animationBlocked = false;
	}
}
