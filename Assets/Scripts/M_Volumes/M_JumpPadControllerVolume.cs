using UnityEngine;
using System.Collections;

public class M_JumpPadControllerVolume : MonoBehaviour
{
    /* クラス説明
     * 
     *      JumpPadControllerの設定
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_JumpPadControllerVolume INSTANCE;

    public bool IsMarkCanControl = false;

    public enum JumpPadControllerColorState
    {
        Gray,
        Yellow,
        Green,
        Orange
    }
    public JumpPadControllerColorState CurrentPadControllerColorState;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            IsMarkCanControl = true;
            CurrentPadControllerColorState = JumpPadControllerColorState.Yellow;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            IsMarkCanControl = false;
            CurrentPadControllerColorState = JumpPadControllerColorState.Gray;
        }
    }

    #endregion
}
