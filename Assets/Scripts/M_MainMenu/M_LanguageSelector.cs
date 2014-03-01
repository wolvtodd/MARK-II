using UnityEngine;
using System.Collections;

public class M_LanguageSelector : MonoBehaviour
{
    /* クラス説明
     * 
     *      LanguageSelection処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    enum LanguageMenuStatus
    {
        Selecting,
        FadeOut
    }
    private LanguageMenuStatus m_LanguageMenuStatus;

    enum LanguageSelection
    {
        English     = 0,
        Japanese,
        Chinese
    }
    private LanguageSelection m_DesiredStatus;
    private LanguageSelection m_LanguageStatus;

    private M_MenuSelection m_English;
    private M_MenuSelection m_Japanese;
    private M_MenuSelection m_Chinese;

    private M_MenuSelection m_SelectLineLeft;
    private M_MenuSelection m_SelectLineRight;
    private M_MenuSelection m_SelectPaint;

    private GameObject m_LanguageSelector;

    #endregion

    #region Function

    void Start()
    {
        m_LanguageMenuStatus    = LanguageMenuStatus.Selecting;
        m_LanguageSelector      = GameObject.Find("Lan_Selections") as GameObject;

        if (GameObject.Find("Lan_0_LanguageEnglish").GetComponent<M_MenuSelection>() != null)
            m_English = GameObject.Find("Lan_0_LanguageEnglish").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_English = GameObject.Find("Lan_0_LanguageEnglish").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Lan_1_LanguageJapanese").GetComponent<M_MenuSelection>() != null)
            m_Japanese = GameObject.Find("Lan_1_LanguageJapanese").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Japanese = GameObject.Find("Lan_1_LanguageJapanese").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Lan_2_LanguageChinese").GetComponent<M_MenuSelection>() != null)
            m_Chinese = GameObject.Find("Lan_2_LanguageChinese").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chinese = GameObject.Find("Lan_2_LanguageChinese").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Lan_SelectLineLeft").GetComponent<M_MenuSelection>() != null)
            m_SelectLineLeft = GameObject.Find("Lan_SelectLineLeft").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineLeft = GameObject.Find("Lan_SelectLineLeft").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Lan_SelectLineRight").GetComponent<M_MenuSelection>() != null)
            m_SelectLineRight = GameObject.Find("Lan_SelectLineRight").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineRight = GameObject.Find("Lan_SelectLineRight").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Lan_SelectPaint").GetComponent<M_MenuSelection>() != null)
            m_SelectPaint = GameObject.Find("Lan_SelectPaint").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectPaint = GameObject.Find("Lan_SelectPaint").AddComponent<M_MenuSelection>() as M_MenuSelection;

        m_DesiredStatus         = LanguageSelection.English;
        m_LanguageStatus        = LanguageSelection.English;
    }

    void Update()
    {
        switch (m_DesiredStatus)
        {
            case LanguageSelection.English:
                M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.English);
                break;
            case LanguageSelection.Japanese:
                M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.Japanese);
                break;
            case LanguageSelection.Chinese:
                M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.Chinese);
                break;
            default:
                return;
        }
        if (m_LanguageMenuStatus == LanguageMenuStatus.Selecting)
        {
            FadeIn();
            if (m_LanguageStatus == m_DesiredStatus)
                RecieveInput();
            float desiredPosition = GetDesiredSelectionPosition(m_DesiredStatus);
            SmoothToSelection(desiredPosition);
        }
        else if (m_LanguageMenuStatus == LanguageMenuStatus.FadeOut)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        m_SelectLineLeft.DesiredAlpha   = 255.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 255.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 75.0f / 255.0f;
        m_English.DesiredAlpha          = 255.0f / 255.0f;
        m_Japanese.DesiredAlpha         = 100.0f / 255.0f;
        m_Chinese.DesiredAlpha          = 50.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.05f;
        m_SelectLineRight.FadeSpeed     = 0.05f;
        m_SelectPaint.FadeSpeed         = 0.05f;
        m_English.FadeSpeed             = 0.05f;
        m_Japanese.FadeSpeed            = 0.05f;
        m_Chinese.FadeSpeed             = 0.05f;
    }

    void FadeOut()
    {
        m_SelectLineLeft.DesiredAlpha = 0.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha = 0.0f / 255.0f;
        m_SelectPaint.DesiredAlpha = 0.0f / 255.0f;
        m_English.DesiredAlpha = 0.0f / 255.0f;
        m_Japanese.DesiredAlpha = 0.0f / 255.0f;
        m_Chinese.DesiredAlpha = 0.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed = 0.2f;
        m_SelectLineRight.FadeSpeed = 0.2f;
        m_SelectPaint.FadeSpeed = 0.2f;
        m_English.FadeSpeed = 0.2f;
        m_Japanese.FadeSpeed = 0.2f;
        m_Chinese.FadeSpeed = 0.2f;

        if (m_SelectLineLeft.ZeroAlpha &&
            m_SelectLineRight.ZeroAlpha &&
            m_SelectPaint.ZeroAlpha &&
            m_English.ZeroAlpha &&
            m_Japanese.ZeroAlpha &&
            m_Chinese.ZeroAlpha)
        {
            Destroy(this);
        }
    }

    void SmoothToSelection(float desiredSelection)
    {
        float tempY = m_LanguageSelector.transform.position.y;
        tempY = Mathf.Lerp(tempY, desiredSelection, 0.2f);
        if (Mathf.Abs(tempY - desiredSelection) <= 1.0f)
        {
            m_LanguageStatus = m_DesiredStatus;
        }
        m_LanguageSelector.transform.position = new Vector3(m_LanguageSelector.transform.position.x,
                                                            tempY,
                                                            m_LanguageSelector.transform.position.z);
    }

    void RecieveInput()
    {
        var deadZone = 0.1f;
        if (Input.GetAxis("Vertical") > deadZone)
        {
            var status = m_LanguageStatus;
            status--;
            if (status < LanguageSelection.English)
            {
                status = LanguageSelection.English;
            }
            m_DesiredStatus = status;
        }
        else if (Input.GetAxis("Vertical") < -deadZone)
        {
            var status = m_LanguageStatus;
            status++;
            if (status > LanguageSelection.Chinese)
            {
                status = LanguageSelection.Chinese;
            }
            m_DesiredStatus = status;
        }
        if (Input.GetButtonDown("Confirm"))
        {
            switch (m_DesiredStatus)
            {
                case LanguageSelection.English:
                    M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.English);
                    break;
                case LanguageSelection.Japanese:
                    M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.Japanese);
                    break;
                case LanguageSelection.Chinese:
                    M_GlobalSetting.SetGlobalLanguage(M_GlobalSetting.Language.Chinese);
                    break;
                default:
                    return;
            }
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_ASSEMBLE);
            this.m_LanguageMenuStatus = LanguageMenuStatus.FadeOut;
        }
        else if (Input.GetButtonDown("Back"))
        {
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_TITLE);
            this.m_LanguageMenuStatus = LanguageMenuStatus.FadeOut;
        }
    }

    float GetDesiredSelectionPosition(LanguageSelection status)
    {
        switch (status)
        {
            case LanguageSelection.English:
                m_English.DesiredAlpha  = 255.0f / 255.0f;
                m_Japanese.DesiredAlpha = 100.0f / 255.0f;
                m_Chinese.DesiredAlpha  = 50.0f / 255.0f;
                m_English.FadeSpeed     = 0.2f;
                m_Japanese.FadeSpeed    = 0.2f;
                m_Chinese.FadeSpeed     = 0.2f;
                return 0.0f;
            case LanguageSelection.Japanese:
                m_English.DesiredAlpha  = 100.0f / 255.0f;
                m_Japanese.DesiredAlpha = 255.0f / 255.0f;
                m_Chinese.DesiredAlpha  = 100.0f / 255.0f;
                m_English.FadeSpeed     = 0.2f;
                m_Japanese.FadeSpeed    = 0.2f;
                m_Chinese.FadeSpeed     = 0.2f;
                return 35.0f;
            case LanguageSelection.Chinese:
                m_English.DesiredAlpha  = 50.0f / 255.0f;
                m_Japanese.DesiredAlpha = 100.0f / 255.0f;
                m_Chinese.DesiredAlpha  = 255.0f / 255.0f;
                m_English.FadeSpeed     = 0.2f;
                m_Japanese.FadeSpeed    = 0.2f;
                m_Chinese.FadeSpeed     = 0.2f;
                return 70.0f;
        }
        return 0.0f;
    }

    #endregion
}