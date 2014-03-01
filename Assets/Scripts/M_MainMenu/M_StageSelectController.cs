using UnityEngine;
using System.Collections;

public class M_StageSelectController : MonoBehaviour
{
    /* クラス説明
     * 
     *      StageSelection処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_StageSelectController INSTANCE;
    public bool CanConfirm;

    enum StageMenuStatus
    {
        Selecting,
        FadeOut,
    }
    private StageMenuStatus m_MenuStatus;

    enum StageSelection
    {
        Chapter00,
        Chapter01,
        Chapter02,
        Chapter03,
        Chapter04,
        Chapter05,
        Chapter06,
        Chapter07
    }
    private StageSelection m_DesiredStageSelection;
    private StageSelection m_CurrentStageSelection;

    private M_MenuSelection m_Chapter00;
    private M_MenuSelection m_Chapter01;
    private M_MenuSelection m_Chapter02;
    private M_MenuSelection m_Chapter03;
    private M_MenuSelection m_Chapter04;
    private M_MenuSelection m_Chapter05;
    private M_MenuSelection m_Chapter06;
    private M_MenuSelection m_Chapter07;

    private GameObject m_StageSelector;

    #endregion

    #region Function

    void Start()
    {
        INSTANCE = this;
        CanConfirm = false;

        m_MenuStatus    = StageMenuStatus.Selecting;
        m_StageSelector = GameObject.Find("StageSelectionAssets") as GameObject;

        if (GameObject.Find("Chapter00").GetComponent<M_MenuSelection>() != null)
            m_Chapter00 = GameObject.Find("Chapter00").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter00 = GameObject.Find("Chapter00").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter01").GetComponent<M_MenuSelection>() != null)
            m_Chapter01 = GameObject.Find("Chapter01").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter01 = GameObject.Find("Chapter01").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter02").GetComponent<M_MenuSelection>() != null)
            m_Chapter02 = GameObject.Find("Chapter02").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter02 = GameObject.Find("Chapter02").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter03").GetComponent<M_MenuSelection>() != null)
            m_Chapter03 = GameObject.Find("Chapter03").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter03 = GameObject.Find("Chapter03").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter04").GetComponent<M_MenuSelection>() != null)
            m_Chapter04 = GameObject.Find("Chapter04").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter04 = GameObject.Find("Chapter04").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter05").GetComponent<M_MenuSelection>() != null)
            m_Chapter05 = GameObject.Find("Chapter05").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter05 = GameObject.Find("Chapter05").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter06").GetComponent<M_MenuSelection>() != null)
            m_Chapter06 = GameObject.Find("Chapter06").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter06 = GameObject.Find("Chapter06").AddComponent<M_MenuSelection>() as M_MenuSelection;

        if (GameObject.Find("Chapter07").GetComponent<M_MenuSelection>() != null)
            m_Chapter07 = GameObject.Find("Chapter07").GetComponent<M_MenuSelection>() as M_MenuSelection;
        else
            m_Chapter07 = GameObject.Find("Chapter07").AddComponent<M_MenuSelection>() as M_MenuSelection;

        m_CurrentStageSelection = StageSelection.Chapter00;
        m_DesiredStageSelection = StageSelection.Chapter00;
    }

    void Update()
    {
        if (m_MenuStatus == StageMenuStatus.Selecting)
        {
            CheckCancel();
            FadeIn();
            if (m_CurrentStageSelection == m_DesiredStageSelection)
                RecieveInput();
            float desiredPosition = GetDesiredSelectionPosition(m_DesiredStageSelection);
            SmoothToSelection(desiredPosition);
        }
        else if (m_MenuStatus == StageMenuStatus.FadeOut)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        m_Chapter00.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter01.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter02.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter03.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter04.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter05.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter06.DesiredAlpha = 255.0f / 255.0f;
        m_Chapter07.DesiredAlpha = 255.0f / 255.0f;

        m_Chapter00.FadeSpeed = 0.05f;
        m_Chapter01.FadeSpeed = 0.05f;
        m_Chapter02.FadeSpeed = 0.05f;
        m_Chapter03.FadeSpeed = 0.05f;
        m_Chapter04.FadeSpeed = 0.05f;
        m_Chapter05.FadeSpeed = 0.05f;
        m_Chapter06.FadeSpeed = 0.05f;
        m_Chapter07.FadeSpeed = 0.05f;

        m_Chapter00.MultiLanguagePorted = true;
        m_Chapter01.MultiLanguagePorted = true;
        m_Chapter02.MultiLanguagePorted = true;
        m_Chapter03.MultiLanguagePorted = true;
        m_Chapter04.MultiLanguagePorted = true;
        m_Chapter05.MultiLanguagePorted = true;
        m_Chapter06.MultiLanguagePorted = true;
        m_Chapter07.MultiLanguagePorted = true;
    }

    void FadeOut()
    {
        m_Chapter00.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter01.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter02.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter03.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter04.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter05.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter06.DesiredAlpha = 0.0f / 255.0f;
        m_Chapter07.DesiredAlpha = 0.0f / 255.0f;

        m_Chapter00.FadeSpeed = 0.2f;
        m_Chapter01.FadeSpeed = 0.2f;
        m_Chapter02.FadeSpeed = 0.2f;
        m_Chapter03.FadeSpeed = 0.2f;
        m_Chapter04.FadeSpeed = 0.2f;
        m_Chapter05.FadeSpeed = 0.2f;
        m_Chapter06.FadeSpeed = 0.2f;
        m_Chapter07.FadeSpeed = 0.2f;

        if (m_Chapter00.ZeroAlpha &&
            m_Chapter01.ZeroAlpha &&
            m_Chapter02.ZeroAlpha &&
            m_Chapter03.ZeroAlpha &&
            m_Chapter04.ZeroAlpha &&
            m_Chapter05.ZeroAlpha &&
            m_Chapter06.ZeroAlpha &&
            m_Chapter07.ZeroAlpha)
        {
            Destroy(this);
        }
    }

    void SmoothToSelection(float desiredSelection)
    {
        float tempY = m_StageSelector.transform.position.y;
        tempY = Mathf.Lerp(tempY, desiredSelection, 0.2f);
        if (Mathf.Abs(tempY - desiredSelection) <= 1.0f)
        {
            m_CurrentStageSelection = m_DesiredStageSelection;
        }
        m_StageSelector.transform.position = new Vector3(m_StageSelector.transform.position.x,
                                                         tempY,
                                                         m_StageSelector.transform.position.z);
    }

    void RecieveInput()
    {
        var deadZone = 0.1f;
        if (Input.GetAxis("Vertical") > deadZone)
        {
            var status = m_CurrentStageSelection;
            status--;
            if (status < StageSelection.Chapter00)
            {
                status = StageSelection.Chapter00;
            }
            m_DesiredStageSelection = status;
        }
        else if (Input.GetAxis("Vertical") < -deadZone)
        {
            var status = m_CurrentStageSelection;
            status++;
            if (status > StageSelection.Chapter07)
            {
                status = StageSelection.Chapter07;
            }
            m_DesiredStageSelection = status;
        }
        if (Input.GetButtonDown("Confirm"))
        {
            switch (m_DesiredStageSelection)
            {
                case StageSelection.Chapter00:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_01");
                    break;
                case StageSelection.Chapter01:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_02");
                    break;
                case StageSelection.Chapter02:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_03");
                    break;
                case StageSelection.Chapter03:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_04");
                    break;
                case StageSelection.Chapter04:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_05");
                    break;
                case StageSelection.Chapter05:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_06");
                    break;
                case StageSelection.Chapter06:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_07");
                    break;
                case StageSelection.Chapter07:
                    M_MainMenuController.CONTROLLER.SetDesiredOpenStage("Turtorial_Level_08");
                    break;
                default:
                    return;
            }
            M_MainMenuController.CONTROLLER.FadeOutPreperation();
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.FADE_OUT);
            m_MenuStatus = StageMenuStatus.FadeOut;
            CanConfirm = true;
        }
    }

    void CheckCancel()
    {
        if (Input.GetButtonDown("Back"))
        {
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_ASSEMBLE);
            this.m_MenuStatus = StageMenuStatus.FadeOut;
        }
    }

    float GetDesiredSelectionPosition(StageSelection selection)
    {
        m_Chapter00.FadeSpeed = 0.2f;
        m_Chapter01.FadeSpeed = 0.2f;
        m_Chapter02.FadeSpeed = 0.2f;
        m_Chapter03.FadeSpeed = 0.2f;
        m_Chapter04.FadeSpeed = 0.2f;
        m_Chapter05.FadeSpeed = 0.2f;
        m_Chapter06.FadeSpeed = 0.2f;
        m_Chapter07.FadeSpeed = 0.2f;
        switch (selection)
        {
            case StageSelection.Chapter00:
                m_Chapter00.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 0.0f;
            case StageSelection.Chapter01:
                m_Chapter00.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 20.0f;
            case StageSelection.Chapter02:
                m_Chapter00.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 40.0f;
            case StageSelection.Chapter03:
                m_Chapter00.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 60.0f;
            case StageSelection.Chapter04:
                m_Chapter00.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 80.0f;
            case StageSelection.Chapter05:
                m_Chapter00.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 50.0f / 255.0f;
                return 100.0f;
            case StageSelection.Chapter06:
                m_Chapter00.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 100.0f / 255.0f;
                return 120.0f;
            case StageSelection.Chapter07:
                m_Chapter00.DesiredAlpha = 255.0f / 255.0f;
                m_Chapter01.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter02.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter03.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter04.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter05.DesiredAlpha = 50.0f / 255.0f;
                m_Chapter06.DesiredAlpha = 100.0f / 255.0f;
                m_Chapter07.DesiredAlpha = 255.0f / 255.0f;
                return 140.0f;
        }
        return 0.0f;
    }
    #endregion
}