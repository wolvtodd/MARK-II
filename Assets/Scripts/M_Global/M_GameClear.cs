using UnityEngine;
using System.Collections;

public class M_GameClear : MonoBehaviour
{
    /* クラス説明
     * 
     *      クリアシーンの操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private float m_FloatingTime = 0.0f;
    private float m_MoveSpeed = 10.0f;

    private GameObject m_Wolvtodd;
    private GameObject m_Thanks;

    private bool m_EndThisLevel;
    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        m_EndThisLevel  = false;
        m_FloatingTime  = 0.0f;
        m_MoveSpeed     = 10.0f;
        m_Wolvtodd      = GameObject.Find("aWolvtoddGame") as GameObject;
        m_Thanks        = GameObject.Find("thx") as GameObject;
        m_Wolvtodd.renderer.material.color = new Color(m_Wolvtodd.renderer.material.color.r,
                                                       m_Wolvtodd.renderer.material.color.g,
                                                       m_Wolvtodd.renderer.material.color.b,
                                                       0);
        m_Thanks.renderer.material.color = new Color(m_Thanks.renderer.material.color.r,
                                                     m_Thanks.renderer.material.color.g,
                                                     m_Thanks.renderer.material.color.b,
                                                     0);
    }

    void Update()
    {
        m_FloatingTime += Time.deltaTime;
        if (m_Thanks.transform.position.x < 0.0f)
        {
            m_Thanks.transform.Translate(Vector3.right * m_MoveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            m_Thanks.transform.position = new Vector3(0, 25, 20);
        }
        Fade();
    }

    void Fade()
    {
        Renderer wolvtodd = m_Wolvtodd.GetComponent<Renderer>() as Renderer;
        Renderer thx = m_Thanks.GetComponent<Renderer>() as Renderer;
        float alphaW = 0.0f;
        float alphaT = 0.0f;

        alphaW = wolvtodd.material.color.a;
        alphaT = thx.material.color.a;

        if (m_FloatingTime < 5f)
        {
            alphaT = Mathf.Lerp(alphaT, 1.0f, 0.1f);
            thx.material.color = new Color(thx.material.color.r,
                                           thx.material.color.g,
                                           thx.material.color.b,
                                           alphaT);
        }
        else
        {
            alphaT = Mathf.Lerp(alphaT, 0.0f, 0.1f);
            thx.material.color = new Color(thx.material.color.r,
                                           thx.material.color.g,
                                           thx.material.color.b,
                                           alphaT);
        }

        if (m_FloatingTime > 3f && m_FloatingTime < 6f)
        {
            alphaW = Mathf.Lerp(alphaW, 1.0f, 0.1f);
            wolvtodd.material.color = new Color(wolvtodd.material.color.r,
                                           wolvtodd.material.color.g,
                                           wolvtodd.material.color.b,
                                           alphaW);
        }
        else if (m_FloatingTime >= 10.0f)
        {
            if (Input.anyKey)
            {
                m_EndThisLevel = true;
            }

            if (m_EndThisLevel)
            {
                alphaW = Mathf.Lerp(alphaW, 0.0f, 0.1f);
                wolvtodd.material.color = new Color(wolvtodd.material.color.r,
                                                    wolvtodd.material.color.g,
                                                    wolvtodd.material.color.b,
                                                    alphaW);
                if (wolvtodd.material.color.a <= 0.001f)
                {
                    Application.LoadLevel("MainMenu_Level");
                }
            }
        }
    }

    #endregion
}
