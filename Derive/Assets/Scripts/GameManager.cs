using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject player1;
    [SerializeField] 
    private GameObject player2;
    
    private void Start()
    {
        player1.GetComponent<PlayerInput>()
            .SwitchCurrentControlScheme(player1.GetComponent<PlayerInput>().defaultControlScheme, Keyboard.current);
        player2.GetComponent<PlayerInput>()
            .SwitchCurrentControlScheme(player2.GetComponent<PlayerInput>().defaultControlScheme, Keyboard.current);
    }
}
