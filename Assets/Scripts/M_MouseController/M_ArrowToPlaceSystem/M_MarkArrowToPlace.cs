using UnityEngine;

public class M_MarkArrowToPlace : M_ArrowToPlace
{
    /* クラス説明
     * 
     *      Markが行きたいところに矢印を生成します
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_MarkArrowToPlace INSTANCE;

    public GameObject   ArrowDust;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
    }

    //クラスを初期化します
    void Start()
    {
        //EndPosition = M_MousePlayerController.INSTANCE.MouseArrowStickPosition();
        this.gameObject.renderer.material.renderQueue = QueueArrow;
    }

    void Update()
    {
        CheckIfDestroyOnNewClick();
        CheckIfDestroyOnCloseToPlayer();
        if (transform.position.y > EndPosition.y + 2f)
        {
            ApplyGravityAndDrop();
        }
        else
        {
            if (transform.localEulerAngles.z == 0 && this.gameObject.renderer.material.color.a == 1)
            {
                Instantiate(ArrowDust, EndPosition, Quaternion.Euler(270, 0, 0));
            }
            StickIntoGroundAndRotate();
        }
        FadeOutAndKillMe();
    }

    void CheckIfDestroyOnNewClick()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
        {
            if (Input.GetMouseButtonUp(0))
            {
                MatAlphaDecreaseSpeed = 5f;
                CanDestroy = true;
            }
        }
    }

    #endregion
}
