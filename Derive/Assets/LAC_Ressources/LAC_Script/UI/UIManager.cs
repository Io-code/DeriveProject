using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIPlayerData[] playerData;
    public Controller[] playerController;

    bool readyToPlay = true;
    bool endTrigger = true;
    float readyBuffer = 5f;
    float endPlayBuffer = 360;

    [Header("Start")]
    public GameObject startPanel;

    [Header("Round")]
    public GameObject roundPanel;

    [Header("Transition")]
    public GameObject goPanel;
    public GameObject finishPanel;

   

    [Header("RoundEnd")]
    public GameObject roundEndPanel;
  

    [Header("FinalEnd")]
    public GameObject finalEndPanel;

    public PlayerUI[] playersUI;

    private void OnEnable()
    {
        if (!readyToPlay)
        {
            InputHandler.instance.OnAttack += ListenStartInput;
            InputHandler.instance.OnInteract += ListenStartInput;
        }
    }

    private void OnDisable()
    {
        InputHandler.instance.OnAttack -= ListenStartInput;
        InputHandler.instance.OnInteract -= ListenStartInput;
    }
    private void Awake()
    {
        UpdateSingleton();
        AssignControllerToData();
        DontDestroyOnLoad(gameObject);
    }
    
    void AssignControllerToData()
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if (i < playerController.Length)
                playerData[i].refPlayer = playerController[i];
        }
    }

    void ListenStartInput(Controller controller)
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].refPlayer == controller)
                playerData[i].lastInputTime = Time.time;
            
        }

        if (playerData[0].lastInputTime != 0 && playerData[1].lastInputTime != 0)
        {
            if ((Mathf.Abs(playerData[0].lastInputTime - playerData[1].lastInputTime) < readyBuffer) && readyToPlay)
            {
                readyToPlay = false;
                StartPlay();
            }
                
        }
           
    }
    public IEnumerator VerifyEndPlay()
    {
        yield return new WaitForSeconds(endPlayBuffer + 1);
        if(Mathf.Abs(playerData[0].lastInputTime - playerData[1].lastInputTime) >= endPlayBuffer)
            EndPlay();
    }
    public void StartPlay()
    {

    }

    public void EndPlay()
    {
        readyToPlay = true;
    }

    [System.Serializable]
    public struct PlayerUI
    {
        public Controller refCtrl;

        [Header("Round")]
        public GameObject roundNmber1; 
        public GameObject roundNmber2, roundNmber3;

        [Header("RoundEnd")]
        public GameObject point1;
        public GameObject point2;
        public GameObject winRoundImg, loseRoundImg;

        [Header("FinalEnd")]
        public GameObject winTxt;
        public GameObject winImg, loseTxt, loseImg;

    }

    #region debug
    [ContextMenu("Update Singleton")]
    public void UpdateSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

    }
    #endregion
}
