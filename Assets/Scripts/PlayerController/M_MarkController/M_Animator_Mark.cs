using UnityEngine;
using System.Collections;

/* クラス説明
 * 
 *      Markのアニメーションと動きの調整
 *      アニメーションに関する移動など
 * 
 *      Edited By   チンカエン
 * */

public class M_Animator_Mark : MonoBehaviour
{
    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_Animator_Mark INSTANCE;                                                                              //このクラスの実例

    public enum PlayerState                                                                                         //キャラクターのアニメーションの状態
    {
        IDLE,                                                                                                       //PlayerState: 立つ
        RUNNING,                                                                                                    //PlayerState: 走る
        STOP_RUNNING,                                                                                               //PlayerState: 走る停止
        JUMPING,                                                                                                    //PlayerState: ジャンプ
        FALLING,                                                                                                    //PlayerState: 落ちる
        LANDING,                                                                                                    //PlayerState: 落地
        GRABBING,
        CLIMBING,
        TURNING,
    }
    public PlayerState CurrentMarkAnimeState;                                                                          //PlayerStateを実例します
	
	public enum CheckIfHasLANDING
	{
		ON_GROUND,
		JUMPING,
		CLIMBING
	}
	public CheckIfHasLANDING CurrentCheckIfHasLANDING;

    public float InterpolateRotationWhenGRABBINGFix;
    public float InterpolateRotationWhenGRABBINGFixLeft = 270f;
    public float InterpolateRotationWhenGRABBINGFixRight = 90f;

    public float VelPlayerPosWhenGRABBINGSmoothX = 0.05f;
    public float PlayerPosWhenGRABBINGSmoothX = 0.05f;

    public float VelPlayerPosWhenGRABBINGSmoothY = 0.05f;
    public float PlayerPosWhenGRABBINGSmoothY = 0.05f;

    public float VelPlayerRotWhenGRABBINGSmoothY = 0.05f;
    public float PlayerRotWhenGRABBINGSmoothY = 0.05f;

    public float VelPlayerPosWhenCLIMBINGSmoothX = 0.05f;
    public float PlayerPosWhenCLIMBINGSmoothX = 0.25f;

    public float VelPlayerPosWhenCLIMBINGSmoothY = 0.01f;
    public float PlayerPosWhenCLIMBINGSmoothY = 0.55f;

    public float VelPlayerPosWhenClimbOnSmoothX = 0.01f;
    public float PlayerPosWhenClimbOnSmoothX = 0.1f;

    public float VelPlayerPosWhenClimbOnSmoothY = 0.01f;
    public float PlayerPosWhenClimbOnSmoothY = 0.1f;

    public float ClimbMarchFixWhenClimbOn = 0.5f;

    private Vector3 m_CurrentPlayerPosWhenGRABBING;
    private Vector3 m_CurrentPlayerRotWhenGRABBING;
    private Vector3 m_CurrentPlayerPosWhenCLIMBING;
    private Vector3 m_CurrentPlayerPosWhenClimbOn;

    private GameObject m_MarkDeathSmoke;
    #endregion

    #region Function


    /* *
     * 初期化に関するメソッド
     * */

    void Start()
    {
        INSTANCE                    = this;                                                                                            //このクラスを実例します
        CurrentMarkAnimeState       = PlayerState.IDLE;                                                                      //キャラクターのディフォルト状態
        CurrentCheckIfHasLANDING    = CheckIfHasLANDING.ON_GROUND;
        m_MarkDeathSmoke            = Resources.Load("Prefabs/Particles/Mark_DeadSmoke") as GameObject;
    }

    /* *
     * アニメーションに関するメソッド
     * */

