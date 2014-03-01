using UnityEngine;
using System.Collections;

public class M_GUIPlayerSwitchPad : MonoBehaviour
{
    /* クラス説明
     * 
     *      マルチタッチ用操作パッド。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static  M_GUIPlayerSwitchPad INSTANCE;
    
    public BoxCollider  SwitchVolume;
    public Vector2      MousePoint1 = Vector2.zero;
    public Vector2      MousePoint2 = Vector2.zero;
    public bool         CanSwitch   = false;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        INSTANCE = this;
        SwitchVolume = GetComponent<BoxCollider>() as BoxCollider;
        CanSwitch = false;
    }

    void Update()
    {
        if (M_Controller_Mark.MARK_CHARCONTROLLER == null ||
            M_Controller_Mark2.MARK2_CHARCONTROLLER == null)
        {
            return;
        }
        if (CanSwitchPlayer())
        {
            SwitchPlayer();
        }
    }

    bool CanDrag()
    {
        var mousePos = M_GUICamera.INSTANCE.camera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < SwitchVolume.transform.position.x + SwitchVolume.transform.localScale.x * 2.5f &&
            mousePos.x > SwitchVolume.transform.position.x - SwitchVolume.transform.localScale.x * 2.5f &&
            mousePos.y < SwitchVolume.transform.position.y + SwitchVolume.transform.localScale.y * 2.5f &&
            mousePos.y > SwitchVolume.transform.position.y - SwitchVolume.transform.localScale.y * 2.5f)
        {
            return true;
        }
        return false;
    }

    bool CanSwitchPlayer()
    {
        if (CanDrag())
        {
            if (Input.GetMouseButtonDown(0))
            {
                CanSwitch = true;
                MousePoint1 = M_GUICamera.INSTANCE.camera.ScreenToWorldPoint(Input.mousePosition);
            }
            if (CanSwitch)
            {
                if (Input.GetMouseButton(0))
                {
                    MousePoint2 = M_GUICamera.INSTANCE.camera.ScreenToWorldPoint(Input.mousePosition);
                }
                if ((MousePoint2 - MousePoint1).magnitude > 10f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SwitchPlayer()
    {
        if (MousePoint2.x > MousePoint1.x)
        {
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
            {
                M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection = M_PlayerControllerSupport.PlayerSelection.Mark;
                CanSwitch = false;
            }
        }
        else
        {
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
            {
                M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection = M_PlayerControllerSupport.PlayerSelection.Mark2;
                CanSwitch = false;
            }
        }
    }

    #endregion
}
