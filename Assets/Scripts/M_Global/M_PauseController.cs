using UnityEngine;
using System.Collections;

public class M_PauseController : MonoBehaviour
{
    /* クラス説明
     * 
     *      PauseMenu処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    enum PauseMenuStatus
    {
        Selecting,
        FadeOutToGame,
        FadeOutToOther
    }
    private PauseMenuStatus m_PauseMenuStatus;

    enum PauseMenuSelection
    {
        ReturnToGame = 0,
        Restart,
        BackToMainMenu
    }
    private PauseMenuSelection m_DesiredStatus;
    private PauseMenuSelection m_CurrentMenuStatus;

    private M_MenuSelection m_ReturnToGame;
    private M_MenuSelection m_Restart;
    private M_MenuSelection m_BackToMainMenu;

    private M_MenuSelection m_SelectLineLeft;
    private M_MenuSelection m_SelectLineRight;
    private M_MenuSelection m_SelectPaint;

    private Material m_FadePlane;

    private GameObject m_MenuSelector;

    private string m_DesiredStageName;

    #endregion

    #region Function

    void Start()
    {
        m_PauseMenuStatus = PauseMenuStatus.Selecting;
        m_MenuSelector = GameObject.Find("Menu") as GameObject;

        if (GameObject.Find("Resume").GetComponent<M_MenuSelection>() != null)
            m_ReturnToGame = GameObject.Find("Resume").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_ReturnToGame = GameObject.Find("Resume").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Restart").GetComponent<M_MenuSelection>() != null)
            m_Restart = GameObject.Find("Restart").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Restart = GameObject.Find("Restart").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("ExitGame").GetComponent<M_MenuSelection>() != null)
            m_BackToMainMenu = GameObject.Find("ExitGame").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_BackToMainMenu = GameObject.Find("ExitGame").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("SelectLineL").GetComponent<M_MenuSelection>() != null)
            m_SelectLineLeft = GameObject.Find("SelectLineL").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineLeft = GameObject.Find("SelectLineL").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("SelectLineR").GetComponent<M_MenuSelection>() != null)
            m_SelectLineRight = GameObject.Find("SelectLineR").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineRight = GameObject.Find("SelectLineR").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("SelectPaint").GetComponent<M_MenuSelection>() != null)
            m_SelectPaint = GameObject.Find("SelectPaint").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectPaint = GameObject.Find("SelectPaint").AddComponent<M_MenuSelection>() as M_MenuSelection;

        m_DesiredStatus     = PauseMenuSelection.ReturnToGame;
        m_CurrentMenuStatus = PauseMenuSelection.ReturnToGame;

        m_FadePlane = GameObject.Find("FadePlane").GetComponent<Renderer>().material;

        m_DesiredStageName = Application.loadedLevelName;
    }

    void Update()
    {
        switch (m_PauseMenuStatus)
        {
            case PauseMenuStatus.Selecting:
                FadeIn();
                if (m_CurrentMenuStatus == m_DesiredStatus)
                    RecieveInput();
                float desiredPosition = GetDesiredSelectionPosition(m_DesiredStatus);
                SmoothToSelection(desiredPosition);
                break;
            case PauseMenuStatus.FadeOutToGame:
                FadeOutToGame();
                break;
            case PauseMenuStatus.FadeOutToOther:
                FadeOutToOtherStage();
                break;
            default:
                Debug.Log("How do you get here?");
                break;
        }
    }

    void FadeIn()
    {
        m_SelectLineLeft.DesiredAlpha   = 255.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 255.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 75.0f / 255.0f;
        m_ReturnToGame.DesiredAlpha     = 255.0f / 255.0f;
        m_Restart.DesiredAlpha          = 100.0f / 255.0f;
        m_BackToMainMenu.DesiredAlpha   = 50.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.05f;
        m_SelectLineRight.FadeSpeed     = 0.05f;
        m_SelectPaint.FadeSpeed         = 0.05f;
        m_ReturnToGame.FadeSpeed        = 0.05f;
        m_Restart.FadeSpeed             = 0.05f;
        m_BackToMainMenu.FadeSpeed      = 0.05f;

        m_ReturnToGame.MultiLanguagePorted      = true;
        m_Restart.MultiLanguagePorted           = true;
        m_BackToMainMenu.MultiLanguagePorted    = true;
    }

    void FadeOutToGame()
    {
        m_SelectLineLeft.DesiredAlpha   = 0.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 0.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 0.0f / 255.0f;
        m_ReturnToGame.DesiredAlpha     = 0.0f / 255.0f;
        m_Restart.DesiredAlpha          = 0.0f / 255.0f;
        m_BackToMainMenu.DesiredAlpha   = 0.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.2f;
        m_SelectLineRight.FadeSpeed     = 0.2f;
        m_SelectPaint.FadeSpeed         = 0.2f;
        m_ReturnToGame.FadeSpeed        = 0.2f;
        m_Restart.FadeSpeed             = 0.2f;
        m_BackToMainMenu.FadeSpeed      = 0.2f;

        if (m_SelectLineLeft.ZeroAlpha &&
            m_SelectLineRight.ZeroAlpha &&
            m_SelectPaint.ZeroAlpha &&
            m_ReturnToGame.ZeroAlpha &&
            m_Restart.ZeroAlpha &&
            m_BackToMainMenu.ZeroAlpha)
        {
            M_GameMain.GAME_PAUSED = false;
            Destroy(this);
        }
    }

    void FadeOutToOtherStage()
    {
        m_SelectLineLeft.DesiredAlpha   = 0.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 0.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 0.0f / 255.0f;
        m_ReturnToGame.DesiredAlpha     = 0.0f / 255.0f;
        m_Restart.DesiredAlpha          = 0.0f / 255.0f;
        m_BackToMainMenu.DesiredAlpha   = 0.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.2f;
        m_SelectLineRight.FadeSpeed     = 0.2f;
        m_SelectPaint.FadeSpeed         = 0.2f;
        m_ReturnToGame.FadeSpeed        = 0.2f;
        m_Restart.FadeSpeed             = 0.2f;
        m_BackToMainMenu.FadeSpeed      = 0.2f;

        if (m_SelectLineLeft.ZeroAlpha &&
            m_SelectLineRight.ZeroAlpha &&
            m_SelectPaint.ZeroAlpha &&
            m_ReturnToGame.ZeroAlpha &&
            m_Restart.ZeroAlpha &&
            m_BackToMainMenu.ZeroAlpha)
        {
            M_GameMain.INSTANCE.CurrentGameStatus = Const.GAME_STATUS.FadeOutInPause;
            float tempAlpha = m_FadePlane.color.a;
            tempAlpha += Time.deltaTime;
            if (tempAlpha > 1.0f)
            {
                tempAlpha = 1.0f;
                Application.LoadLevel(m_DesiredStageName);
                Destroy(this);
            }
            m_FadePlane.color = new Color(m_FadePlane.color.r,
                                          m_FadePlane.color.g,
                                          m_FadePlane.color.b,
                                          tempAlpha);
        }
    }

    void RecieveInput()
    {
        var deadZone = 0.1f;
        if (Input.GetAxis("Vertical") > deadZone)
        {
            var status = m_CurrentMenuStatus;
            status--;
            if (status < PauseMenuSelection.ReturnToGame)
            {
                status = PauseMenuSelection.ReturnToGame;
            }
            m_DesiredStatus = status;
        }
        else if (Input.GetAxis("Vertical") < -deadZone)
        {
            var status = m_CurrentMenuStatus;
            status++;
            if (status > PauseMenuSelection.BackToMainMenu)
            {
                status = PauseMenuSelection.BackToMainMenu;
            }
            m_DesiredStatus = status;
        }
        if (Input.GetButtonDown("Confirm"))
        {
            switch (m_DesiredStatus)
            {
                case PauseMenuSelection.ReturnToGame:
                    this.m_PauseMenuStatus = PauseMenuStatus.FadeOutToGame;
                    break;
                case PauseMenuSelection.Restart:
                    m_DesiredStageName = Application.loadedLevelName;
                    this.m_PauseMenuStatus = PauseMenuStatus.FadeOutToOther;
                    break;
                case PauseMenuSelection.BackToMainMenu:
                    m_DesiredStageName = "MainMenu_Level";
                    this.m_PauseMenuStatus = PauseMenuStatus.FadeOutToOther;
                    break;
                default:
                    return;
            }
        }
        else if (Input.GetButtonDown("Back") ||
                 Input.GetButtonDown("Start") ||
                 Input.GetKeyDown(KeyCode.Escape))
        {
            this.m_PauseMenuStatus = PauseMenuStatus.FadeOutToGame;
        }
    }

    void SmoothToSelection(float desiredSelection)
    {
        float tempY = m_MenuSelector.transform.position.y;
        tempY = Mathf.Lerp(tempY, desiredSelection, 0.2f);
        if (Mathf.Abs(tempY - desiredSelection) <= 1.0f)
        {
            m_CurrentMenuStatus = m_DesiredStatus;
        }
        m_MenuSelector.transform.position = new Vector3(m_MenuSelector.transform.position.x,
                                                        tempY,
                                                        m_MenuSelector.transform.position.z);
    }

    float GetDesiredSelectionPosition(PauseMenuSelection selection)
    {
        switch (selection)
        {
            case PauseMenuSelection.ReturnToGame:
                m_ReturnToGame.DesiredAlpha     = 255.0f / 255.0f;
                m_Restart.DesiredAlpha          = 100.0f / 255.0f;
                m_BackToMainMenu.DesiredAlpha   = 50.0f / 255.0f;
                m_ReturnToGame.FadeSpeed        = 0.2f;
                m_Restart.FadeSpeed             = 0.2f;
                m_BackToMainMenu.FadeSpeed      = 0.2f;
                return 0.0f;
            case PauseMenuSelection.Restart:
                m_ReturnToGame.DesiredAlpha     = 100.0f / 255.0f;
                m_Restart.DesiredAlpha          = 255.0f / 255.0f;
                m_BackToMainMenu.DesiredAlpha   = 100.0f / 255.0f;
                m_ReturnToGame.FadeSpeed        = 0.2f;
                m_Restart.FadeSpeed             = 0.2f;
                m_BackToMainMenu.FadeSpeed      = 0.2f;
                return 30.0f;
            case PauseMenuSelection.BackToMainMenu:
                m_ReturnToGame.DesiredAlpha     = 50.0f / 255.0f;
                m_Restart.DesiredAlpha          = 100.0f / 255.0f;
                m_BackToMainMenu.DesiredAlpha   = 255.0f / 255.0f;
                m_ReturnToGame.FadeSpeed        = 0.2f;
                m_Restart.FadeSpeed             = 0.2f;
                m_BackToMainMenu.FadeSpeed      = 0.2f;
                return 60.0f;
        }
        return 0.0f;
    }

    #endregion
}