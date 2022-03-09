using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float playerSpeed = 2.0f;
    [SerializeField] 
    private float knockbackSpeed;
    [SerializeField] 
    private float force;
    
    [Space]
    [SerializeField] 
    private Animator animations;
    [SerializeField] 
    private Rigidbody rb;
    
    [Space]
    public GameObject hand;
    public bool interacting;
    
    private float cooldown;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 playerMovementInput;
    private Vector3 moveVector;
    private bool isAttacked;
    private bool canAttack = true;
    private float power;
    
    [SerializeField]
    private Transform respawn;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        playerMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && !interacting)
        {
            interacting = true;
            StartCoroutine(InteractingTime());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (hand.GetComponentInChildren<Rigidbody>() != null && canAttack)
            {
                animations.SetBool("Attack", true);
                canAttack = false;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            transform.position = respawn.position;
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(hand.GetComponentInChildren<WeaponController>().cooldown);
        canAttack = true;
        animations.SetBool("Attack", false);
    }
    
    private IEnumerator InteractingTime()
    {
        yield return new WaitForSeconds(0.1f);

        interacting = false;
    }

    
    private IEnumerator KnockbackHit(Vector3 hit, float power)
    {
        
        
        isAttacked = true;
        
        Vector3 destination = transform.position +
          new Vector3(
              (transform.position.x - hit.x) *  power,
              0,
              (transform.position.z - hit.z) * power
          );
        Debug.Log(destination);
        
        while ((isAttacked  && (destination - transform.position).magnitude > 4))
        {
            transform.position = Vector3.Lerp(transform.position, destination, knockbackSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        
        
        isAttacked = false;
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if ((other.transform.CompareTag("Trap") || other.transform.CompareTag("Spawner")) && isAttacked)
        {
            StopAllCoroutines();
            isAttacked = false;
        }
        if (other.transform.CompareTag("Weapon"))
        {
            StartCoroutine(KnockbackHit(other.transform.position, other.transform.GetComponentInParent<WeaponController>().power));
        }
        
        if (other.transform.CompareTag("Water"))
        {
            transform.position = respawn.position;
        }
    }
    
    
    private void MovePlayer()
    {
        moveVector = playerMovementInput * playerSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
    }

    private void Update()
    {
        MovePlayer();
        
        if (playerMovementInput != Vector3.zero)
        {
            gameObject.transform.forward = playerMovementInput.normalized;
        }

    }
}
