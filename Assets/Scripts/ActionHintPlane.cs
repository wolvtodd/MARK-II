using UnityEngine;
using System.Collections;

public class ActionHintPlane : MonoBehaviour
{
    /* クラス説明
     * 
     *      アクション操作できるかどうか明示用ヒント
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static ActionHintPlane INSTANCE;

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

    #endregion
}
