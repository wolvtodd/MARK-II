using UnityEngine;
using System.Collections;

public class M_Motor_Mark2 : MonoBehaviour
{
    #region Fields

    public static M_Motor_Mark2 INSTANCE;

    public bool IsMark2Attacking        = false;

    private float PlayerSpeed            = 10f;
    public float PlayerJumpMovingSpeed  = 1f;
    public float Gravity                = 200f;
    public float JumpSpeed              = 90f;
    public float VelCharY               = 0.1f;
    public float CharYSmooth            = 0.5f;
    public float VerticalSpeed          = 0f;
    public float RotationYtoTurnTo      = 270f;
    public float MoveDirection          = 0f;
    public int   CharDirection          = 0;
    public Vector3 MoveVector           = Vector3.zero;
    public Vector3 DesiredPosition      = Vector3.zero;
    public Vector3 Mark2PositionBefore  = Vector3.zero;
    public Vector3 Mark2PositionAfter   = Vector3.zero;
    public M_JumpPadMain CurrentOnJumpPad;

    private Vector3 m_TempMoveRotation  = Vector3.zero;

    #endregion



    #region Function

    void Awake()
    {
        INSTANCE = this;
    }

    public void UpdateMotion()
    {
        ProcessMotion();
        Turns();
    }

    void ProcessMotion()
    {
        MoveDirection = 0;
        if (M_GameMain.INSTANCE.StartPointForMark2 != null)
        {
            WalkToStartPoint();
        }
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.GameClear)
        {
            WalkToEndPoint();
        }
        if (MoveVector.magnitude > 1)
        {
            MoveVector = Vector3.Normalize(MoveVector);
        }
        MoveVector *= PlayerSpeed;
        if (CurrentOnJumpPad != null)
        {
            MoveVector.x += CurrentOnJumpPad.Mark2AddOnSpeed;
        }
        ApplyGravity();
        M_Controller_Mark2.MARK2_CHARCONTROLLER.Move(MoveVector * Time.deltaTime);
    }

    public bool WalkToStartPoint()
    {
        var mark2Pos = M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position;
        var startPos = M_GameMain.INSTANCE.StartPointForMark2;

        if (startPos == null)
        {
            return true;
        }

        M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee = M_Animator_Mark2.PlayerAnimationState.RUN;
        MoveVector.x += 1;
        if (mark2Pos.x >= startPos.transform.position.x)
        {
            M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee = M_Animator_Mark2.PlayerAnimationState.IDLE;
            MoveVector = Vector3.zero;
            Destroy(M_GameMain.INSTANCE.StartPointForMark2.gameObject);
            return true;
        }
        return false;
    }

    public bool WalkToEndPoint()
    {
        var mark2Pos = M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position;
        var endPos = M_GameMain.INSTANCE.EndPointForMarks;
        
        if (mark2Pos.x >= endPos.transform.position.x)
        {
            return true;
        }
        M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee = M_Animator_Mark2.PlayerAnimationState.RUN;
        MoveVector.x += 1;
        return false;
    }

    void ApplyGravity()
    {
        VerticalSpeed -= Gravity * Time.deltaTime;
        MoveVector.y = VerticalSpeed;

        if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
        {
            if (VerticalSpeed < -1)
                VerticalSpeed = -1;
        }
    }

    void Turns()
    {
        CheckTurns();
        InterpolateRotation();
    }

    void CheckTurns()
    {
        var currentRotationY = transform.eulerAngles.y;

        if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
        {
            if (MoveVector.x < 0)
            {
                RotationYtoTurnTo = 270;
            }
            else if (MoveVector.x > 0)
            {
                RotationYtoTurnTo = 90;
            }
        }

        if (currentRotationY > 180)
        {
            CharDirection = -1;
        }
        else
        {
            CharDirection = 1;
        }
    }

    void InterpolateRotation()
    {

        m_TempMoveRotation.y = Mathf.SmoothDamp(transform.eulerAngles.y,
                                                RotationYtoTurnTo,
                                                ref VelCharY,
                                                CharYSmooth);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                                              m_TempMoveRotation.y,
                                              transform.eulerAngles.z);
        if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
        {
            if (m_TempMoveRotation.y > 120 && m_TempMoveRotation.y < 240)
            {
                M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee = M_Animator_Mark2.PlayerAnimationState.TURNING;
            }
        }
    }

    public bool CanAttack()
    {
        if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded &&
            M_Animator_Mark2.INSTANCE.CurrentMark2AnimeStatee != M_Animator_Mark2.PlayerAnimationState.TURNING)
        {
            return true;
        }
        return false;
    }

    #endregion
}
