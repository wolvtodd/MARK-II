using UnityEngine;
using System.Collections;

public class M_ActivateClimbPointSenser : MonoBehaviour
{
    #region Fields

    public static M_ActivateClimbPointSenser INSTANCE;

    #endregion

    #region Function

    void Awake()
    {
        INSTANCE = this;
        
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "ClimbSenser" &&
            Mathf.Abs(transform.eulerAngles.y -
                      M_PlayerClimbSenser_Mark.INSTANCE.gameObject.transform.eulerAngles.y) < 90f)
        {
            gameObject.AddComponent("M_ClimPointSenser");
            M_PlayerClimbSenser_Mark.INSTANCE.CurrentJumpState = M_PlayerClimbSenser_Mark.JumpState.grab;
        }
    }

    #endregion
}
