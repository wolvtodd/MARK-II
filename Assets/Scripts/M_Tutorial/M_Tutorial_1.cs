using UnityEngine;
using System.Collections;

public class M_Tutorial_1 : MonoBehaviour
{
    /* クラス説明
     * 
     *      ステージ１のチュートリアル流れ。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    public int              CurrentStageID;

    public Transform        Parent;
    public GameObject       Hand;
    public GameObject       TouchEffect;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        //GameObject生成
        Parent      = this.transform;

        Hand        = GameObject.Find("hand") as GameObject;
        TouchEffect = GameObject.Find("TouchEffect") as GameObject;

        //Parent設定
        Hand.transform.parent           = Parent;
        TouchEffect.transform.parent    = Parent;

        //座標設定
        Hand.transform.localPosition        = new Vector3(0, 0, 0);
        TouchEffect.transform.localPosition = new Vector3(0, -50, 10);

        //見た目設定
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void ProcessHand()
    {
        var endPoint    = new Vector3(100, -50, 0);

        Hand.transform.localPosition = Vector3.Lerp(Hand.transform.localPosition, endPoint, Time.deltaTime);
    }

    #endregion
}
