using UnityEngine;
using System.Collections;

public class M_Switch : MonoBehaviour
{
    /* クラス説明
     * 
     *      ジャンプパッドのスイッチ操作。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    public M_JumpPadMain        ParentJumpPad;
    public GameObject           Handler;
    public BoxCollider          SwitchVolume;

    //[SerializeField]
    //private Transform       m_Hint;
    //private Vector3         m_RoundPoint;
    //private float           m_HintTriggerDistance = 5f;

    /* *
     * すべてのparamを宣言します
     * */

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        ParentJumpPad = transform.parent.GetComponent<M_JumpPadMain>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Handler")
            {
                Handler = child.gameObject;
            }
            if (child.gameObject.name == "SwitchPanel")
            {
                SwitchVolume = child.GetComponent<BoxCollider>() as BoxCollider;
            }
        }
    }

    public void UpdateSwitch()
    {
        if (CanActivate() && (M_Controller_Mark.INSTANCE.RecievePlayerFunctionInput() || M_GUIButton_MarkAction.INSTANCE.IsButtonPressed))
        {
            M_Motor_Mark.INSTANCE.SwitchToOperate = this.gameObject.transform.position + new Vector3(-0.75f, 1f, 0f);
            M_Motor_Mark.INSTANCE.PerformingAction = true;
            M_Motor_Mark.INSTANCE.CanPlaySwitchAnimation = true;
            ParentJumpPad.ActiveTrigger = true;
        }
        if (M_Motor_Mark.INSTANCE.HasRotateToActionRot && M_Motor_Mark.INSTANCE.HasMoveToActionPos)
        {
            if (ParentJumpPad.ActiveTrigger)
            {
                CheckActivation();
                ParentJumpPad.ActiveTrigger = false;
            }
        }
        UpdateHandler();
    }

    public bool CanActivate()
    {
        var player = GameObject.FindGameObjectWithTag("Player").transform as Transform;
        var playerPosition = player.position;
        var playerHeight = player.GetComponent<CharacterController>().height;
        var switchPosition = SwitchVolume.transform.position;
        var switchSize = SwitchVolume.size;

        if (M_Controller_Mark.MARK_CHARCONTROLLER.enabled)
        {
            if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
            {
                if (playerPosition.x >= switchPosition.x - switchSize.x &&
                    playerPosition.x <= switchPosition.x + switchSize.x &&
                    playerPosition.y + (playerHeight / 2) >= switchPosition.y &&
                    playerPosition.y + (playerHeight / 2) <= switchPosition.y + switchSize.y)
                {
                    M_Motor_Mark.INSTANCE.CurrentOnSwitch = this.ParentJumpPad;
                    return true;
                }
            }
        }
        return false;
    }

    void CheckActivation()
    {
        if (ParentJumpPad.IsActivated)
        {
            ParentJumpPad.ToDeactive    = true;
        }
        else
        {
            ParentJumpPad.ToActive      = true;
        }
    }

    void UpdateHandler()
    {
        if (ParentJumpPad.ToActive)
        {
            ProcessHandlerWhenActivated();
        }
        else if (ParentJumpPad.ToDeactive)
        {
            ProcessHandlerWhenDectivated();
        }
    }

    //void UpdateHint()
    //{
    //    Vector3 desiredHintPosition     = Vector3.zero;
    //    float distanceBtwSwitchAndMark  = this.transform.position.x - M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x;

    //    if (CanActivate())
    //    {
    //        m_Hint.gameObject.renderer.enabled  = true;
    //        desiredHintPosition.x               = distanceBtwSwitchAndMark;
    //        desiredHintPosition.y               = Mathf.Sin(Mathf.Acos(desiredHintPosition.x / m_HintTriggerDistance)) * m_HintTriggerDistance;
    //        desiredHintPosition                *= 1 - M_MainCamera.INSTANCE.ZoomRate;
    //        desiredHintPosition.z               = -5.1f;
    //        m_Hint.position                     = m_RoundPoint + desiredHintPosition;
    //    }
    //    else
    //    {
    //        m_Hint.gameObject.renderer.enabled = false;
    //    }
    //}

    void ProcessHandlerWhenActivated()
    {
        if (Handler.transform.eulerAngles.y % 180 >= 0f && Handler.transform.eulerAngles.x >= 350f)
        {
            Handler.transform.eulerAngles       = new Vector3(0f, 180f, 0f);
            ParentJumpPad.IsActivated = true;
            ParentJumpPad.ToActive = false;
            return;
        }
        Handler.transform.Rotate(new Vector3(360 * Time.deltaTime, 0, 0));
    }

    void ProcessHandlerWhenDectivated()
    {
        if (Mathf.Abs(Handler.transform.eulerAngles.y) <= 1f && Handler.transform.eulerAngles.x >= 350f)
        {
            Handler.transform.eulerAngles       = new Vector3(0f, 0f, 180f);
            ParentJumpPad.IsActivated = false;
            ParentJumpPad.ToDeactive = false;
            return;
        }
        Handler.transform.Rotate(new Vector3(-360 * Time.deltaTime, 0, 0));
    }


    #endregion
}
