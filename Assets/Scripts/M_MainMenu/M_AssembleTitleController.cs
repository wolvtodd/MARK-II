using UnityEngine;
using System.Collections;

public class M_AssembleTitleController : MonoBehaviour
{
    /* クラス説明
     * 
     *      AssembleTitle処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    enum AssembleMenuStatus
    {
        Selecting,
        FadeOut,
        HalfFadeOutToAuthor,
        HalfFadeOutToChapter
    }
    private AssembleMenuStatus m_AssembleMenuStatus;

    enum AssembleMenuSelection
    {
        GameStart       = 0,
        StageSelect,
        AboutAuthor
    }
    private AssembleMenuSelection m_DesiredStatus;
    private AssembleMenuSelection m_CurrentMenuStatus;

    private M_MenuSelection m_GameStart;
    private M_MenuSelection m_StageSelect;
    private M_MenuSelection m_AboutAuthor;

    private M_MenuSelection m_SelectLineLeft;
    private M_MenuSelection m_SelectLineRight;
    private M_MenuSelection m_SelectPaint;

    private GameObject m_MenuSelector;

    #endregion

    #region Function

    void Start()
    {
        m_AssembleMenuStatus    = AssembleMenuStatus.Selecting;
        m_MenuSelector          = GameObject.Find("Title_Selections") as GameObject;

        if (GameObject.Find("Ass_GameStart").GetComponent<M_MenuSelection>() != null)
            m_GameStart = GameObject.Find("Ass_GameStart").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_GameStart = GameObject.Find("Ass_GameStart").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Ass_StageSelect").GetComponent<M_MenuSelection>() != null)
            m_StageSelect = GameObject.Find("Ass_StageSelect").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_StageSelect = GameObject.Find("Ass_StageSelect").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Ass_AboutAuthor").GetComponent<M_MenuSelection>() != null)
            m_AboutAuthor = GameObject.Find("Ass_AboutAuthor").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_AboutAuthor = GameObject.Find("Ass_AboutAuthor").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Ass_SelectLineLeft").GetComponent<M_MenuSelection>() != null)
            m_SelectLineLeft = GameObject.Find("Ass_SelectLineLeft").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineLeft = GameObject.Find("Ass_SelectLineLeft").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Ass_SelectLineRight").GetComponent<M_MenuSelection>() != null)
            m_SelectLineRight = GameObject.Find("Ass_SelectLineRight").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectLineRight = GameObject.Find("Ass_SelectLineRight").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Ass_SelectPaint").GetComponent<M_MenuSelection>() != null)
            m_SelectPaint = GameObject.Find("Ass_SelectPaint").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_SelectPaint = GameObject.Find("Ass_SelectPaint").AddComponent<M_MenuSelection>() as M_MenuSelection;


        m_CurrentMenuStatus     = AssembleMenuSelection.GameStart;
        m_DesiredStatus         = AssembleMenuSelection.GameStart;
    }

    void Update()
    {
        if (m_AssembleMenuStatus == AssembleMenuStatus.Selecting)
        {
            FadeIn();
            if (m_CurrentMenuStatus == m_DesiredStatus)
                RecieveInput();
            float desiredPosition = GetDesiredSelectionPosition(m_DesiredStatus);
            SmoothToSelection(desiredPosition);
        }
        else if (m_AssembleMenuStatus == AssembleMenuStatus.FadeOut)
        {
            FadeOut();
        }
        else if (m_AssembleMenuStatus == AssembleMenuStatus.HalfFadeOutToChapter)
        {
            RecieveStageInput();
            HalfFadeOutToChapter();
        }
        else if (m_AssembleMenuStatus == AssembleMenuStatus.HalfFadeOutToAuthor)
        {
            RecieveAuthorInput();
            HalfFadeOutToAuthor();
        }
    }

    void FadeIn()
    {
        m_SelectLineLeft.DesiredAlpha   = 255.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 255.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 75.0f / 255.0f;
        m_GameStart.DesiredAlpha        = 255.0f / 255.0f;
        m_StageSelect.DesiredAlpha      = 100.0f / 255.0f;
        m_AboutAuthor.DesiredAlpha      = 50.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.05f;
        m_SelectLineRight.FadeSpeed     = 0.05f;
        m_SelectPaint.FadeSpeed         = 0.05f;
        m_GameStart.FadeSpeed           = 0.05f;
        m_StageSelect.FadeSpeed         = 0.05f;
        m_AboutAuthor.FadeSpeed         = 0.05f;

        m_GameStart.MultiLanguagePorted     = true;
        m_StageSelect.MultiLanguagePorted   = true;
        m_AboutAuthor.MultiLanguagePorted   = true;
    }

    void FadeOut()
    {
        m_SelectLineLeft.DesiredAlpha   = 0.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 0.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 0.0f / 255.0f;
        m_GameStart.DesiredAlpha        = 0.0f / 255.0f;
        m_StageSelect.DesiredAlpha      = 0.0f / 255.0f;
        m_AboutAuthor.DesiredAlpha      = 0.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.2f;
        m_SelectLineRight.FadeSpeed     = 0.2f;
        m_SelectPaint.FadeSpeed         = 0.2f;
        m_GameStart.FadeSpeed           = 0.2f;
        m_StageSelect.FadeSpeed         = 0.2f;
        m_AboutAuthor.FadeSpeed         = 0.2f;

        if (m_SelectLineLeft.ZeroAlpha &&
            m_SelectLineRight.ZeroAlpha &&
            m_SelectPaint.ZeroAlpha &&
            m_GameStart.ZeroAlpha &&
            m_StageSelect.ZeroAlpha &&
            m_AboutAuthor.ZeroAlpha)
        {
            Destroy(this);
        }
    }

    void HalfFadeOutToChapter()
    {
        m_SelectLineLeft.DesiredAlpha   = 185.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 185.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 0.0f / 255.0f;
        m_GameStart.DesiredAlpha        = 0.0f / 255.0f;
        m_StageSelect.DesiredAlpha      = 255.0f / 255.0f;
        m_AboutAuthor.DesiredAlpha      = 0.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.1f;
        m_SelectLineRight.FadeSpeed     = 0.1f;
        m_SelectPaint.FadeSpeed         = 0.1f;
        m_GameStart.FadeSpeed           = 0.1f;
        m_StageSelect.FadeSpeed         = 0.1f;
        m_AboutAuthor.FadeSpeed         = 0.1f;
    }

    void HalfFadeOutToAuthor()
    {
        m_SelectLineLeft.DesiredAlpha   = 185.0f / 255.0f;
        m_SelectLineRight.DesiredAlpha  = 185.0f / 255.0f;
        m_SelectPaint.DesiredAlpha      = 0.0f / 255.0f;
        m_GameStart.DesiredAlpha        = 0.0f / 255.0f;
        m_StageSelect.DesiredAlpha      = 0.0f / 255.0f;
        m_AboutAuthor.DesiredAlpha      = 255.0f / 255.0f;

        m_SelectLineLeft.FadeSpeed      = 0.1f;
        m_SelectLineRight.FadeSpeed     = 0.1f;
        m_SelectPaint.FadeSpeed         = 0.1f;
        m_GameStart.FadeSpeed           = 0.1f;
        m_StageSelect.FadeSpeed         = 0.1f;
        m_AboutAuthor.FadeSpeed         = 0.1f;
    }

    void SmoothToSelection(float desiredSelection)
    {
        float tempY = m_MenuSelector.transform.position.y;
        tempY = Mathf.Lerp(tempY, desiredSelection, 0.2f);
        if (Mathf.Abs(tempY - desiredSelection) <= 1.0f)
        {
            m_CurrentMenuStatus = m_DesiredStatus;
        }
        m_MenuSelector.transform.position = new Vector3(m_MenuSelector.transform.position.x,
                                                        tempY,
                                                        m_MenuSelector.transform.position.z);
    }

    void RecieveInput()
    {
        var deadZone = 0.1f;
        if (Input.GetAxis("Vertical") > deadZone)
        {
            var status = m_CurrentMenuStatus;
            status--;
            if (status < AssembleMenuSelection.GameStart)
            {
                status = AssembleMenuSelection.GameStart;
            }
            m_DesiredStatus = status;
        }
        else if (Input.GetAxis("Vertical") < -deadZone)
        {
            var status = m_CurrentMenuStatus;
            status++;
            if (status > AssembleMenuSelection.AboutAuthor)
            {
                status = AssembleMenuSelection.AboutAuthor;
            }
            m_DesiredStatus = status;
        }
        if (Input.GetButtonDown("Confirm"))
        {
            switch (m_DesiredStatus)
            {
                case AssembleMenuSelection.GameStart:
                    M_MainMenuController.CONTROLLER.FadeOutPreperation();
                    M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.FADE_OUT);
                    this.m_AssembleMenuStatus = AssembleMenuStatus.FadeOut;
                    break;
                case AssembleMenuSelection.StageSelect:
                    M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_CHAPTER_SELECTION);
                    this.m_AssembleMenuStatus = AssembleMenuStatus.HalfFadeOutToChapter;
                    break;
                case AssembleMenuSelection.AboutAuthor:
                    M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_ABOUT_THE_AUTHOR);
                    this.m_AssembleMenuStatus = AssembleMenuStatus.HalfFadeOutToAuthor;
                    break;
                default:
                    return;
            }
        }
        else if (Input.GetButtonDown("Back"))
        {
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_LANGUAGE_SELECTION);
            this.m_AssembleMenuStatus = AssembleMenuStatus.FadeOut;
        }
    }

    void RecieveStageInput()
    {
        if (Input.GetButtonDown("Back"))
        {
            this.m_AssembleMenuStatus = AssembleMenuStatus.Selecting;
        }
        if (M_StageSelectController.INSTANCE != null &&
            M_StageSelectController.INSTANCE.CanConfirm)
        {
            this.m_AssembleMenuStatus = AssembleMenuStatus.FadeOut;
        }
    }

    void RecieveAuthorInput()
    {
        if (Input.GetButtonDown("Back") ||
            Input.GetButtonDown("Confirm"))
        {
            this.m_AssembleMenuStatus = AssembleMenuStatus.Selecting;
        }
    }

    float GetDesiredSelectionPosition(AssembleMenuSelection selection)
    {
        switch (selection)
        {
            case AssembleMenuSelection.GameStart:
                m_GameStart.DesiredAlpha    = 255.0f / 255.0f;
                m_StageSelect.DesiredAlpha  = 100.0f / 255.0f;
                m_AboutAuthor.DesiredAlpha  = 50.0f / 255.0f;
                m_GameStart.FadeSpeed       = 0.2f;
                m_StageSelect.FadeSpeed     = 0.2f;
                m_AboutAuthor.FadeSpeed     = 0.2f;
                return 0.0f;
            case AssembleMenuSelection.StageSelect:
                m_GameStart.DesiredAlpha    = 100.0f / 255.0f;
                m_StageSelect.DesiredAlpha  = 255.0f / 255.0f;
                m_AboutAuthor.DesiredAlpha  = 100.0f / 255.0f;
                m_GameStart.FadeSpeed       = 0.2f;
                m_StageSelect.FadeSpeed     = 0.2f;
                m_AboutAuthor.FadeSpeed     = 0.2f;
                return 25.0f;
            case AssembleMenuSelection.AboutAuthor:
                m_GameStart.DesiredAlpha    = 50.0f / 255.0f;
                m_StageSelect.DesiredAlpha  = 100.0f / 255.0f;
                m_AboutAuthor.DesiredAlpha  = 255.0f / 255.0f;
                m_GameStart.FadeSpeed       = 0.2f;
                m_StageSelect.FadeSpeed     = 0.2f;
                m_AboutAuthor.FadeSpeed     = 0.2f;
                return 50.0f;
        }
        return 0.0f;
    }
    #endregion
}