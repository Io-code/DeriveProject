using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public GameObject inGameUI;

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

            InputHandler.OnAttack += ListenEndStartInput;
            InputHandler.OnInteract += ListenEndStartInput;
        InputHandler.OnMove += ListenEndStartInput;

        PlayerDataUtils.winRound += EndRound;
        
    }

    private void OnDisable()
    {
        InputHandler.OnAttack -= ListenEndStartInput;
        InputHandler.OnInteract -= ListenEndStartInput;
        InputHandler.OnMove -= ListenEndStartInput;
        PlayerDataUtils.winRound -= EndRound;
    }
    private void Awake()
    {
        ListenEndStartInput(null);
        UpdateSingleton();
        AssignControllerToData();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (uiData.inGame)
        {
            playersUI[0].slider.value = (int)playerData[0].distToObjectif;
            playersUI[0].scoreTxt1.text = playersUI[0].scoreTxt2.text = ((int)playerData[0].distToObjectif).ToString();

            playersUI[1].slider.value = (int)playerData[1].distToObjectif;
            playersUI[1].scoreTxt1.text = playersUI[1].scoreTxt2.text = ((int)playerData[1].distToObjectif).ToString();
        }
    }

    private void Start()
    {
        inGameUI.SetActive(false);
        for (int i = 0; i < playerData.Length; i++)
        {
            //playerData[i].lastInputTime = 0;
            PlayerDataUtils.ResetScore(playerData[i]);
        }
        if (uiData.inGame)
        {
            StartPlay();
            StartRound();
        } 
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
            //Debug.Log(playerData[0].lastInputTime + "/" + playerData[1].lastInputTime);
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
        if(uiData.inGame == false)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        uiData.inGame = true;


    }
    public void StartRound()
    {
        uiPanel.SetActive(true);
        roundPanel.SetActive(true);

        playersUI[0].roundNmber[uiData.round].SetActive(true);
        playersUI[1].roundNmber[uiData.round].SetActive(true);
        StartCoroutine(StartRoundDelay(2));
    }
    public void EndRound(UIPlayerData winner)
    {
        winner.winRound[uiData.round] = true;
        PlayerDataUtils.UpdateRound(uiData);

        int winRounds = 0;
        for (int i = 0; i < winner.winRound.Length; i++)
        {
            if (winner.winRound[i])
                winRounds++;
        }

        if ((winRounds < 2))
        {
            roundEndPanel.SetActive(true);
            for(int i = 0; i < playersUI.Length; i++)
            {
                if (playerData[i].winRound[uiData.round])
                    playersUI[i].winRoundImg.SetActive(true);
                else
                    playersUI[i].loseRoundImg.SetActive(true);

                playersUI[i].point[0].SetActive(playerData[i].winRound[0]);
                playersUI[i].point[1].SetActive(playerData[i].winRound[1]);

            }
        }
        else
        {
            finalEndPanel.SetActive(true);

            for (int i = 0; i < playersUI.Length; i++)
            {
                int totalWinRound = 0;
                for(int j = 0; j < playerData[i].winRound.Length; j++)
                {
                    if (playerData[i].winRound[j])
                        totalWinRound++;
                }
                if (totalWinRound > 1)
                {
                    playersUI[i].winTxt.SetActive(true);
                    playersUI[i].winImg.SetActive(true);
                }
                else
                {
                    playersUI[i].loseTxt.SetActive(true);
                    playersUI[i].loseImg.SetActive(true);
                }
                    

            }
        }
            

        StartCoroutine(EndRoundDelay(3));
    }
    public void EndPlay()
    {
        for (int i = 0; i < playerData.Length; i++)
        {
           PlayerDataUtils.ResetPlayerData(playerData[i]);
            
        }
        PlayerDataUtils.ResetRound(uiData);
        uiData.inGame = false;
        readyToPlay = true;
        //PlayerDataUtils.UpdateRound(uiData);
        Debug.Log("EndPlay");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator StartRoundDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        uiPanel.SetActive(false);
        roundPanel.SetActive(false);

        playersUI[0].roundNmber[uiData.round].SetActive(false);
        playersUI[1].roundNmber[uiData.round].SetActive(false);

        inGameUI.SetActive(true);
    }
    public IEnumerator EndRoundDelay( float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (uiData.round < 3)
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
        public GameObject[] roundNmber ; 


        [Header("RoundEnd")]
        public GameObject[] point;
        public GameObject winRoundImg, loseRoundImg;

        [Header("FinalEnd")]
        public GameObject winTxt;
        public GameObject winImg, loseTxt, loseImg;

        [Header("Score")]
        public Slider slider;
        public TextMeshProUGUI scoreTxt1, scoreTxt2;

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
