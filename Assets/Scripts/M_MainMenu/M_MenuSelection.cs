using UnityEngine;
using System.Collections;

public class M_MenuSelection : MonoBehaviour
{
    /* クラス説明
     * 
     *      主に色の処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public float DesiredAlpha;
    public float FadeSpeed;
    public bool  ZeroAlpha;
    public bool  MultiLanguagePorted;

    private Material m_SelectionMaterial;

    #endregion

    #region Function

    void Start()
    {
        DesiredAlpha = 0.0f;
        FadeSpeed = 0.2f;
        ZeroAlpha = false;
        MultiLanguagePorted = false;
        m_SelectionMaterial = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        SmoothToDesiredAlpha(DesiredAlpha);
        if (MultiLanguagePorted)
        {
            ProcessMultiLanguage(M_GlobalSetting.GetLanguageOffset());
        }
    }

    void SmoothToDesiredAlpha(float desiredAlphad)
    {
        ZeroAlpha = false;
        float tempAlpha = m_SelectionMaterial.color.a;
        tempAlpha = Mathf.Lerp(tempAlpha, desiredAlphad, FadeSpeed);
        if (Mathf.Abs(tempAlpha - desiredAlphad) <= 0.01f)
        {
            ZeroAlpha = true;
        }
        m_SelectionMaterial.color = new Color(m_SelectionMaterial.color.r, 
                                              m_SelectionMaterial.color.g, 
                                              m_SelectionMaterial.color.b, 
                                              tempAlpha);
    }

    public void ProcessMultiLanguage(Vector2 offset)
    {
        m_SelectionMaterial.SetTextureScale("_MainTex", new Vector2(m_SelectionMaterial.GetTextureScale("_MainTex").x, 1.0f / 3));
        m_SelectionMaterial.SetTextureOffset("_MainTex", new Vector2(m_SelectionMaterial.GetTextureOffset("_MainTex").x, offset.y));
    }

    #endregion
}