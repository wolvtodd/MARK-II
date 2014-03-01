using UnityEngine;
using System.Collections;

public class M_FunctionController : MonoBehaviour
{
    /* クラス説明
     * 
     *      指で操作必要の感応ファンクション
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public GameObject FunctionSign;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "FunctionSign")
            {
                FunctionSign = child.gameObject;
            }
        }
    }

    void Update()
    {
        if (M_GameMain.GAME_PAUSED)
        {
            return;
        }

        var desiredEuler = 0f;
        var tempEulerZ = 0f;
        var signRotateSpeed = 120f;

        switch (this.name)
        {
            case "MarkFunctionPlane":
                FunctionSign.transform.Rotate(new Vector3(0, 0, signRotateSpeed * Time.deltaTime));
                FunctionSign.transform.localEulerAngles = Vector3.zero + Vector3.forward * FunctionSign.transform.localEulerAngles.z;
                if (M_Motor_Mark.INSTANCE.CanDoAction)
                {
                    desiredEuler = 135f;
                }
                break;
            case "Mark2FunctionPlane":
                FunctionSign.transform.Rotate(new Vector3(0, 0, signRotateSpeed * Time.deltaTime));
                FunctionSign.transform.localEulerAngles = Vector3.zero + Vector3.forward * FunctionSign.transform.localEulerAngles.z;
                if (M_Motor_Mark2.INSTANCE.CanAttack())
                {
                    desiredEuler = 135f;
                }
                break;
        }
        tempEulerZ = Mathf.Lerp(transform.eulerAngles.z, desiredEuler, 10f * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, tempEulerZ);
    }

    #endregion
}
