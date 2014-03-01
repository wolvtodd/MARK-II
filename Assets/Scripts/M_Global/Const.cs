using UnityEngine;
using System.Collections;

public class Const : MonoBehaviour
{
    /* クラス説明
     * 
     *      すべてのグローバル変数設定
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    public enum GAME_STATUS
    {
        Reset,
        FadeIn,
        Playing,
        GameClear,
        GameOver,
        FadeOutInPause,
        FadeOut
    }

    #endregion
}