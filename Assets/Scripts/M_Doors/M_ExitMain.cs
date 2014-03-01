using UnityEngine;
using System.Collections;

public class M_ExitMain : MonoBehaviour
{
    /* クラス説明
     * 
     *      出口の操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_ExitMain INSTANCE;

    private GameObject  m_DoorModel;
    private Transform   m_ExitDoor;
    private Renderer    m_DeExitPlane;
    private Renderer    m_CanExitPlane;
    private const int   m_RenderQueue   = 3003;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "ExitDoor")
            {
                m_ExitDoor = child.transform;
            }
            if (child.gameObject.name == "CanExitPlane")
            {
                m_CanExitPlane = child.renderer;
            }
            if (child.gameObject.name == "DeExitPlane")
            {
                m_DeExitPlane = child.renderer;
            }
        }
        m_DoorModel = GameObject.Find("ExitDoorModel") as GameObject;
        m_DoorModel.renderer.material.renderQueue = m_RenderQueue;
    }

    void Update()
    {
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
        {
            CheckPlayerCanExit();
            ProcessExitPlane();
            if (M_PlayerControllerSupport.INSTANCE.CanMarkExit &&
                M_PlayerControllerSupport.INSTANCE.CanMark2Exit)
            {
                M_GameMain.INSTANCE.CurrentGameStatus = Const.GAME_STATUS.GameClear;
            }
        }
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.GameClear)
        {
            if (M_Motor_Mark.INSTANCE.WalkToEndPoint() &&
                M_Motor_Mark2.INSTANCE.WalkToEndPoint())
            {
                CloseTheDoor();
            }
        }
    }

    void ProcessExitDoor()
    {
        m_ExitDoor.Rotate(0, -360 * Time.deltaTime, 0);
        if (m_ExitDoor.localEulerAngles.y < 240f)
        {
            m_ExitDoor.localEulerAngles = new Vector3(m_ExitDoor.transform.eulerAngles.x, 
                                                     240f, 
                                                     m_ExitDoor.transform.eulerAngles.z);
        }
    }

    void ProcessExitPlane()
    {
        if (M_GameMain.INSTANCE.KeyGet < 3)
        {
            m_DeExitPlane.enabled = true;
            m_CanExitPlane.enabled = false;
        }
        else
        {
            m_CanExitPlane.enabled = true;
            m_DeExitPlane.enabled = false;
            ProcessExitDoor();
        }
    }

    void CheckPlayerCanExit()
    {
        var posMark     = GameObject.Find("Mark").GetComponent<CharacterController>().transform.position;
        var posMark2    = GameObject.Find("Mark-II").GetComponent<CharacterController>().transform.position;
        var posExit     = this.collider.transform.position;

        if (M_GameMain.INSTANCE.KeyGet >= 3)
        {
            if (Mathf.Abs(posExit.y - posMark.y) <= 1f &&
                Mathf.Abs(posExit.x - posMark.x) <= 5f)
            {
                M_PlayerControllerSupport.INSTANCE.CanMarkExit = true;
            }
            else
            {
                M_PlayerControllerSupport.INSTANCE.CanMarkExit = false;
            }
            if (Mathf.Abs(posExit.y - posMark2.y) <= 1f &&
                Mathf.Abs(posExit.x - posMark2.x) <= 5f)
            {
                M_PlayerControllerSupport.INSTANCE.CanMark2Exit = true;
            }
            else
            {
                M_PlayerControllerSupport.INSTANCE.CanMark2Exit = false;
            }
        }
    }

    void CloseTheDoor()
    {
        m_ExitDoor.Rotate(0, 360 * Time.deltaTime, 0);
        if (m_ExitDoor.localEulerAngles.y > 0.0f &&
            m_ExitDoor.localEulerAngles.y <= 180.0f)
        {
            m_ExitDoor.localEulerAngles = Vector3.zero;
            M_GameMain.INSTANCE.CurrentGameStatus = Const.GAME_STATUS.FadeOut;
        }
    }

    #endregion
}
