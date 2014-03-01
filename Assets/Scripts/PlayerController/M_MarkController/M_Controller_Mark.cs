using UnityEngine;
using System.Collections;

/* クラス説明
     * 
     *      Markのコントローラー
     *      
     *      Edited By   チンカエン
     * */

public class M_Controller_Mark : MonoBehaviour
{
    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static   M_Controller_Mark   INSTANCE;                                                                               //このクラスを実例する
    public static   CharacterController MARK_CHARCONTROLLER;                                                                    //Markのコントローラーを実例する

    #endregion


    #region Function
    
    void Awake()
    {
        MARK_CHARCONTROLLER = GetComponent("CharacterController") as CharacterController;
        INSTANCE = this;
    }

    void Update()
    {
        if (M_GameMain.GAME_PAUSED &&
            !M_Motor_Mark.INSTANCE.PerformingAction &&
            !M_Motor_Mark.INSTANCE.IsDying)
        {
            M_Animator_Mark.INSTANCE.UpdateAnimation();
            return;
        }
        if (!M_Motor_Mark.INSTANCE.IsDying)
        {
            if (!M_Motor_Mark.INSTANCE.PerformingAction)
            {
                M_MousePlayerController.INSTANCE.IsControllingGUI = false;
                if (M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.GRABBING &&
                    M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.CLIMBING)
                {
                    M_Motor_Mark.INSTANCE.MoveVector = Vector3.zero;
                    if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
                    {
                        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
                        {

                            RecievePlayerInput();
                            RecievePlayerActionInput();

                            RecieveMouseInput();
                            RecieveMouseActionInput();
                            RecievePlayerFunctionInput();
                        }
                    }
                    M_Motor_Mark.INSTANCE.UpdateMotion();
                }
                M_Animator_Mark.INSTANCE.UpdateAnimation();
            }
            else
            {
                M_Motor_Mark.INSTANCE.UpdateAction();
                M_Animator_Mark.INSTANCE.UpdateActionAnimation();
            }
        }
        else
        {
            M_Motor_Mark.INSTANCE.UpdateDeath();
            M_Animator_Mark.INSTANCE.UpdateDeathAnimation();
        }
    }

    void LateUpdate()
    {
        if (!M_Motor_Mark.INSTANCE.IsDying)
        {
            JumpEnd();
        }
    }

    void RecievePlayerInput()
    {
        var deadZone = 0.1f;

        M_Motor_Mark.INSTANCE.MoveVector = Vector3.zero;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone)
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.RUNNING;
            M_Motor_Mark.INSTANCE.MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }
    }

    void RecieveMouseInput()
    {
        if (M_Motor_Mark.INSTANCE.MoveDirection == 1)
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.RUNNING;
            M_Motor_Mark.INSTANCE.MoveVector += new Vector3(M_PlayerControllerSupport.INSTANCE.LerpWalkSpeed(0, M_Motor_Mark.INSTANCE.MoveDirection), 0, 0);
        }
        else if (M_Motor_Mark.INSTANCE.MoveDirection == -1)
        {
            M_Animator_Mark.INSTANCE.CurrentMarkAnimeState = M_Animator_Mark.PlayerState.RUNNING;
            M_Motor_Mark.INSTANCE.MoveVector += new Vector3(M_PlayerControllerSupport.INSTANCE.LerpWalkSpeed(0, M_Motor_Mark.INSTANCE.MoveDirection), 0, 0);
        }
    }

    void RecieveMouseActionInput()
    {
        if (M_Motor_Mark.INSTANCE.CanJump)
        {
            if (M_Controller_Mark.MARK_CHARCONTROLLER.enabled)
            {
                M_Motor_Mark.INSTANCE.Jump();
                M_Motor_Mark.INSTANCE.MoveDirection = 0;
                M_Motor_Mark.INSTANCE.MoveVector.x += M_Motor_Mark.INSTANCE.JumpVector.x;
            }
            else
            {
                JumpEnd();
            }
        }
    }

    void JumpEnd()
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            M_Motor_Mark.INSTANCE.JumpVector = Vector3.zero;
            M_Motor_Mark.INSTANCE.CanJump = false;
        }
    }

    void RecievePlayerActionInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            M_Motor_Mark.INSTANCE.Jump();
        }
    }

    public bool RecievePlayerFunctionInput()
    {
        if ((Input.GetKeyDown(KeyCode.F) ||
            Input.GetButtonDown("Use")) &&
            M_Controller_Mark.MARK_CHARCONTROLLER.enabled)
        {
            if (M_Motor_Mark.INSTANCE.Action())
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
