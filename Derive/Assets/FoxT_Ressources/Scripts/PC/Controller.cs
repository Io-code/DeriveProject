using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    private Player pc;
	
	//Movement elements
	public float maxSpeed;
	public float accelerationStep;
	public float decelerationStep;
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

	private void InputHandler()
	{ 
		pc.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}
}
