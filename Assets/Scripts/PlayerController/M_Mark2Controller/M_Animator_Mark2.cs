using UnityEngine;
using System.Collections;

public class M_Animator_Mark2 : MonoBehaviour
{
    /* クラス説明
     * 
     *      Mark-2のアニメーションと動きの調整
     *      アニメーションに関する移動など
     *      
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_Animator_Mark2 INSTANCE;

    public enum PlayerAnimationState
    {
        STAND_BY,
        IDLE,
        RUN,
        FALL,
        LANDING,
        ATTACK,
        TURNING
    }
    public PlayerAnimationState CurrentMark2AnimeStatee;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
        CurrentMark2AnimeStatee = PlayerAnimationState.STAND_BY;
    }

    public void UpdateAnimation()
    {
        ProcessCurrentAnimationStatus();
        ProcessCurrentAnimationClip();
    }

    void ProcessCurrentAnimationStatus()
    {
        if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
        {
            if (CurrentMark2AnimeStatee != PlayerAnimationState.FALL &&
                CurrentMark2AnimeStatee != PlayerAnimationState.ATTACK &&
                CurrentMark2AnimeStatee != PlayerAnimationState.LANDING &&
                CurrentMark2AnimeStatee != PlayerAnimationState.TURNING &&
                CurrentMark2AnimeStatee != PlayerAnimationState.STAND_BY)
            {
                if (M_Motor_Mark2.INSTANCE.MoveVector.x != 0)
                {
                    CurrentMark2AnimeStatee = PlayerAnimationState.RUN;
                }
                else
                {
                    CurrentMark2AnimeStatee = PlayerAnimationState.IDLE;
                }
            }
        }
        else
            CurrentMark2AnimeStatee = PlayerAnimationState.FALL;
    }

    void ProcessCurrentAnimationClip()
    {
        if (M_GameMain.GAME_PAUSED)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 0.0f;
            }
            return;
        }
        if (CurrentMark2AnimeStatee == PlayerAnimationState.IDLE)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.0f;
            }
            animation.CrossFade("idle");
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.RUN)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.5f;
            }
            animation.CrossFade("walking");
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.ATTACK)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.0f;
            }
            animation.Play("hit");
            CurrentMark2AnimeStatee = PlayerAnimationState.STAND_BY;
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.TURNING)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.5f;
            }
            animation.CrossFade("walking");
            if (M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.eulerAngles.y > 240 || M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.eulerAngles.y < 120)
                CurrentMark2AnimeStatee = PlayerAnimationState.IDLE;
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.FALL)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.0f;
            }
            animation.CrossFade("fall");
            if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
                CurrentMark2AnimeStatee = PlayerAnimationState.LANDING;
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.LANDING)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.0f;
            }
            animation.CrossFade("landing", 0.1f);
            CurrentMark2AnimeStatee = PlayerAnimationState.STAND_BY;
        }
        else if (CurrentMark2AnimeStatee == PlayerAnimationState.STAND_BY)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 1.0f;
            }
            if (!animation.isPlaying)
            {
                CurrentMark2AnimeStatee = PlayerAnimationState.IDLE;
                M_Motor_Mark2.INSTANCE.IsMark2Attacking = false;
                M_MousePlayerController.INSTANCE.IsControllingGUI = false;
            }
        }
    }

    #endregion
}
