using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UIData uiData;
    public UIPlayerData[] playerData;
    public Controller[] playerController;

    bool readyToPlay = true;
    bool endTrigger = true;
    float readyBuffer = 5f;
    float endPlayBuffer = 360;

    public GameObject uiPanel;
    [Header("Start")]
    public GameObject startPanel;

    [Header("Round")]
    public GameObject roundPanel;
    public float RoundShowTime = 4f;

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

            InputHandler.instance.OnAttack += ListenEndStartInput;
            InputHandler.instance.OnInteract += ListenEndStartInput;
        InputHandler.instance.OnMove += ListenEndStartInput;

        PlayerDataUtils.winRound += EndRound;
        
    }

    private void OnDisable()
    {
        InputHandler.instance.OnAttack -= ListenEndStartInput;
        InputHandler.instance.OnInteract -= ListenEndStartInput;
        InputHandler.instance.OnMove -= ListenEndStartInput;
        PlayerDataUtils.winRound -= EndRound;
    }
    private void Awake()
    {
        ListenEndStartInput(null);
        UpdateSingleton();
        AssignControllerToData();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            //playerData[i].lastInputTime = 0;
            PlayerDataUtils.ResetScore(playerData[i]);
        }
        if (uiData.round != 0)
            StartPlay();

    }
    void AssignControllerToData()
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if (i < playerController.Length)
                playerData[i].refPlayer = playerController[i];
        }
    }

    void ListenEndStartInput( Controller controller)
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].refPlayer == controller)
                playerData[i].lastInputTime = Time.time;
            
        }

        if (playerData[0].lastInputTime != 0 && playerData[1].lastInputTime != 0)
        {
            Debug.Log(playerData[0].lastInputTime + "/" + playerData[1].lastInputTime);
            if ((Mathf.Abs(playerData[0].lastInputTime - playerData[1].lastInputTime) < readyBuffer) && readyToPlay)
                StartPlay();
            
                
        }

        if (endTrigger)
        {
            endTrigger = false;
            VerifyEndPlay();
        }
           
    }
    void ListenEndStartInput(Vector2 dir, Controller controller)
    {
        ListenEndStartInput(controller);
    }
    public IEnumerator VerifyEndPlay()
    {
        yield return new WaitForSeconds(endPlayBuffer + 1);
        if(Mathf.Abs(playerData[0].lastInputTime - playerData[1].lastInputTime) >= endPlayBuffer)
            EndPlay();

        endTrigger = true;
    }

    #region UI Loop
    public void StartPlay()
    {
        readyToPlay = false;
        uiPanel.SetActive(false);
        startPanel.SetActive(false);
        Debug.Log("StartPlay");
        //StartCoroutine(DebugEnd());
    }
    public void EndRound(UIPlayerData winner)
    {
        PlayerDataUtils.UpdateRound(uiData);
        uiPanel.SetActive(true);
        roundPanel.SetActive(true);

        StartCoroutine(RoundDelay(3));
    }
    public void EndPlay()
    {
        for (int i = 0; i < playerData.Length; i++)
        {
           PlayerDataUtils.ResetPlayerData(playerData[i]);
            
        }
        PlayerDataUtils.ResetRound(uiData);
        readyToPlay = true;
        //PlayerDataUtils.UpdateRound(uiData);
        Debug.Log("EndPlay");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator DebugEnd()
    {
        yield return new WaitForSeconds(2);
            EndPlay();
    }

    public IEnumerator RoundDelay( float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (uiData.round <= 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            EndPlay();
    }
    #endregion

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

    [ContextMenu("End Round")]
    public void DebugEndRound()
    {
        EndRound(null);
    }
    #endregion
}
