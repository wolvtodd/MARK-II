using UnityEngine;
using System.Collections;

public class M_GUIButton : MonoBehaviour
{
    /* クラス説明
     * 
     *      ＧＵＩのボタン反応。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public bool IsButtonPressed = false;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    protected virtual void Update()
    {
        M_GUIController.IS_CONTROLLING_GUI = CanFunction();
        if (CanFunction())
        {
            ProcessStatus();
        }
    }

    bool CanFunction()
    {
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width &&
            Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)
        {
            var mousePos = M_GUICamera.INSTANCE.camera.ScreenToWorldPoint(Input.mousePosition);
            var functionVolume = GetComponent<BoxCollider>() as BoxCollider;
            var funcCenter = new Vector2(functionVolume.transform.position.x, functionVolume.transform.position.y);

            if (Vector3.Magnitude((Vector2)mousePos - funcCenter) <= 25f)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void ProcessStatus()
    {
        IsButtonPressed = false;
    }

    #endregion
}
