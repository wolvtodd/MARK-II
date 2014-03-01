using UnityEngine;
using System.Collections;

public class M_Controller_Mark2 : MonoBehaviour
{
    #region Fields

    public static M_Controller_Mark2    INSTANCE;
    public static CharacterController   MARK2_CHARCONTROLLER;

    #endregion


    #region Function

    void Awake()
    {
        MARK2_CHARCONTROLLER = GetComponent("CharacterController") as CharacterController;
        INSTANCE = this;
    }

    void Update()
    {
        if (M_GameMain.GAME_PAUSED)
        {
            M_Animator_Mark2.INSTANCE.UpdateAnimation();
            return;
        }

        if (MARK2_CHARCONTROLLER.isGrounded)
        {
            M_Motor_Mark2.INSTANCE.MoveVector = Vector3.zero;
        }
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
        {
            if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
            {
                RecievePlayerInput();
                RecievePlayerActionInput();
                RecieveMouseInput();
            }
        }
        M_Motor_Mark2.INSTANCE.UpdateMotion();
        M_Animator_Mark2.INSTANCE.UpdateAnimation();
    }

    void RecievePlayerInput()
    {
        var deadZone = 0.1f;

        if (MARK2_CHARCONTROLLER.isGrounded)
        {
            M_Motor_Mark2.INSTANCE.MoveVector = Vector3.zero;
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone)
        {
            if (M_Motor_Mark2.INSTANCE.IsMark2Attacking == false)
            {
                M_Motor_Mark2.INSTANCE.MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            }
        }
    }

    void RecieveMouseInput()
    {
        if (M_Motor_Mark2.INSTANCE.MoveDirection == 1)
        {
            if (M_Motor_Mark2.INSTANCE.IsMark2Attacking == false)
            {
                M_Motor_Mark2.INSTANCE.MoveVector += new Vector3(M_PlayerControllerSupport.INSTANCE.LerpWalkSpeed(0, M_Motor_Mark2.INSTANCE.MoveDirection), 0, 0);
            }
        }
        else if (M_Motor_Mark2.INSTANCE.MoveDirection == -1)
        {
            if (M_Motor_Mark2.INSTANCE.IsMark2Attacking == false)
            {
                M_Motor_Mark2.INSTANCE.MoveVector += new Vector3(M_PlayerControllerSupport.INSTANCE.LerpWalkSpeed(0, M_Motor_Mark2.INSTANCE.MoveDirection), 0, 0);
            }
        }
    }

    void RecievePlayerActionInput()
    {
        if ((Input.GetKeyDown(KeyCode.F) ||
            Input.GetButtonDown("Use") ||
            M_GUIButton_Mark2Action.INSTANCE.IsButtonPressed) &&
            M_Motor_Mark2.INSTANCE.CanAttack())
        {
            M_Motor_Mark2.INSTANCE.IsMark2Attacking = true;
            M_Motor_Mark2.INSTANCE.MoveDirection = 0;
            M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee = M_Animator_Mark2.PlayerAnimationState.ATTACK;
        }
    }

    #endregion
}
