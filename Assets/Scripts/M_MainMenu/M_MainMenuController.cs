using UnityEngine;
using System.Collections;

public class M_MainMenuController : MonoBehaviour
{
    /* クラス説明
     * 
     *      MainMenu処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_MainMenuController CONTROLLER;

    public float            DesiredCameraPos;

    private Material        m_PressStartMatarial;
    private Material        m_FadePlaneMatarial;
    private Material        m_LogoMaterial_0;
    private Material        m_LogoMaterial_1;
    private Material        m_LogoMaterial_2;
    private Material        m_AllRightsInfoMaterial;

    private M_MenuSelection m_HintConfirm;
    private M_MenuSelection m_HintCancel;
    private M_MenuSelection m_HintSelect;

    private float           m_AlphaChangeSpeed;
    private float[]         m_LogoAlphaFadeSpeed;
    private float           m_FadeSpeed;

    private string          m_DesiredOpenName;

    public enum MAIN_MENU_STATUS
    {
        FADE_IN,
        MAIN_MENU_TITLE,
        FADE_TO_MENU_LANGUAGE_SELECTION,
        MAIN_MENU_LANGUAGE_SELECTION,
        MAIN_MENU_ASSEMBLE,
        MAIN_MENU_CHAPTER_SELECTION,
        MAIN_MENU_ABOUT_THE_AUTHOR,
        FADE_OUT
    }
    private MAIN_MENU_STATUS m_Status;

    #endregion

    #region Function

    void Start()
    {
        CONTROLLER = this;

        DesiredCameraPos                = 0.0f;
        m_Status                        = MAIN_MENU_STATUS.FADE_IN;
        m_PressStartMatarial            = GameObject.Find("Press_Start").GetComponent<Renderer>().material;
        m_PressStartMatarial.color      = new Color(m_PressStartMatarial.color.r,
                                                    m_PressStartMatarial.color.g,
                                                    m_PressStartMatarial.color.b,
                                                    0.0f);
        m_FadePlaneMatarial             = GameObject.Find("FadePlane").GetComponent<Renderer>().material;
        m_LogoMaterial_0                = GameObject.Find("Logo_Title_0").GetComponent<Renderer>().material;
        m_LogoMaterial_1                = GameObject.Find("Logo_Title_1").GetComponent<Renderer>().material;
        m_LogoMaterial_2                = GameObject.Find("Logo_Title_2").GetComponent<Renderer>().material;
        m_AllRightsInfoMaterial         = GameObject.Find("All_Rights_Info").GetComponent<Renderer>().material;
        m_LogoMaterial_0.color          = new Color(m_LogoMaterial_0.color.r,
                                                    m_LogoMaterial_0.color.g,
                                                    m_LogoMaterial_0.color.b,
                                                    1.0f);
        m_LogoMaterial_1.color          = new Color(m_LogoMaterial_1.color.r,
                                                    m_LogoMaterial_1.color.g,
                                                    m_LogoMaterial_1.color.b,
                                                    0.0f);
        m_LogoMaterial_2.color          = new Color(m_LogoMaterial_2.color.r,
                                                    m_LogoMaterial_2.color.g,
                                                    m_LogoMaterial_2.color.b,
                                                    0.0f);
        m_AlphaChangeSpeed              = 1.0f;
        m_LogoAlphaFadeSpeed            = new float[3]{1.0f, 1.5f, 2.0f};
        m_FadeSpeed                     = 1.0f;

        m_HintConfirm                   = GameObject.Find("Hint_Confirm").AddComponent<M_MenuSelection>();
        m_HintCancel                    = GameObject.Find("Hint_Cancel").AddComponent<M_MenuSelection>();
        m_HintSelect                    = GameObject.Find("Hint_Select").AddComponent<M_MenuSelection>();

        m_HintConfirm.DesiredAlpha      = 0.0f;
        m_HintCancel.DesiredAlpha       = 0.0f;
        m_HintSelect.DesiredAlpha       = 0.0f;
        M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.English);
        m_DesiredOpenName               = "Turtorial_Level_01";
    }

    void Update()
    {
        ProcessHints();
        switch (m_Status)
        {
            case MAIN_MENU_STATUS.FADE_IN:
                DesiredCameraPos = 0.0f;
                FadeIn();
                break;
            case MAIN_MENU_STATUS.MAIN_MENU_TITLE:
                DesiredCameraPos = 0.0f;
                FadeIn();
                TitleUpdate();
                RecieveTitleInput();
                break;
            case MAIN_MENU_STATUS.FADE_TO_MENU_LANGUAGE_SELECTION:
                DesiredCameraPos = 25.0f;
                HalfFadeOut();
                break;
            case MAIN_MENU_STATUS.MAIN_MENU_LANGUAGE_SELECTION:
                DesiredCameraPos = 25.0f;
                LanguageSelectionUpdate();
                break;
            case MAIN_MENU_STATUS.MAIN_MENU_ASSEMBLE:
                DesiredCameraPos = -25.0f;
                AssembleTitleUpdate();
                break;
            case MAIN_MENU_STATUS.MAIN_MENU_CHAPTER_SELECTION:
                DesiredCameraPos = -100.0f;
                ChapterSelectionUpdate();
                break;
            case MAIN_MENU_STATUS.MAIN_MENU_ABOUT_THE_AUTHOR:
                DesiredCameraPos = -100.0f;
                AboutTheAuthorUpdate();
                break;
            case MAIN_MENU_STATUS.FADE_OUT:
                FadeOut();
                break;
            default:
                break;
        }
    }

    void ProcessHints()
    {
        if (m_Status == MAIN_MENU_STATUS.FADE_IN ||
            m_Status == MAIN_MENU_STATUS.MAIN_MENU_TITLE ||
            m_Status == MAIN_MENU_STATUS.FADE_OUT ||
            m_Status == MAIN_MENU_STATUS.MAIN_MENU_ABOUT_THE_AUTHOR)
        {
            m_HintConfirm.DesiredAlpha = 0.0f;
            m_HintCancel.DesiredAlpha = 0.0f;
            m_HintSelect.DesiredAlpha = 0.0f;
        }
        else
        {
            m_HintConfirm.MultiLanguagePorted = true;
            m_HintCancel.MultiLanguagePorted = true;
            m_HintSelect.MultiLanguagePorted = true;
            m_HintConfirm.DesiredAlpha = 1.0f;
            m_HintCancel.DesiredAlpha = 1.0f;
            m_HintSelect.DesiredAlpha = 1.0f;
        }
    }

    void FadeIn()
    {
        float alpha = m_FadePlaneMatarial.color.a;
        alpha -= m_FadeSpeed * Time.deltaTime;
        if (alpha <= 0.0f)
        {
            alpha = 0.0f;
            SetMainMenuStatus(MAIN_MENU_STATUS.MAIN_MENU_TITLE);
            return;
        }
        m_FadePlaneMatarial.color = new Color(m_FadePlaneMatarial.color.r,
                                              m_FadePlaneMatarial.color.g,
                                              m_FadePlaneMatarial.color.b,
                                              alpha);
    }

    public void FadeOutPreperation()
    {
        m_LogoMaterial_0.color = new Color(m_LogoMaterial_0.color.r,
                                           m_LogoMaterial_0.color.g,
                                           m_LogoMaterial_0.color.b,
                                           0.0f);
        m_LogoMaterial_1.color = new Color(m_LogoMaterial_1.color.r,
                                           m_LogoMaterial_1.color.g,
                                           m_LogoMaterial_1.color.b,
                                           0.0f);
        m_LogoMaterial_2.color = new Color(m_LogoMaterial_2.color.r,
                                           m_LogoMaterial_2.color.g,
                                           m_LogoMaterial_2.color.b,
                                           0.0f);
        GameObject.Find("Logo_Title_0").transform.position = new Vector3(GameObject.Find("Logo_Title_0").transform.position.x,
                                                                         GameObject.Find("Logo_Title_0").transform.position.y,
                                                                         GameObject.Find("FadePlane").transform.position.z - 1.0f);
        GameObject.Find("Logo_Title_1").transform.position = new Vector3(GameObject.Find("Logo_Title_1").transform.position.x,
                                                                         GameObject.Find("Logo_Title_1").transform.position.y,
                                                                         GameObject.Find("FadePlane").transform.position.z - 1.0f);
        GameObject.Find("Logo_Title_2").transform.position = new Vector3(GameObject.Find("Logo_Title_2").transform.position.x,
                                                                         GameObject.Find("Logo_Title_2").transform.position.y,
                                                                         GameObject.Find("FadePlane").transform.position.z - 1.0f);
    }

    void FadeOut()
    {
        float alpha = m_FadePlaneMatarial.color.a;
        alpha += m_FadeSpeed * Time.deltaTime;

        if (alpha >= 1.0f)
        {
            alpha = 1.0f;
            float tempAlpha = m_LogoMaterial_0.color.a - Time.deltaTime * 0.3f;
            if (tempAlpha < 0.0f)
            {
                tempAlpha = 0.0f;
            }
            m_LogoMaterial_0.color = new Color(m_LogoMaterial_0.color.r,
                                               m_LogoMaterial_0.color.g,
                                               m_LogoMaterial_0.color.b,
                                               tempAlpha);

            tempAlpha = m_LogoMaterial_1.color.a - Time.deltaTime * 0.2f;
            if (tempAlpha < 0.0f)
            {
                tempAlpha = 0.0f;
            }
            m_LogoMaterial_1.color = new Color(m_LogoMaterial_1.color.r,
                                               m_LogoMaterial_1.color.g,
                                               m_LogoMaterial_1.color.b,
                                               tempAlpha);

            tempAlpha = m_LogoMaterial_2.color.a - Time.deltaTime * 0.1f;
            if (tempAlpha < 0.0f)
            {
                tempAlpha = 0.0f;
            }
            m_LogoMaterial_2.color = new Color(m_LogoMaterial_2.color.r,
                                               m_LogoMaterial_2.color.g,
                                               m_LogoMaterial_2.color.b,
                                               tempAlpha);
        }
        else
        {
            m_LogoMaterial_0.color = new Color(m_LogoMaterial_0.color.r,
                                               m_LogoMaterial_0.color.g,
                                               m_LogoMaterial_0.color.b,
                                               m_LogoMaterial_0.color.a + Time.deltaTime * 0.5f);
            m_LogoMaterial_1.color = new Color(m_LogoMaterial_1.color.r,
                                               m_LogoMaterial_1.color.g,
                                               m_LogoMaterial_1.color.b,
                                               m_LogoMaterial_1.color.a + Time.deltaTime * 0.5f);
            m_LogoMaterial_2.color = new Color(m_LogoMaterial_2.color.r,
                                               m_LogoMaterial_2.color.g,
                                               m_LogoMaterial_2.color.b,
                                               m_LogoMaterial_2.color.a + Time.deltaTime * 0.5f);
        }
        m_FadePlaneMatarial.color = new Color(m_FadePlaneMatarial.color.r,
                                              m_FadePlaneMatarial.color.g,
                                              m_FadePlaneMatarial.color.b,
                                              alpha);
        if (m_LogoMaterial_0.color.a == 0.0f &&
            m_LogoMaterial_1.color.a == 0.0f &&
            m_LogoMaterial_2.color.a == 0.0f)
        {
            Application.LoadLevel(m_DesiredOpenName);
        }
    }

    void TitleUpdate()
    {
        float tempAlpha = m_PressStartMatarial.color.a;
        tempAlpha += m_AlphaChangeSpeed * Time.deltaTime;
        if (tempAlpha >= 1.0f)
        {
            tempAlpha = 1.0f;
            m_AlphaChangeSpeed = -1.0f;
        }
        else if (tempAlpha <= 0.1f)
        {
            tempAlpha = 0.1f;
            m_AlphaChangeSpeed = 1.0f;
        }
        m_PressStartMatarial.color = new Color(m_PressStartMatarial.color.r,
                                               m_PressStartMatarial.color.g,
                                               m_PressStartMatarial.color.b,
                                               tempAlpha);

        tempAlpha = m_LogoMaterial_0.color.a;
        tempAlpha += m_LogoAlphaFadeSpeed[0] * Time.deltaTime / 2.5f;
        if (tempAlpha >= 1.0f)
        {
            tempAlpha = 1.0f;
            m_LogoAlphaFadeSpeed[0] = -1.0f;
        }
        else if (tempAlpha <= 0.1f)
        {
            tempAlpha = 0.1f;
            m_LogoAlphaFadeSpeed[0] = 1.0f;
        }
        m_LogoMaterial_0.color = new Color(m_LogoMaterial_0.color.r,
                                           m_LogoMaterial_0.color.g,
                                           m_LogoMaterial_0.color.b,
                                           tempAlpha);

        tempAlpha = m_LogoMaterial_1.color.a;
        tempAlpha += m_LogoAlphaFadeSpeed[1] * Time.deltaTime / 5.0f;
        if (tempAlpha >= 1.0f)
        {
            tempAlpha = 1.0f;
            m_LogoAlphaFadeSpeed[1] = -1.0f;
        }
        else if (tempAlpha <= 0.1f)
        {
            tempAlpha = 0.1f;
            m_LogoAlphaFadeSpeed[1] = 1.0f;
        }
        m_LogoMaterial_1.color = new Color(m_LogoMaterial_1.color.r,
                                           m_LogoMaterial_1.color.g,
                                           m_LogoMaterial_1.color.b,
                                           tempAlpha);

        tempAlpha = m_LogoMaterial_2.color.a;
        tempAlpha += m_LogoAlphaFadeSpeed[2] * Time.deltaTime / 1.0f;
        if (tempAlpha >= 1.0f)
        {
            tempAlpha = 1.0f;
            m_LogoAlphaFadeSpeed[2] = -1.0f;
        }
        else if (tempAlpha <= 0.1f)
        {
            tempAlpha = 0.1f;
            m_LogoAlphaFadeSpeed[2] = 1.0f;
        }
        m_LogoMaterial_2.color = new Color(m_LogoMaterial_2.color.r,
                                           m_LogoMaterial_2.color.g,
                                           m_LogoMaterial_2.color.b,
                                           tempAlpha);


        tempAlpha = m_AllRightsInfoMaterial.color.a;
        tempAlpha += Time.deltaTime;
        if (tempAlpha >= 1.0f)
        {
            tempAlpha = 1.0f;
        }
        m_AllRightsInfoMaterial.color = new Color(m_AllRightsInfoMaterial.color.r,
                                                  m_AllRightsInfoMaterial.color.g,
                                                  m_AllRightsInfoMaterial.color.b,
                                                  tempAlpha);
    }

    void LanguageSelectionUpdate()
    {
        if (this.gameObject.GetComponent<M_LanguageSelector>() == null)
        {
            this.gameObject.AddComponent<M_LanguageSelector>();
        }
    }

    void AssembleTitleUpdate()
    {
        if (this.gameObject.GetComponent<M_AssembleTitleController>() == null)
        {
            this.gameObject.AddComponent<M_AssembleTitleController>();
        }
    }

    void ChapterSelectionUpdate()
    {
        if (this.gameObject.GetComponent<M_StageSelectController>() == null)
        {
            this.gameObject.AddComponent<M_StageSelectController>();
        }
    }

    void AboutTheAuthorUpdate()
    {
        if (this.gameObject.GetComponent<M_AboutProducerController>() == null)
        {
            this.gameObject.AddComponent<M_AboutProducerController>();
        }
    }

    void RecieveTitleInput()
    {
        if (Input.anyKeyDown)
        {
            this.SetMainMenuStatus(MAIN_MENU_STATUS.FADE_TO_MENU_LANGUAGE_SELECTION);
        }
    }

    void HalfFadeOut()
    {
        float alpha = m_FadePlaneMatarial.color.a;
        alpha += m_FadeSpeed * Time.deltaTime;
        if (alpha >= 0.5f)
        {
            alpha = 0.5f;
            SetMainMenuStatus(MAIN_MENU_STATUS.MAIN_MENU_LANGUAGE_SELECTION);
        }
        m_FadePlaneMatarial.color = new Color(m_FadePlaneMatarial.color.r,
                                              m_FadePlaneMatarial.color.g,
                                              m_FadePlaneMatarial.color.b,
                                              alpha);

        alpha = m_LogoMaterial_0.color.a;
        alpha -= Time.deltaTime * 2.0f;
        if (alpha <= 0.0f)
        {
            alpha = 0.0f;
        }
        m_LogoMaterial_0.color = new Color(m_LogoMaterial_0.color.r,
                                           m_LogoMaterial_0.color.g,
                                           m_LogoMaterial_0.color.b,
                                           alpha);

        m_LogoMaterial_1.color = new Color(m_LogoMaterial_1.color.r,
                                           m_LogoMaterial_1.color.g,
                                           m_LogoMaterial_1.color.b,
                                           alpha);

        m_LogoMaterial_2.color = new Color(m_LogoMaterial_2.color.r,
                                           m_LogoMaterial_2.color.g,
                                           m_LogoMaterial_2.color.b,
                                           alpha);

        m_AllRightsInfoMaterial.color = new Color(m_AllRightsInfoMaterial.color.r,
                                                  m_AllRightsInfoMaterial.color.g,
                                                  m_AllRightsInfoMaterial.color.b,
                                                  alpha);

        m_PressStartMatarial.color = new Color(m_PressStartMatarial.color.r,
                                               m_PressStartMatarial.color.g,
                                               m_PressStartMatarial.color.b,
                                               alpha);
    }

    public void SetDesiredOpenStage(string stageName)
    {
        m_DesiredOpenName = stageName;
    }

    public void SetMainMenuStatus(MAIN_MENU_STATUS status)
    {
        m_Status = status;
    }

    public MAIN_MENU_STATUS GetMainMenuStatus()
    {
        return m_Status;
    }

    #endregion
}