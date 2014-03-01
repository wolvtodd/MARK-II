using UnityEngine;
using System.Collections;

public class M_GUIKeyMotor : MonoBehaviour
{
    /* クラス説明
     * 
     *      鍵取りに関する操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    public static M_GUIKeyMotor INSTANCE;

    public  Transform   StartPoint;
    public  Transform   HookPoint;
    public  float       FallSpeed       = 0f;
    public  float       Gravity         = 25f;
    public  bool        CanDropKey      = false;

    private float       m_FlashSpeed    = 2.0f;
    private float       m_RotateAngle   = 0.0f;
    private float       m_HelperAngle   = 0.0f;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Start()
    {
        INSTANCE = this;
        transform.position = StartPoint.position;
        m_HelperAngle = Random.Range(-Mathf.PI * 2, Mathf.PI * 2);
    }

    public void Update()
    {
        if (CanDropKey)
        {
            IAmFallingAndHookMe();
        }
        RotateMe();
        FlashMe();
    }

    void RotateMe()
    {
        float rate;

        m_HelperAngle   += Time.deltaTime;
        if (m_HelperAngle >= Mathf.PI * 2)
        {
            m_HelperAngle = 0f;
        }
        rate            = Mathf.Cos(m_HelperAngle);
        m_RotateAngle   = rate * 15;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, m_RotateAngle, m_RotateAngle * 0.2f);
    }

    void IAmFallingAndHookMe()
    {
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
        {
            if (transform.position.y > HookPoint.position.y)
            {
                FallSpeed += Gravity;
                transform.position += Vector3.down * FallSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, HookPoint.position.y, transform.position.z);
            }
        }
    }

    void FlashMe()
    {
        var keyRenderer = this.GetComponentInChildren<MeshRenderer>();
        if (M_GameMain.INSTANCE.KeyGet >= 3)
        {
            var tempColor = keyRenderer.material.color.g;

            tempColor += m_FlashSpeed * Time.deltaTime;
            if (tempColor > 300.0f / 255.0f)
            {
                tempColor = 300.0f / 255.0f;
                m_FlashSpeed *= -1;
            }
            else if (tempColor < 150.0f / 255.0f)
            {
                tempColor = 150.0f / 255.0f;
                m_FlashSpeed *= -1;
            }
            keyRenderer.material.color = new Color(tempColor, tempColor, tempColor, 1.0f);
        }
        else
        {
            keyRenderer.material.color = new Color(150.0f / 255.0f, 150.0f / 255.0f, 150.0f / 255.0f, 1.0f);
        }
    }

    #endregion
}
