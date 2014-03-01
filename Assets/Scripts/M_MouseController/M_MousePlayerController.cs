using UnityEngine;
using System.Collections;

public class M_MousePlayerController : MonoBehaviour
{
    /* クラス説明
     * 
     *      キャラクターのマウスで操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_MousePlayerController INSTANCE;

    public Vector2      MousePoint1                 = Vector2.zero;
    public Vector2      MousePoint2                 = Vector2.zero;
    public Vector3      TempJumpVector              = Vector3.zero;
    public Vector3      DesiredDropArrowPosition    = Vector3.zero;

    public float        OriginalDistanceForCameraZoom    = 0f;
    public float        CameraZoomLimit                  = 0f;
    public float        CameraZoomRate                   = 0f;

    public bool         CanBeJumpVector             = false;
    public bool         IsControllingGUI            = false;
    public bool         IsZoomingCamera             = false;

    public Rect         MouseControllerRect;

    private float           m_TriggerTime                   = 0f;
    private readonly float  m_TriggerTimeLimit              = 0.15f;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Start()
    {
        INSTANCE = this;
        MouseControllerRect = new Rect(0, 0, Screen.width, Screen.height * 0.8f);
        m_TriggerTime = 0f;
    }

    void Update()
    {
        if (!M_GUIController.IS_CONTROLLING_GUI)
        {
            ProcessMouseInput();
        }
    }

    bool CanMouseControl(Rect rect)
    {
        if (Input.mousePosition.x < rect.xMax &&
            Input.mousePosition.x > rect.xMin &&
            Input.mousePosition.y < rect.yMax &&
            Input.mousePosition.y > rect.yMin)
        {
            return true;
        }
        return false;
    }

    void ProcessMouseInput()
    {
        if (!IsControllingGUI)
        {
            if (!IsJumpVector())
            {
                if (CanRecieveMove())
                {
                    CheckDesiredDirection();
                }
            }
            else
            {
                CheckDesiredJumpVector();
            }
        }
    }

    bool CanRecieveMove()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonUp(0))
        {
            m_TriggerTime = 0f;
        }
        if (Input.GetMouseButton(0))
        {
            m_TriggerTime += Time.deltaTime;
        }
        if (m_TriggerTime >= m_TriggerTimeLimit)
        {
            return true;
        }
        return false;
    }

    void CheckDesiredDirection()
    {
        if (CanMouseControl(MouseControllerRect))
        {
            switch (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection)
            {
                case M_PlayerControllerSupport.PlayerSelection.Mark:
                    if (Input.GetMouseButton(0))
                    {
                        M_Motor_Mark.INSTANCE.MoveDirection = PlayerMoveDirection(Input.mousePosition);
                    }
                    break;
                case M_PlayerControllerSupport.PlayerSelection.Mark2:
                    if (Input.GetMouseButton(0))
                    {
                        M_Motor_Mark2.INSTANCE.MoveDirection = PlayerMoveDirection(Input.mousePosition);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void CheckDesiredJumpVector()
    {
        if (MousePoint2.y < MousePoint1.y)
        {
            return;
        }
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
        {
            if (Input.GetMouseButton(0))
            {
                if (CanMouseControl(MouseControllerRect))
                {
                    CanBeJumpVector = true;
                    TempJumpVector = MousePoint2 - MousePoint1;
                    if (TempJumpVector.magnitude > 1)
                    {
                        TempJumpVector = Vector3.Normalize(TempJumpVector);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (CanBeJumpVector)
                {
                    M_Motor_Mark.INSTANCE.CanJump = true;
                    M_Motor_Mark.INSTANCE.JumpVector = TempJumpVector;
                    CanBeJumpVector = false;
                }
            }
        }
    }

    float PlayerMoveDirection(Vector3 pos)
    {
        if (Input.mousePosition.x > MouseControllerRect.xMax / 2)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    bool IsJumpVector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePoint1 = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            MousePoint2 = Input.mousePosition;
        }
        if ((MousePoint2 - MousePoint1).magnitude > 50f)
        {
            return true;
        }
        return false;
    }

    #endregion
}
