using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public bool menuState;
    [SerializeField] private GameObject menuGroup;
    [SerializeField] private GameObject menuButtonGO;
    [SerializeField] private KeyCode menuKey = KeyCode.Escape;


    private void Awake()
    {
        UnpauseTime();
    }

    public void OnClickMenu()
    {
        menuGroup.SetActive(!menuState);
        menuButtonGO.SetActive(menuState);
        menuState = !menuState;

        if (menuState)
        {
            PauseTime();
        }
        else
        {
            UnpauseTime();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(menuKey))
        {
            OnClickMenu();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void UnpauseTime()
    {
        Time.timeScale = 1;
    }
}
