using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    /* クラス説明
     * 
     *      Test用リスタートボタン
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    #endregion



    #region Function

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.85f, Screen.width * 0.1f, Screen.height * 0.1f), "Restart"))
        {
            Application.LoadLevel(0);
        }
    }

    #endregion
}
