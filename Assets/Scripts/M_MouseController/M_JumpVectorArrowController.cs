using UnityEngine;
using System.Collections;

public class M_JumpVectorArrowController : MonoBehaviour
{
    /* クラス説明
     * 
     *      ジャンプ方向と力度の操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_JumpVectorArrowController INSTANCE;

    public float JumpArrowScaleX = 0f;

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
        JumpArrowScaleX = M_MousePlayerController.INSTANCE.TempJumpVector.x;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ProcessJumpVectorArrowScale();
            ProcessJumpVectorArrowPosition();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            renderer.enabled = false;
        }
    }

    void ProcessJumpVectorArrowScale()
    {
        JumpArrowScaleX = M_MousePlayerController.INSTANCE.TempJumpVector.x;
        transform.localScale = new Vector3(JumpArrowScaleX, 
                                           transform.localScale.y, 
                                           transform.localScale.z);
    }

    void ProcessJumpVectorArrowPosition()
    {
        transform.position = new Vector3(M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x + JumpArrowScaleX * 5, 
                                         M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y + M_Controller_Mark.MARK_CHARCONTROLLER.height, 
                                         M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.z);
    }

    #endregion
}
