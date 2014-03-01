using UnityEngine;
using System.Collections;

public class M_JumpPadMain : MonoBehaviour
{
    /* クラス説明
     * 
     *      ジャンプパッドすべての操作メイン処理。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    private M_Switch    m_Switch;
    private M_JumpPad   m_JumpPad;

    public bool     IsActivated     = false;
    public bool     ToActive        = false;
    public bool     ToDeactive      = false;

    public bool     ActiveTrigger   = false;

    public float    Mark2AddOnSpeed = 0f;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */
    
    //クラスを初期化します
    void Start()
    {
        m_Switch    = gameObject.GetComponentInChildren<M_Switch>()  as M_Switch;
        m_JumpPad   = gameObject.GetComponentInChildren<M_JumpPad>() as M_JumpPad;
    }

    void Update()
    {
        m_Switch.UpdateSwitch();
        m_JumpPad.UpdateJumpPad();
    }

    #endregion
}
