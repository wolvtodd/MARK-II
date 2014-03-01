using UnityEngine;
using System.Collections;

public class M_JumpPadVolume : MonoBehaviour
{
    /* クラス説明
     * 
     *      JumpPadについてのすべての処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_JumpPadVolume INSTANCE;

    public bool IsMark2OnJumpPad = false;

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

    //クラスを初期化します
    void Start()
    {

    }

    void Update()
    {
        if (IsMark2OnJumpPad)
            M_Motor_Mark2.INSTANCE.VerticalSpeed = M_Motor_Mark2.INSTANCE.JumpSpeed;
    }

    void OnTriggerEnter(Collider otherController)
    {
        if (otherController.tag == "SubPlayer")
            IsMark2OnJumpPad = true;
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.tag == "SubPlayer")
        {
            IsMark2OnJumpPad = false;
        }
    }

    #endregion
}
