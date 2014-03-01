using UnityEngine;
using System.Collections;

public class ElectricAnimator : MonoBehaviour
{
    /* クラス説明
     * 
     *      電気フローアニメーションの処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    public float    CurrentFrame;
    
    private float   m_AnimationFPS = 24;
    private float   m_MaxFrame = 8;
    private float   m_CurrentTime;

    void Update()
    {
        m_AnimationFPS = DesiredFPS();
        if (this.gameObject.renderer.enabled)
        {
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime > 1 / m_AnimationFPS)
            {
                CurrentFrame++;
                m_CurrentTime = 0;
            }
            if (CurrentFrame > m_MaxFrame)
            {
                CurrentFrame = 1;
            }
            this.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(0, CurrentFrame / m_MaxFrame));
            this.gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(1, 0.125f));
        }
    }

    float DesiredFPS()
    {
        if (CurrentFrame <= 5)
        {
            return 24f;
        }
        else
        {
            return 8f;
        }
    }

    #endregion
}
