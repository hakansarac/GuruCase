using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton to create only one GameManager object 
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    //end singleton

    [Header("Panels")]
    //Menu
    public GameObject panelMenu;
    //Play
    public GameObject panelPlay;
    //LevelCompleted
    public GameObject panelFail;
    //Try Again
    public GameObject panelSuccess;

    //[SerializeField] private TextMeshProUGUI m_TextLevel;

    //Current level
    private int m_CurrentLevel;
    public int CurrentLevel
    {
        get { return m_CurrentLevel; }
    }

    //Current gold
    private int m_Gold;
    public int Gold
    {
        get { return m_Gold; }
    }

    //Current diamond
    private int m_Diamond;
    public int Diamond
    {
        get { return m_Diamond; }
    }

    //Game states
    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, TRYAGAIN }
    public State state;

    public delegate void GameState();
    public static event GameState GameStateChanged;
    public static event GameState LevelUp;
    public static event GameState LevelTextUpdate;
    public static event GameState GoldTextUpdate;
    public static event GameState DiamondTextUpdate;

    public delegate void ResetAction();
    public static event ResetAction ResetLevel;


    private void Start()
    {
        m_CurrentLevel = PlayerPrefs.GetInt("level") == 0 ? 1 : PlayerPrefs.GetInt("level");
        m_Gold = PlayerPrefs.GetInt("gold");
        m_Diamond = PlayerPrefs.GetInt("diamond");
        SwitchState(State.MENU);
    }

    /// <summary>
    ///     switchs gamemanager state
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        EndState();
        if(state== State.LEVELCOMPLETED && newState == State.MENU)
        {
            LevelUp();
        }
        state = newState;
        GameStateChanged();
        BeginState(newState);
    }

    /// <summary>
    ///     begin of new state
    /// </summary>
    /// <param name="newState"></param>
    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                panelPlay.SetActive(true);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                LevelTextUpdate();
                break;
            case State.LEVELCOMPLETED:
                m_CurrentLevel++;
                PlayerPrefs.SetInt("level", m_CurrentLevel);
                panelSuccess.SetActive(true);
                break;
            case State.LOADLEVEL:
                SwitchState(State.PLAY);
                break;
            case State.TRYAGAIN:
                panelFail.SetActive(true);
                ResetLevel();
                break;
        }
    }

    /// <summary>
    ///     end of last state
    /// </summary>
    void EndState()
    {
        switch (state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                panelPlay.SetActive(false);
                break;
            case State.LEVELCOMPLETED:
                panelSuccess.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.TRYAGAIN:
                panelFail.SetActive(false);
                break;
        }
    }


    
    /// <summary>
    ///     event trigger when user presses tap to start button
    /// </summary>
    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    /// <summary>
    ///     event trigger when user presses tap to retry or next level buttons
    /// </summary>
    public void ReturnMenu()
    {
        SwitchState(State.MENU);
    } 

    /// <summary>
    ///     if user is failed, start try again state
    /// </summary>
    public void Failed()
    {
        SwitchState(State.TRYAGAIN);
    }

    /// <summary>
    ///     if user is failed, start level completed state
    /// </summary>
    public void Successed()
    {
        SwitchState(State.LEVELCOMPLETED);
    }

    private void OnEnable()
    {
        RemoveCoin.GoldUpdate += NewGold;
        RemoveDiamond.DiamondUpdate += NewDiamond;
    }

    private void OnDisable()
    {
        RemoveCoin.GoldUpdate -= NewGold;
        RemoveDiamond.DiamondUpdate -= NewDiamond;
    }

    /// <summary>
    ///     update golds
    /// </summary>
    private void NewGold()
    {
        m_Gold += 10;
        PlayerPrefs.SetInt("gold", m_Gold);
        GoldTextUpdate();
    }

    /// <summary>
    ///     update diamonds
    /// </summary>
    private void NewDiamond()
    {
        m_Diamond += 1;
        PlayerPrefs.SetInt("diamond", m_Diamond);
        DiamondTextUpdate();
    }
}
