using UnityEngine;
using System.Collections;

public class FPSChecker : MonoBehaviour
{
    /* クラス説明
     * 
     *      ゲーム実際のＦＰＳをチェックします。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private float   m_TimeA;
    public  float   FPS;
    public  float   LastFPS;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        m_TimeA = Time.timeSinceLevelLoad;
        //DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - m_TimeA <= 1f)
        {
            FPS++;
        }
        else
        {
            LastFPS = FPS + 1f;
            m_TimeA = Time.timeSinceLevelLoad;
            FPS = 0.0f;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), "FPS: " + LastFPS);
    }

    #endregion
}
