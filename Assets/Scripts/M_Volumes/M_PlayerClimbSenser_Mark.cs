using UnityEngine;
using System.Collections;

public class M_PlayerClimbSenser_Mark : MonoBehaviour
{
    #region Fields

    public static M_PlayerClimbSenser_Mark INSTANCE;

    public enum JumpState
    {
        jump,
        grab,
        climb
    }

    public JumpState CurrentJumpState;

    public float DistanceOfSenserX = 2.5f;
    public float DistanceOfSenserY = 10f;

    private float m_CurrentDistanceX;
    private float m_CurrentDistanceY;

    #endregion

    #region Function

    void Awake()
    {
        INSTANCE = this;
        CurrentJumpState = JumpState.jump;
    }

    void Update()
    {
        CheckSides();
        ProcessPosition();
    }

    void CheckSides()
    {
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 270 &&
            M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            m_CurrentDistanceX = -DistanceOfSenserX;
            m_CurrentDistanceY = DistanceOfSenserY;
        }
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 90 &&
            M_Controller_Mark.MARK_CHARCONTROLLER.isGrounded)
        {
            m_CurrentDistanceX = DistanceOfSenserX;
            m_CurrentDistanceY = DistanceOfSenserY;
        }
    }

    void ProcessPosition()
    {
        transform.position = M_Controller_Mark.MARK_CHARCONTROLLER.transform.position +
                             new Vector3(m_CurrentDistanceX,
                                         m_CurrentDistanceY,
                                         0);
    }
    #endregion
}
