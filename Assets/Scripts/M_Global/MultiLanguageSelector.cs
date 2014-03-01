using UnityEngine;
using System.Collections;

public class MultiLanguageSelector : MonoBehaviour
{
    /* クラス説明
     * 
     *      マルチ言語選択器。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private Material material;

    #endregion

    #region Functions

    void Start()
    {
        material = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        ProcessMultiLanguage(M_GlobalSetting.GetLanguageOffset());
    }

    void ProcessMultiLanguage(Vector2 offset)
    {
        material.SetTextureScale("_MainTex", new Vector2(material.GetTextureScale("_MainTex").x, 1.0f / 3));
        material.SetTextureOffset("_MainTex", new Vector2(material.GetTextureOffset("_MainTex").x, offset.y));
    }

    #endregion
}