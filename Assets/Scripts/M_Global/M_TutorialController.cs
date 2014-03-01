using UnityEngine;
using System.Collections;

public class M_TutorialController : MonoBehaviour
{
    /* クラス説明
     * 
     *      チュートリアルのGUI位置処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private Material material;
    private Material backgroundMaterial;

    #endregion

    #region Function

    void Start()
    {
        material = this.GetComponent<Renderer>().material;
        backgroundMaterial = GameObject.Find("TutorialBackground").GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.GameClear)
        {
            FadeOut();
        }
        ProcessMultiLanguage(M_GlobalSetting.GetLanguageOffset());
        SmoothToDesiredPosition(GetDesiredPosition());
    }

    void SmoothToDesiredPosition(float desiredPosition)
    {
        float tempPositionX = this.transform.localPosition.x;
        tempPositionX = Mathf.Lerp(tempPositionX, desiredPosition, 0.2f);
        this.transform.localPosition = new Vector3(tempPositionX,
                                                   this.transform.localPosition.y,
                                                   this.transform.localPosition.z);
    }

    float GetDesiredPosition()
    {
        float desiredPosition = 0.0f;

        switch (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection)
        {
            case M_PlayerControllerSupport.PlayerSelection.Mark:
                desiredPosition = -30.0f;
                break;
            case M_PlayerControllerSupport.PlayerSelection.Mark2:
                desiredPosition = 30.0f;
                break;
        }
        return desiredPosition;
    }

    void FadeOut()
    {
        float tempAlpha = backgroundMaterial.color.a;
        tempAlpha-= Time.deltaTime;
        if( tempAlpha < 0.0f )
        {
            tempAlpha = 0.0f;
        }
        backgroundMaterial.color = new Color(backgroundMaterial.color.r,
                                             backgroundMaterial.color.g,
                                             backgroundMaterial.color.b,
                                             tempAlpha);
        tempAlpha = material.color.a;
        tempAlpha -= Time.deltaTime;
        if (tempAlpha < 0.0f)
        {
            tempAlpha = 0.0f;
        }
        material.color = new Color(material.color.r,
                                   material.color.g,
                                   material.color.b,
                                   tempAlpha);
    }

    void ProcessMultiLanguage(Vector2 offset)
    {
        material.SetTextureScale("_MainTex", new Vector2(material.GetTextureScale("_MainTex").x, 1.0f / 3));
        material.SetTextureOffset("_MainTex", new Vector2(material.GetTextureOffset("_MainTex").x, offset.y));
    }

    #endregion
}