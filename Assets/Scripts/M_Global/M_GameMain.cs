using UnityEngine;
using System.Collections;

public class M_GameMain : MonoBehaviour
{
    /* クラス説明
     * 
     *      ゲーム中の設定、アイテムゲットなどの判定、条件を行う
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_GameMain    INSTANCE;
    public static bool          GAME_PAUSED;

    private float   m_FadeSpeed                 = 2.0f;
    private float[] m_ChapterFadeINAlpha        = { -0.75f, -0.0f, -0.5f, -1.5f };
    private float   m_MainFadeInAlpha           = 4.0f;

    private Vector3 m_OriginStartPosForMark     = new Vector3(-97.25f, 0.0f, 0.0f);
    private Vector3 m_OriginStartPosForMark2    = new Vector3(-87.25f, 0.0f, 0.0f);

    public Const.GAME_STATUS    CurrentGameStatus   = Const.GAME_STATUS.Reset;
    public Renderer             FadePlaneMaterial;
    public GameObject[]         ChapterNumbers;
    public GameObject           ChapterName;
    public int                  KeyGet              = 0;
    public GameObject[]         GUIKeys;

    public GameObject           StartPointForMark;
    public GameObject           StartPointForMark2;
    public Collider             StartWallCollider;
    public Collider             StartStandPlane;

    public GameObject           EndPointForMarks;
    public Collider             EndWallCollider;
    public Collider             EndStandPlane;

    private Vector3[]           ClearGUIDesiredPos;
    private Transform[]         GUIAssets;
    private GameObject          GUIMain;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        Reset();
    }

    void Update()
    {
        //if (Input.GetButton("Jump") &&
        //    Input.GetButton("TriggerL") &&
        //    Input.GetButton("TriggerR") &&
        //    Input.GetButton("Use") &&
        //    Input.GetButton("L3"))
        //{
        //    Application.LoadLevel(0);
        //}

        //if (Input.GetButton("TriggerL") &&
        //    Input.GetButton("TriggerR") &&
        //    Input.GetButton("L3") &&
        //    Input.GetButton("R3"))
        //{
        //    Application.LoadLevel(Application.loadedLevel);
        //}

        switch (CurrentGameStatus)
        {
            case Const.GAME_STATUS.Reset:
                Reset();
                break;
            case Const.GAME_STATUS.FadeIn:
                FadeIn();
                break;
            case Const.GAME_STATUS.Playing:
                PauseFade();
                if (Input.GetKeyDown(KeyCode.Escape)||
                    Input.GetButtonDown("Start"))
                {
                    if (!GAME_PAUSED)
                    {
                        if (this.GetComponent<M_PauseController>() == null)
                        {
                            this.gameObject.AddComponent<M_PauseController>();
                        }
                        GAME_PAUSED = true;
                    }
                }
                break;
            case Const.GAME_STATUS.GameOver:
                PauseFade();
                break;
            case Const.GAME_STATUS.GameClear:
                PauseFade();
                GameClearProcess();
                break;
            case Const.GAME_STATUS.FadeOut:
                FadeOut();
                break;
            case Const.GAME_STATUS.FadeOutInPause:
            default:
                break;
        }
    }

    void Reset()
    {
        GAME_PAUSED                             = false;

        ChapterNumbers                          = new GameObject[3];
        ChapterNumbers[0]                       = GameObject.Find("ChapterDot") as GameObject;              //the dot
        ChapterNumbers[1]                       = GameObject.Find("ChapterNumber0") as GameObject;          //the number1
        ChapterNumbers[2]                       = GameObject.Find("ChapterNumber1") as GameObject;          //the number2
        ChapterName                             = GameObject.Find("ChapterName") as GameObject;

        ChapterNumbers[0].transform.position    = new Vector3(-20, 25, -190);
        ChapterNumbers[1].transform.position    = new Vector3(-40, 25, -190);
        ChapterNumbers[2].transform.position    = new Vector3(-30, 25, -190);
        ChapterName.transform.position          = new Vector3(30, 25, -191);

        StartPointForMark                       = new GameObject();
        StartPointForMark2                      = new GameObject();
        StartPointForMark.name                  = "StartPointForMark";
        StartPointForMark2.name                 = "StartPointForMark2";
        StartPointForMark.transform.position    = new Vector3(-30, 0, 0);
        StartPointForMark2.transform.position   = new Vector3(-35, 0, 0);

        M_Controller_Mark.MARK_CHARCONTROLLER.transform.position    = m_OriginStartPosForMark;
        M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position  = m_OriginStartPosForMark2;

        StartWallCollider                       = GameObject.Find("LeftFloor").GetComponent<BoxCollider>();
        StartWallCollider.enabled               = false;

        StartStandPlane                         = GameObject.Find("StartStandPlane").GetComponent<BoxCollider>();
        StartStandPlane.enabled                 = true;

        EndPointForMarks                        = new GameObject();
        EndPointForMarks.name                   = "EndPointFormarks";
        EndPointForMarks.transform.position     = new Vector3(52.5f, 0.0f, 0.0f);

        EndWallCollider                         = GameObject.Find("RightFloor").GetComponent<BoxCollider>();
        EndWallCollider.enabled                 = true;

        EndStandPlane                           = GameObject.Find("EndStandPlane").GetComponent<BoxCollider>();
        EndStandPlane.enabled                   = false;

        GUIMain                                 = GameObject.Find("____GUI") as GameObject;

        ClearGUIDesiredPos                      = new Vector3[3];
        GUIAssets                               = new Transform[3];

        foreach (Transform child in GUIMain.transform)
        {
            if (child.name == "Mark2GUI")
            {
                GUIAssets[0] = child;
            }
            if (child.name == "MarkGUI")
            {
                GUIAssets[1] = child;
            }
            if (child.name == "PlayerSwitcher")
            {
                GUIAssets[2] = child;
            }
        }

        ClearGUIDesiredPos[0] = GUIAssets[0].position + Vector3.right * 100.0f;
        ClearGUIDesiredPos[1] = GUIAssets[1].position + Vector3.left * 100.0f;
        ClearGUIDesiredPos[2] = GUIAssets[2].position + Vector3.up * 100.0f;

        GUIKeys             = new GameObject[3];
        for (int i = 0; i < GUIKeys.Length; i++)
        {
            GUIKeys[i]      = GameObject.Find("GUI_Key" + (i + 1).ToString()) as GameObject;
        }
        INSTANCE            = this;
        KeyGet              = 0;
        FadePlaneMaterial   = GameObject.Find("FadePlane").renderer;
        CurrentGameStatus   = Const.GAME_STATUS.FadeIn;
    }

    void FadeInChapter()
    {
        for (int i = 0; i < m_ChapterFadeINAlpha.Length; i++)
        {
            if (m_MainFadeInAlpha > 0.0f)
            {
                m_ChapterFadeINAlpha[i] += Time.deltaTime;
                if (m_ChapterFadeINAlpha[i] > 1)
                {
                    m_ChapterFadeINAlpha[i] = 1;
                }
            }
            else
            {
                m_ChapterFadeINAlpha[i] -= Time.deltaTime;
                if (m_ChapterFadeINAlpha[i] < 0.0f)
                {
                    m_ChapterFadeINAlpha[i] = 0.0f;
                    if (i < m_ChapterFadeINAlpha.Length - 1)
                    {
                        ChapterNumbers[i].renderer.enabled = false;
                    }
                    else
                    {
                        ChapterName.renderer.enabled = false;
                    }
                }
            }
            if (m_ChapterFadeINAlpha[i] > 0)
            {
                if (i < m_ChapterFadeINAlpha.Length - 1)
                {
                    ChapterNumbers[i].renderer.material.color = new Color(1.0f, 1.0f, 1.0f, m_ChapterFadeINAlpha[i]);
                }
                else
                {
                    ChapterName.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, m_ChapterFadeINAlpha[i]);
                }
            }
            else
            {
                if (i < m_ChapterFadeINAlpha.Length - 1)
                {
                    ChapterNumbers[i].renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                else
                {
                    ChapterName.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
            }
            if (i < m_ChapterFadeINAlpha.Length - 1)
            {
                ChapterNumbers[i].transform.Translate(Vector3.left * 20f * Time.deltaTime, Space.World);
            }
            else
            {
                ChapterName.transform.Translate(Vector3.left * 20f * Time.deltaTime, Space.World);
            }
        }
    }

    void FadeInAll()
    {
        if (m_MainFadeInAlpha > 1f)
        {
            m_MainFadeInAlpha -= Time.deltaTime;
            FadePlaneMaterial.material.color = new Color(FadePlaneMaterial.material.color.r,
                                                         FadePlaneMaterial.material.color.g,
                                                         FadePlaneMaterial.material.color.b,
                                                         1.0f);
        }
        else
        {
            m_MainFadeInAlpha -= m_FadeSpeed * Time.deltaTime;
            if (m_MainFadeInAlpha < 0.01f)
            {
                m_MainFadeInAlpha = 0.0f;
                FadePlaneMaterial.enabled = false;
            }
            FadePlaneMaterial.material.color = new Color(FadePlaneMaterial.material.color.r,
                                                         FadePlaneMaterial.material.color.g,
                                                         FadePlaneMaterial.material.color.b,
                                                         m_MainFadeInAlpha);
        }
    }

    void PauseFade()
    {
        if (GAME_PAUSED)
        {
            FadePlaneMaterial.enabled = true;
            m_MainFadeInAlpha += m_FadeSpeed * 2 * Time.deltaTime;
            if (m_MainFadeInAlpha > 0.75f)
            {
                m_MainFadeInAlpha = 0.75f;
            }
        }
        else
        {
            m_MainFadeInAlpha -= m_FadeSpeed * 2 * Time.deltaTime;
            if (m_MainFadeInAlpha < 0.0f)
            {
                m_MainFadeInAlpha = 0.0f;
                FadePlaneMaterial.enabled = false;
            }
        }
        FadePlaneMaterial.material.color = new Color(FadePlaneMaterial.material.color.r,
                                                     FadePlaneMaterial.material.color.g,
                                                     FadePlaneMaterial.material.color.b,
                                                     m_MainFadeInAlpha);
    }

    void FadeIn()
    {
        FadeInChapter();
        FadeInAll();
        if (m_MainFadeInAlpha <= 0f)
        {
            if (M_Motor_Mark.INSTANCE.WalkToStartPoint() &&
                M_Motor_Mark2.INSTANCE.WalkToStartPoint())
            {
                for (int i = 0; i < m_ChapterFadeINAlpha.Length; i++)
                {
                    if (m_ChapterFadeINAlpha[i] > 0f)
                    {
                        m_ChapterFadeINAlpha[i] = 0f;
                        return;
                    }
                    if (i < m_ChapterFadeINAlpha.Length - 1)
                    {
                        Destroy(ChapterNumbers[i]);
                    }
                    else
                    {
                        Destroy(ChapterName);
                    }
                    StartWallCollider.enabled = true;
                    Destroy(StartStandPlane.gameObject);
                    CurrentGameStatus = Const.GAME_STATUS.Playing;
                }
            }
        }
    }

    void FadeOut()
    {
        FadePlaneMaterial.enabled = true;
        m_MainFadeInAlpha += m_FadeSpeed * Time.deltaTime;
        if (m_MainFadeInAlpha > 1.0f)
        {
            m_MainFadeInAlpha = 1.0f;
            if (M_Motor_Mark.INSTANCE.IsDying)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel + 1);
            }
        }
        FadePlaneMaterial.material.color = new Color(FadePlaneMaterial.material.color.r,
                                                     FadePlaneMaterial.material.color.g,
                                                     FadePlaneMaterial.material.color.b,
                                                     m_MainFadeInAlpha);
    }

    void GameClearProcess()
    {
        GameClearSettings();
        ScaleOutGUI();
    }

    void GameClearSettings()
    {
        EndWallCollider.enabled = false;
        EndStandPlane.enabled   = true;
    }

    void ScaleOutGUI()
    {
        ClearGUIDesiredPos[0].y = GUIAssets[0].position.y;
        ClearGUIDesiredPos[1].y = GUIAssets[1].position.y;
        ClearGUIDesiredPos[2].x = GUIAssets[2].position.x;

        for (int i = 0; i < GUIAssets.Length; i++)
        {
            GUIAssets[i].transform.position = Vector3.Lerp(GUIAssets[i].transform.position, ClearGUIDesiredPos[i], Time.deltaTime);
        }
    }

    #endregion
}