    //キャラクターのアニメーションを再生します
    public void UpdateAnimation()                                                                                         
    {
        if (M_GameMain.GAME_PAUSED)
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 0.0f;
            }
            return;
        }
        foreach (AnimationState state in animation)
        {
            state.speed = 1.0f;
        }
        CheckIfGrab();
        JudgeCurrentAnimation();
    }

    public bool UpdateActionAnimation()
    {   
        if (M_Motor_Mark.INSTANCE.CanPlaySwitchAnimation)
        {
            if (!M_Motor_Mark.INSTANCE.CurrentOnSwitch.IsActivated)
            {
                animation.CrossFade("activate", 0.5f);
            }
            else
            {
                animation.CrossFade("deactivate", 0.5f);
            }
            M_Motor_Mark.INSTANCE.CanPlaySwitchAnimation = false;
        }
        if (!animation.isPlaying)
        {
            return false;
        }
        return true;
    }

    public void UpdateDeathAnimation()
    {
        var markRenderer                = GameObject.Find("Mark-body").renderer;
        var tempColor                   = Mathf.Lerp(markRenderer.material.color.r, 0, 5 * Time.deltaTime);
        
        markRenderer.material.color     = new Color(tempColor, tempColor, tempColor, 1);
        
        if (M_Motor_Mark.INSTANCE.CanPlayDeathAnimation)
        {
            StartCoroutine(Death());
            animation.Play("electricShock");
            M_Motor_Mark.INSTANCE.CanPlayDeathAnimation = false;
        }
    }

    IEnumerator Death()
    {
        var markRenderer                = GameObject.Find("Mark-body").renderer;
        yield return new WaitForSeconds(1f);
        markRenderer.enabled = false;
        GameObject.Instantiate(m_MarkDeathSmoke, this.transform.position, Quaternion.identity);
    }

    //キャラクターのアニメーション選択器
    void JudgeCurrentAnimation()                                                                                    
    {
        switch (CurrentMarkAnimeState)
        {
            case PlayerState.IDLE:
                IDLE();
                break;
            case PlayerState.RUNNING:
                RUNNING();
                break;
            case PlayerState.JUMPING:
                Jump();
                break;
            case PlayerState.FALLING:
                Fall();
                break;
            case PlayerState.GRABBING:
                Grab();
                break;
            case PlayerState.CLIMBING:
                Climb();
                break;
            case PlayerState.TURNING:
                Turn();
                break;
            default:
                break;
        }
    }

    //立つアニメーションに関する動作
    void IDLE()                                                                                                     
    {
		CurrentCheckIfHasLANDING = CheckIfHasLANDING.ON_GROUND; 
        animation.CrossFade("idle", 0.2f);                                                                                //立つアニメーションを再生します
    }

    //走るアニメーションに関する動作
    void RUNNING()                                                                                                  
    {
        CurrentCheckIfHasLANDING = CheckIfHasLANDING.ON_GROUND; 
        animation.CrossFade("running");                                                                             //走るアニメーションを再生します
        if (M_Motor_Mark.INSTANCE.MoveVector.x != 0)
            CurrentMarkAnimeState = PlayerState.IDLE;                                                                  //いまのキャラクター状態をIDLEとなります
    }

    //ジャンプアニメーションに関する動作
    void Jump()                                                                                                     
    {
		CurrentCheckIfHasLANDING = CheckIfHasLANDING.JUMPING;
        CurrentMarkAnimeState = PlayerState.FALLING;                                                                   //今のキャラクター状態をFALLINGとなります
    }

    //落ちるアニメーションに関する動作
    void Fall()                                                                                                     
    {
        if (!M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded &&                                                              //キャラクターは地面をタッチしていない状態
            CurrentMarkAnimeState != PlayerState.JUMPING)                                                              //いまのキャラクター状態はJUMPINGではないなら
        {
            animation.CrossFade("fall");                                                                            //落ちるアニメーションを再生します
        }
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)                                                              //今のキャラクター状態はFALLINGではないなら
        {
			if (CurrentCheckIfHasLANDING == CheckIfHasLANDING.JUMPING ||
                CurrentCheckIfHasLANDING == CheckIfHasLANDING.ON_GROUND)
			{
            	CurrentMarkAnimeState = PlayerState.LANDING;
          		animation.Play("landing");
			}
			if(CurrentCheckIfHasLANDING == CheckIfHasLANDING.CLIMBING)
			{
				CurrentMarkAnimeState = PlayerState.IDLE;
			}
        }
        if (M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded &&
            CurrentMarkAnimeState == PlayerState.LANDING)
            CurrentMarkAnimeState = PlayerState.IDLE;
    }

    void Grab()
    {
		CurrentCheckIfHasLANDING = CheckIfHasLANDING.CLIMBING;
        M_Controller_Mark.MARK_CHARCONTROLLER.enabled = false;
        animation.CrossFade("hanging",0.15f);
        InterpolatePlayerPositionWhenGrab();
        InterpolatePlayerRotationWhenGrab();
		if(transform.position == M_ClimPointSenser.INSTANCE.GrabPoint.transform.position)
        {
            M_PlayerClimbSenser_Mark.INSTANCE.CurrentJumpState = M_PlayerClimbSenser_Mark.JumpState.climb;
            CurrentMarkAnimeState = PlayerState.CLIMBING;
        }
    }

    void Climb()
    {
        animation.CrossFade("climbOn");
        if (transform.position.y < M_ClimPointSenser.INSTANCE.ClimbInterpolatePoint.transform.position.y - 2.7f)
        {
            InterpolatePlayerPositionWhenClimb();
        }
        else
            InterpolatePlayerPositionWhenClimbOn();
        if (Mathf.Abs(transform.position.x - M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.x) < ClimbMarchFixWhenClimbOn &&
            Mathf.Abs(transform.position.y - M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.y) < ClimbMarchFixWhenClimbOn)
        {
            transform.position                                  = M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position + Vector3.up * 0.1f;
            M_Motor_Mark.INSTANCE.VerticalSpeed                 = -1;
            M_PlayerClimbSenser_Mark.INSTANCE.CurrentJumpState  = M_PlayerClimbSenser_Mark.JumpState.jump;
            CurrentMarkAnimeState                               = PlayerState.IDLE;
            M_Motor_Mark.INSTANCE.JumpVector                    = Vector3.zero;
            M_Controller_Mark.MARK_CHARCONTROLLER.enabled       = true;
        }
    }

    void Turn()
    {
        animation.CrossFade("running");
    }

    void CheckIfGrab()
    {
        if (M_PlayerClimbSenser_Mark.INSTANCE.CurrentJumpState == M_PlayerClimbSenser_Mark.JumpState.grab)
        {
            CurrentMarkAnimeState = PlayerState.GRABBING;
        }
    }

    void InterpolatePlayerPositionWhenGrab()
    {
        m_CurrentPlayerPosWhenGRABBING.x = Mathf.SmoothDamp(transform.position.x,
                                                            M_ClimPointSenser.INSTANCE.GrabPoint.transform.position.x,
                                                            ref VelPlayerPosWhenGRABBINGSmoothX,
                                                            PlayerPosWhenGRABBINGSmoothX);
        m_CurrentPlayerPosWhenGRABBING.y = Mathf.SmoothDamp(transform.position.y,
                                                            M_ClimPointSenser.INSTANCE.GrabPoint.transform.position.y,
                                                            ref VelPlayerPosWhenGRABBINGSmoothY,
                                                            PlayerPosWhenGRABBINGSmoothY);
        m_CurrentPlayerPosWhenGRABBING.z = 0f;
        transform.position = m_CurrentPlayerPosWhenGRABBING;
    }

    void InterpolatePlayerRotationWhenGrab()
    {
        m_CurrentPlayerRotWhenGRABBING.y = Mathf.SmoothDamp(transform.eulerAngles.y,
                                                            M_Motor_Mark.INSTANCE.RotationYtoTurnTo,
                                                            ref VelPlayerRotWhenGRABBINGSmoothY,
                                                            PlayerRotWhenGRABBINGSmoothY);
        transform.eulerAngles = m_CurrentPlayerRotWhenGRABBING;
    }

    void InterpolatePlayerPositionWhenClimb()
    {
        m_CurrentPlayerPosWhenCLIMBING.x = Mathf.SmoothDamp(transform.position.x,
                                                            M_ClimPointSenser.INSTANCE.ClimbInterpolatePoint.transform.position.x,
                                                            ref VelPlayerPosWhenCLIMBINGSmoothX,
                                                            PlayerPosWhenCLIMBINGSmoothX);
        m_CurrentPlayerPosWhenCLIMBING.y = Mathf.SmoothDamp(transform.position.y,
                                                            M_ClimPointSenser.INSTANCE.ClimbInterpolatePoint.transform.position.y,
                                                            ref VelPlayerPosWhenCLIMBINGSmoothY,
                                                            PlayerPosWhenCLIMBINGSmoothY);
        m_CurrentPlayerPosWhenCLIMBING.z = 0f;
        transform.position = m_CurrentPlayerPosWhenCLIMBING;
    }

    void InterpolatePlayerPositionWhenClimbOn()
    {
        m_CurrentPlayerPosWhenClimbOn.x = Mathf.SmoothDamp(transform.position.x,
                                                           M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.x,
                                                           ref VelPlayerPosWhenClimbOnSmoothX,
                                                           PlayerPosWhenClimbOnSmoothX);
        m_CurrentPlayerPosWhenClimbOn.y = Mathf.SmoothDamp(transform.position.y,
                                                           M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.y,
                                                           ref VelPlayerPosWhenClimbOnSmoothY,
                                                           PlayerPosWhenClimbOnSmoothY);
        m_CurrentPlayerPosWhenClimbOn.z = 0f;
        transform.position = m_CurrentPlayerPosWhenClimbOn;
    }

    #endregion
}
