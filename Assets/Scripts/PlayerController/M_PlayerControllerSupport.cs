using UnityEngine;
using System.Collections;

public class M_PlayerControllerSupport : MonoBehaviour
{
    /* クラス説明
     * 
     *      プレーやがゲームの中での共通動作を処理します
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_PlayerControllerSupport INSTANCE;

    public enum PlayerFadeState
    {
        noFade,
        fadeMark,
        fadeMark2
    }
    public PlayerFadeState CurrentPlayerFadeState;

    public enum PlayerSelection
    {
        Mark,
        Mark2
    }
    public PlayerSelection CurrentPlayerSelection;

    public float    FadeSpeed           = 1.7f;
    public float    MaxPlayerAlpha      = 1.0f;
    public float    MinPlayerAlpha      = 0.25f;

    public bool     CanMarkExit         = false;
    public bool     CanMark2Exit        = false;
    
    private float   m_CurrentMarkAlpha  = 0.0f;
    private float   m_CurrentMark2Alpha = 0.0f;
    private float   m_FlashSpeedMark    = 2.0f;
    private float   m_FlashSpeedMark2   = 2.0f;
    private int     m_QueueBase         = 3001;
    private int     m_QueueMark         = 3001;
    private int     m_QueueMark2        = 3002;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
        CurrentPlayerSelection = PlayerSelection.Mark;
    }

    //クラスを初期化します
    void Start()
    {
        m_CurrentMarkAlpha = MaxPlayerAlpha;
        m_CurrentMark2Alpha = MaxPlayerAlpha;
    }

    void Update()
    {
        if (M_GameMain.GAME_PAUSED)
        {
            return;
        }
        if (M_Controller_Mark2.MARK2_CHARCONTROLLER != null)
        {
            SwitchPlayer();
            CheckIfFade();
            CalculatePlayerMaterialAlpha();
            ProcessPlayersColor();
        }
    }

    void FixedUpdate()
    {
        if (M_Controller_Mark2.MARK2_CHARCONTROLLER != null)
        {
            IgnoreCollisionBtwMarks();
        }
    }

    void IgnoreCollisionBtwMarks()
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER.enabled == true &&
            M_Controller_Mark2.MARK2_CHARCONTROLLER.enabled == true)
            Physics.IgnoreCollision(M_Controller_Mark.MARK_CHARCONTROLLER, M_Controller_Mark2.MARK2_CHARCONTROLLER);
    }

    void CheckIfFade()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
        {
            CurrentPlayerFadeState = PlayerFadeState.fadeMark2;
            if (M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x + M_Controller_Mark.MARK_CHARCONTROLLER.radius < M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x - M_Controller_Mark2.MARK2_CHARCONTROLLER.radius * 2 ||
                M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x - M_Controller_Mark.MARK_CHARCONTROLLER.radius > M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x + M_Controller_Mark2.MARK2_CHARCONTROLLER.radius * 2 ||
                M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y > M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y + M_Controller_Mark2.MARK2_CHARCONTROLLER.height ||
                M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y + M_Controller_Mark.MARK_CHARCONTROLLER.height < M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y)
            {
                CurrentPlayerFadeState = PlayerFadeState.noFade;
            }
        }
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
        {
            CurrentPlayerFadeState = PlayerFadeState.fadeMark;
            if (M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x + M_Controller_Mark2.MARK2_CHARCONTROLLER.radius < M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x - M_Controller_Mark.MARK_CHARCONTROLLER.radius * 2 ||
                M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x - M_Controller_Mark2.MARK2_CHARCONTROLLER.radius > M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x + M_Controller_Mark.MARK_CHARCONTROLLER.radius * 2 ||
                M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y > M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y + M_Controller_Mark.MARK_CHARCONTROLLER.height ||
                M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y + M_Controller_Mark2.MARK2_CHARCONTROLLER.height < M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y)
            {
                CurrentPlayerFadeState = PlayerFadeState.noFade;
            }
        }
    }

    void CalculatePlayerMaterialAlpha()
    {
        if (CurrentPlayerFadeState == PlayerFadeState.fadeMark)
        {
            m_CurrentMarkAlpha -= FadeSpeed * Time.deltaTime;
            m_CurrentMark2Alpha += FadeSpeed * Time.deltaTime;
            if (m_CurrentMarkAlpha < MinPlayerAlpha)
                m_CurrentMarkAlpha = MinPlayerAlpha;
            if (m_CurrentMark2Alpha > MaxPlayerAlpha)
                m_CurrentMark2Alpha = MaxPlayerAlpha;
        }
        else if (CurrentPlayerFadeState == PlayerFadeState.fadeMark2)
        {
            m_CurrentMarkAlpha += FadeSpeed * Time.deltaTime;
            m_CurrentMark2Alpha -= FadeSpeed * Time.deltaTime;
            if (m_CurrentMark2Alpha < MinPlayerAlpha)
                m_CurrentMark2Alpha = MinPlayerAlpha;
            if (m_CurrentMarkAlpha > MaxPlayerAlpha)
                m_CurrentMarkAlpha = MaxPlayerAlpha;
        }
        else
        {
            m_CurrentMarkAlpha += FadeSpeed * Time.deltaTime;
            m_CurrentMark2Alpha += FadeSpeed * Time.deltaTime;
            if (m_CurrentMarkAlpha > MaxPlayerAlpha)
                m_CurrentMarkAlpha = MaxPlayerAlpha;
            if (m_CurrentMark2Alpha > MaxPlayerAlpha)
                m_CurrentMark2Alpha = MaxPlayerAlpha;
        }
    }

    void ProcessPlayersColor()
    {
        var mark    = GameObject.Find("Mark-body");
        var mark2   = GameObject.Find("Mark2-body");
        mark.renderer.material.color    = new Color(GameObject.Find("Mark-body").renderer.material.color.r,
                                                    GameObject.Find("Mark-body").renderer.material.color.g,
                                                    GameObject.Find("Mark-body").renderer.material.color.b,
                                                    m_CurrentMarkAlpha);
        mark2.renderer.material.color   = new Color(GameObject.Find("Mark2-body").renderer.material.color.r,
                                                    GameObject.Find("Mark2-body").renderer.material.color.g,
                                                    GameObject.Find("Mark2-body").renderer.material.color.b,
                                                    m_CurrentMark2Alpha);
        FlashPlayers(mark.renderer, mark2.renderer);
        mark.renderer.material.renderQueue  = m_QueueMark;
        mark2.renderer.material.renderQueue = m_QueueMark2;
    }

    void FlashPlayers(Renderer mark, Renderer mark2)
    {
        var markGUI     = GameObject.Find("Mark_GUIMain") as GameObject;
        var mark2GUI    = GameObject.Find("Mark2_GUIMain") as GameObject;

        if (CanMarkExit)
        {
            var tempColorG = mark.material.color.g;
            tempColorG += m_FlashSpeedMark * Time.deltaTime;
            if (tempColorG > 400.0f / 255.0f)
            {
                tempColorG = 400.0f / 255.0f;
                m_FlashSpeedMark *= -1;
            }
            else if (tempColorG < 125.0f / 255.0f)
            {
                tempColorG = 125.0f / 255.0f;
                m_FlashSpeedMark *= -1;
            }
            mark.material.color = new Color(tempColorG - 5.0f / 255.0f, tempColorG, tempColorG + 5.0f / 255.0f, mark.material.color.a);
            markGUI.renderer.material.color = new Color(tempColorG, tempColorG, tempColorG, markGUI.renderer.material.color.a);
        }
        else
        {
            if (!M_Motor_Mark.INSTANCE.IsDying)
            {
                mark.material.color = new Color(145.0f / 255.0f, 150.0f / 255.0f, 155.0f / 255.0f, mark.material.color.a);
            }
            markGUI.renderer.material.color = Color.white;
        }

        if (CanMark2Exit)
        {
            var tempColorG = mark2.material.color.g;
            tempColorG += m_FlashSpeedMark2 * Time.deltaTime;
            if (tempColorG > 400.0f / 255.0f)
            {
                tempColorG = 400.0f / 255.0f;
                m_FlashSpeedMark2 *= -1;
            }
            else if (tempColorG < 125.0f / 255.0f)
            {
                tempColorG = 125.0f / 255.0f;
                m_FlashSpeedMark2 *= -1;
            }
            mark2.material.color = new Color(tempColorG - 5.0f / 255.0f, tempColorG, tempColorG + 5.0f / 255.0f, mark2.material.color.a);
            mark2GUI.renderer.material.color = new Color(tempColorG, tempColorG, tempColorG, mark2GUI.renderer.material.color.a);
        }
        else
        {
            mark2.material.color = new Color(145.0f / 255.0f, 150.0f / 255.0f, 155.0f / 255.0f, mark2.material.color.a);
            mark2GUI.renderer.material.color = Color.white;
        }
    }

    void SwitchPlayer()
    {
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Q) ||
                Input.GetButtonDown("TriggerL") ||
                Input.GetButtonDown("TriggerR") ||
                Input.GetMouseButtonDown(2))
            {
                if (CurrentPlayerSelection == PlayerSelection.Mark)
                {
                    CurrentPlayerSelection = PlayerSelection.Mark2;
                    m_QueueMark = m_QueueBase + 1;
                    m_QueueMark2 = m_QueueBase;
                }
                else
                {
                    CurrentPlayerSelection = PlayerSelection.Mark;
                    m_QueueMark = m_QueueBase;
                    m_QueueMark2 = m_QueueBase + 1;
                }
            }
        }
    }

    public float LerpWalkSpeed(float from, float to)
    {
        return Mathf.Lerp(from, to, Time.time);
    }

    #endregion
}
