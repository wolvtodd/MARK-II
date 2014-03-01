using UnityEngine;
using System.Collections;

public class M_ClimPointSenser : MonoBehaviour
{
    #region Fields

    public static M_ClimPointSenser INSTANCE;

    public GameObject GrabPoint;
    public GameObject ClimbPoint;
    public GameObject ClimbInterpolatePoint;

    private float m_GrabPointPositionX;
    private float m_ClimbPointPositionX;

    #endregion

    #region Function

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {
        if (M_PlayerClimbSenser_Mark.INSTANCE.CurrentJumpState == M_PlayerClimbSenser_Mark.JumpState.jump)
        {
            Destroy(GrabPoint);
            Destroy(ClimbPoint);
            Destroy(ClimbInterpolatePoint);
            Destroy(this);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "ClimbSenser" &&
            Mathf.Abs(transform.eulerAngles.y -
                      M_PlayerClimbSenser_Mark.INSTANCE.gameObject.transform.eulerAngles.y) < 90f)
        {
            CreateGrabPoint();
            CreateClimbPoint();
            CreateClimbInterpolatePoint();
        }
    }

    float CheckGrabPointPositionX(float position)
    {
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 270)
        {
            position = GrabPoint.transform.localScale.x / 2 + M_Controller_Mark.MARK_CHARCONTROLLER.radius * 2 / 3;
            M_Animator_Mark.INSTANCE.InterpolateRotationWhenGRABBINGFix = M_Animator_Mark.INSTANCE.InterpolateRotationWhenGRABBINGFixLeft;
        }
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 90)
        {
            position = -(GrabPoint.transform.localScale.x / 2 + M_Controller_Mark.MARK_CHARCONTROLLER.radius * 2 / 3);
            M_Animator_Mark.INSTANCE.InterpolateRotationWhenGRABBINGFix = M_Animator_Mark.INSTANCE.InterpolateRotationWhenGRABBINGFixRight;
        }
        return position;
    }


    float CheckClimbPointPositionX(float position)
    {
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 270)
        {
            position = -ClimbPoint.transform.localScale.x;
        }
        if (M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 90)
        {
            position = ClimbPoint.transform.localScale.x;
        }
        return position;
    }


    void CreateGrabPoint()
    {
        GrabPoint = new GameObject("GrabPoint") as GameObject;
        m_GrabPointPositionX = CheckGrabPointPositionX(m_GrabPointPositionX);
        GrabPoint.transform.position = new Vector3(transform.position.x + m_GrabPointPositionX,
                                                   transform.position.y + transform.localScale.y / 2 - M_Controller_Mark.MARK_CHARCONTROLLER.height - 2f,
                                                   transform.position.z);

    }

    void CreateClimbPoint()
    {
        ClimbPoint = new GameObject("ClimbPoint") as GameObject;
        m_ClimbPointPositionX = CheckClimbPointPositionX(m_ClimbPointPositionX);
        ClimbPoint.transform.position = new Vector3(transform.position.x + m_ClimbPointPositionX / 2,
                                                    transform.position.y + transform.localScale.y / 2,
                                                    transform.position.z);
    }

    void CreateClimbInterpolatePoint()
    {
        ClimbInterpolatePoint = new GameObject("ClimbInterpolatePoint") as GameObject;
        ClimbInterpolatePoint.transform.position = new Vector3(GrabPoint.transform.position.x + m_ClimbPointPositionX / 3,
                                                               ClimbPoint.transform.position.y,
                                                               GrabPoint.transform.position.x);
    }

    #endregion
}
