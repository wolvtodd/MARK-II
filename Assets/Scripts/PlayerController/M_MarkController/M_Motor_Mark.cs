using UnityEngine;
using System.Collections;

public class M_Motor_Mark : MonoBehaviour
{
    /* クラス説明
     * 
     *      Mark実際の動き調整。
     *      
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_Motor_Mark INSTANCE;                                                    //このクラスを実例する

    public float            PlayerSpeed                 = 14f;                              //Markの歩くスピード
    public float            VelCharY                    = 0.1f;                             //転向するときのスムーズスピード
    public float            CharYSmooth                 = 0.15f;                            //転向時間
    public float            Gravity                     = 98f;                              //重力
    public float            JumpSpeed                   = 28f;                              //ジャンプ力
    public float            RotationYtoTurnTo           = 90f;                              //Markは何の方向に向かっている
    public float            VerticalSpeed               = -1f;                              //ディフォルトのY方向のスピード
    public float            MoveDirection               = 0f;
    public bool             IsDying                     = false;
    public bool             CanPlayDeathAnimation       = true;
    public bool             CheckClimb                  = false;                            //Markは今登られるかどうか
    public bool             CanJump                     = false;
    public bool             HasMoveToActionPos          = false;
    public bool             HasRotateToActionRot        = false;
    public bool             PerformingAction            = false;
    public bool             CanPlaySwitchAnimation      = false;
    public Vector3          MoveVector                  = Vector3.zero;                                                                                         //Markの移動方向とスピード
    public Vector3          DesiredPosition             = Vector3.zero;
    public Vector3          JumpVector                  = Vector3.zero;
    public Vector3          MarkPositionBefore          = Vector3.zero;
    public Vector3          MarkPositionAfter           = Vector3.zero;
    public Vector3          SwitchToOperate             = Vector3.zero;
    public M_JumpPadMain    CurrentOnSwitch;

    public bool             CanDoAction                   = false;

    private Vector3 m_TempMoveRotation          = Vector3.zero;                                                                                   //臨時用転向角度

    #endregion


    #region Function
    
    void Awake()
    {
        INSTANCE = this;
    }

    public void UpdateMotion()                                                                                          //これを「M_Controller_Mark」の「Update」ファンクションに使う
    {
        if (CurrentOnSwitch != null)
        {
            CheckIfCanAction();
        }
        ProcessMotion();
        Turns();
    }

    void ProcessMotion()
    {
        MoveDirection = 0;
        if (M_GameMain.INSTANCE.StartPointForMark != null)
        {
            WalkToStartPoint();
        }
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.GameClear)
        {
            WalkToEndPoint();
        }
        if (MoveVector.magnitude > 1)
            MoveVector = Vector3.Normalize(MoveVector);                                                                 //Markの移動速度を単位化する

        MoveVector *= PlayerSpeed;                                                                                      //移動速度を掛ける
        ApplyGravity();                                                                                                 //重力を加算する
        M_Controller_Mark.MARK_CHARCONTROLLER.Move(MoveVector * Time.deltaTime);                                        //最後の移動速度でドライブする
    }

    public bool WalkToStartPoint()
    {
        var markPos     = M_Controller_Mark.MARK_CHARCONTROLLER.transform.position;
        var startPos    = M_GameMain.INSTANCE.StartPointForMark;

        if (startPos == null)
        {
            return true;
        }

        M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.RUNNING;
        MoveVector.x += 1;
        if (markPos.x >= startPos.transform.position.x)
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.IDLE;
            MoveVector = Vector3.zero;
            Destroy(M_GameMain.INSTANCE.StartPointForMark.gameObject);
            return true;
        }
        return false;
    }

    public bool WalkToEndPoint()
    {
        var markPos = M_Controller_Mark.MARK_CHARCONTROLLER.transform.position;
        var endPos = M_GameMain.INSTANCE.EndPointForMarks;

        if (markPos.x >= endPos.transform.position.x)
        {
            return true;
        }
        M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.RUNNING;
        MoveVector.x += 1;
        return false;
    }

    void ApplyGravity()
    {
        VerticalSpeed -= Gravity * Time.deltaTime;                                                                      //Y方向で重力を減算する
        MoveVector.y = VerticalSpeed;                                                                                   //Y方向のスピードになる

        if (!M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded &&
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.JUMPING)
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.FALLING;                          //地面に立ってない時、アニメ状態を確認する
        }
        
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            if (M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.RUNNING &&
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.STOP_RUNNING &&
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.LANDING &&
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.JUMPING &&
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.FALLING)
            {
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState  = M_Animator_Mark.PlayerState.IDLE;                         //地面に立っているとき、アニメ状態を確認する
            }
            if (VerticalSpeed < -1)
                VerticalSpeed = -1;                                                                                     //Y方向で速度単位化
        }
    }

    void Turns()                                                                                                        //Markの地面状態と速度を確認した後に実行する
    {
        CheckTurns();
        InterpolateRotation();
    }

    void CheckTurns()                                                                                                   //地面での転向方向を確認する
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            if (MoveVector.x < 0)
                RotationYtoTurnTo = 270;
            if (MoveVector.x > 0)
                RotationYtoTurnTo = 90;
        }
    }

    void InterpolateRotation()                                                                                          //Markを正しい方向に転向する
    {
        m_TempMoveRotation.y = Mathf.SmoothDamp(transform.eulerAngles.y,
                                                RotationYtoTurnTo,
                                                ref VelCharY,
                                                CharYSmooth);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                                              m_TempMoveRotation.y,
                                              transform.eulerAngles.z);
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            if (m_TempMoveRotation.y > 120 && m_TempMoveRotation.y < 240)
            {
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.TURNING;                      //転向するときのアニメ状態を確認する
            }
        }
    }

    public void UpdateAction()
    {
        if (!HasMoveToActionPos || !HasRotateToActionRot)
            LerpMarkToAction();
        else
        {
            if (!M_Animator_Mark.INSTANCE.UpdateActionAnimation())
            {
                LerpToOriginalRotation();
            }
        }
    }

    void LerpMarkToAction()
    {
        M_Controller_Mark.MARK_CHARCONTROLLER.enabled = false;
        LerpToPerformActionPosition();
        LerpToPerformActionRotation();
    }

    void LerpToPerformActionPosition()
    {
        float tempPositionX;
        tempPositionX = Mathf.Lerp(transform.position.x, SwitchToOperate.x, 5f * Time.deltaTime);
        transform.position = new Vector3(tempPositionX, transform.position.y, transform.position.z);
        if (Mathf.Abs(tempPositionX - SwitchToOperate.x) <= 0.1f)
        {
            transform.position = new Vector3(SwitchToOperate.x, transform.position.y, transform.position.z);
            HasMoveToActionPos = true;
        }
    }

    void LerpToPerformActionRotation()
    {
        float destinationEuler;
        float tempEulerY;

        if (RotationYtoTurnTo == 270f)
        {
            destinationEuler = 360f;
            if (transform.eulerAngles.y >= 0f && transform.eulerAngles.y <= 5f)
            {
                destinationEuler = 0f;
            }
        }
        else
        {
            destinationEuler = 0f;
        }
        tempEulerY = Mathf.Lerp(transform.eulerAngles.y, destinationEuler, 10f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, tempEulerY, 0);
        if (Mathf.Abs(transform.eulerAngles.y) <= 0.5f || Mathf.Abs(transform.eulerAngles.y - 360) <= 0.5f)
        {
            transform.eulerAngles = Vector3.zero;
            HasRotateToActionRot = true;
        }
    }

    void LerpToOriginalRotation()
    {
        float destinationEuler;
        float tempEulerY;
        
        if (RotationYtoTurnTo == 270f)
        {
            destinationEuler = -90f;
            if (transform.eulerAngles.y <=360f && transform.eulerAngles.y >= 270f)
            {
                destinationEuler = 270f;
            }
        }
        else
        {
            destinationEuler = 90f;
        }
        tempEulerY = Mathf.Lerp(transform.eulerAngles.y, destinationEuler, 7f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, tempEulerY, 0);
        if (Mathf.Abs(transform.eulerAngles.y - RotationYtoTurnTo) <= 0.5f)
        {
            transform.eulerAngles       = new Vector3(0, RotationYtoTurnTo, 0);
            SwitchToOperate             = Vector3.zero;
            HasMoveToActionPos          = false;
            HasRotateToActionRot        = false;
            PerformingAction            = false;
            CanPlaySwitchAnimation      = false;
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.IDLE;
            animation.Play("idle");
            M_Controller_Mark.MARK_CHARCONTROLLER.enabled = true;
        }
    }

    public void UpdateDeath()
    {
        M_Controller_Mark.MARK_CHARCONTROLLER.enabled = false;
        if (RotationYtoTurnTo == 270)
        {
            transform.eulerAngles = new Vector3(-15f, 265f, 0f);  
        }
        else
        {
            transform.eulerAngles = new Vector3(-15f, 95f, 0f);
        }
    }

    public void Jump()                                                                                                  //Markのジャンプファンクション
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)                                                           //地面に立ってしかないときにジャンプできる
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.JUMPING;
            VerticalSpeed = JumpSpeed;                                                                                  //ジャンプスピードを乗算
        }
    }

    void CheckIfCanAction()
    {
        if (CurrentOnSwitch.GetComponentInChildren<M_Switch>().CanActivate())
        {
            CanDoAction = true;
        }
        else
        {
            CanDoAction = false;
        }
    }

    public bool Action()
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded &&
            M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
        {
            if (M_Animator_Mark.INSTANCE.CurrentMarkAnimeState == M_Animator_Mark.PlayerState.IDLE ||
                M_Animator_Mark.INSTANCE.CurrentMarkAnimeState == M_Animator_Mark.PlayerState.RUNNING)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
