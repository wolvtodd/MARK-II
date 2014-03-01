using UnityEngine;
using System.Collections;

public class M_GUIButton_MarkAction : M_GUIButton
{
    /* クラス説明
     * 
     *      Markの操作ボタン
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_GUIButton_MarkAction    INSTANCE;

    #endregion



    #region Function

    void Start()
    {
        INSTANCE = this;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ProcessStatus()
    {
        base.ProcessStatus();
        if (Input.GetMouseButtonDown(0))
        {
            M_MousePlayerController.INSTANCE.IsControllingGUI = true;
            IsButtonPressed = true;
        }
    }

    #endregion
}
