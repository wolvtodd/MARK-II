using UnityEngine;
using System.Collections;

public class M_GUIController : MonoBehaviour
{
    /* クラス説明
     * 
     *      GUI操作
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_GUIController INSTANCE;

    private const float DEFAULT_ASPECT                  = 1.6f;
    private const float DEFAULT_PLAYERSWITCHER_ASPECT   = 16f / 9f;
    private const float DEFAULT_MARKGUI_POS             = -290f;
    private const float DEFAULT_MARK2GUI_POS            = 290f;

    private const float GUI_POSITIONX_MARK_SELECTED     = 155f;
    private const float GUI_POSITIONX_MARK2_SELECTED    = -155f;
    private float       GUIPosXSmooth                   = 5f;

    public static bool IS_CONTROLLING_GUI               = false;

    public GameObject[] GUIAssets;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    void Start()
    {
        INSTANCE     = this;
        GUIAssets[0] = GameObject.Find("PlayerSwitcher")    as GameObject;
        GUIAssets[1] = GameObject.Find("MarkGUI")           as GameObject;
        GUIAssets[2] = GameObject.Find("Mark2GUI")          as GameObject;

        GUIAssets[0].transform.localPosition = Vector3.zero;
        GUIAssets[1].transform.localPosition = Vector3.right * DEFAULT_MARKGUI_POS;
        GUIAssets[2].transform.localPosition = Vector3.right * DEFAULT_MARK2GUI_POS;
        CheckGUIAssetsPosition();
    }

    void Update()
    {
        transform.position = new Vector3(GUIPosX(), transform.position.y, transform.position.z);
        UpdatePlayerSwitcherPosition();
    }

    float GUIPosX()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
        {
            return Mathf.Lerp(transform.position.x, GUI_POSITIONX_MARK2_SELECTED, GUIPosXSmooth * Time.deltaTime);
        }
        else
        {
            return Mathf.Lerp(transform.position.x, GUI_POSITIONX_MARK_SELECTED, GUIPosXSmooth * Time.deltaTime);
        }
    }

    void CheckGUIAssetsPosition()
    {
        var currentAspect   = Camera.mainCamera.aspect;
        var aspectHelper    = DEFAULT_ASPECT - currentAspect;

        GUIAssets[1].transform.localPosition += Vector3.right * aspectHelper * 100f;
        GUIAssets[2].transform.localPosition += Vector3.left * aspectHelper * 100f;
    }

    void UpdatePlayerSwitcherPosition()
    {
        var playerSwitcherPositionX     = 0f;
        var currentAspect               = Camera.mainCamera.aspect;
        var aspectHelper                = DEFAULT_PLAYERSWITCHER_ASPECT - currentAspect;
        
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
        {
            playerSwitcherPositionX = Mathf.Lerp(GUIAssets[0].transform.localPosition.x, -aspectHelper * 100f, GUIPosXSmooth / 2 * Time.deltaTime);
        }
        else
        {
            playerSwitcherPositionX = Mathf.Lerp(GUIAssets[0].transform.localPosition.x, aspectHelper * 100f, GUIPosXSmooth / 2 * Time.deltaTime);
        }
        GUIAssets[0].transform.localPosition = new Vector3(playerSwitcherPositionX, GUIAssets[0].transform.localPosition.y, GUIAssets[0].transform.localPosition.z);
    }

    #endregion
}
