using UnityEngine;
using System.Collections;

public class M_AboutProducerController : MonoBehaviour
{
    /* クラス説明
     * 
     *      作者についての処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    enum AboutProducerStatus
    {
        Selecting,
        FadeOut
    }
    private AboutProducerStatus m_AboutProducerStatus;

    private Material m_Material;

    #endregion

    #region Functions

    void Start()
    {
        m_AboutProducerStatus = AboutProducerStatus.Selecting;
        m_Material = GameObject.Find("AboutTheAuthor").GetComponent<Renderer>().material;
        m_Material.color = new Color(m_Material.color.r,
                                     m_Material.color.g,
                                     m_Material.color.b,
                                     0.0f);
    }

    void Update()
    {
        if (m_AboutProducerStatus == AboutProducerStatus.Selecting)
        {
            FadeIn();
            RecieveInput();
        }
        else
        {
            FadeOut();
        }
    }

    void RecieveInput()
    {
        if (Input.GetButtonDown("Confirm") ||
            Input.GetButtonDown("Back"))
        {
            m_AboutProducerStatus = AboutProducerStatus.FadeOut;
            M_MainMenuController.CONTROLLER.SetMainMenuStatus(M_MainMenuController.MAIN_MENU_STATUS.MAIN_MENU_ASSEMBLE);
        }
    }

    void FadeIn()
    {
        float tempAlpha = m_Material.color.a;
        tempAlpha += 5.0f * Time.deltaTime;
        if (tempAlpha > 1.0f)
        {
            tempAlpha = 1.0f;
        }
        m_Material.color = new Color(m_Material.color.r,
                                     m_Material.color.g,
                                     m_Material.color.b,
                                     tempAlpha);
    }

    void FadeOut()
    {
        float tempAlpha = m_Material.color.a;
        tempAlpha -= 5.0f * Time.deltaTime;
        if (tempAlpha < 0.0f)
        {
            tempAlpha = 0.0f;
            Destroy(this);
        }
        m_Material.color = new Color(m_Material.color.r,
                                     m_Material.color.g,
                                     m_Material.color.b,
                                     tempAlpha);
    }


    #endregion
}