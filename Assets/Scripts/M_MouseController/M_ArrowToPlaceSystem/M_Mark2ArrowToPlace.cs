using UnityEngine;

public class M_Mark2ArrowToPlace : M_ArrowToPlace
{
    /* クラス説明
     * 
     *      Mark2が行きたいところに矢印を生成します
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_Mark2ArrowToPlace INSTANCE;

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
            StickIntoGroundAndRotate();
        }
        FadeOutAndKillMe();
    }

    void CheckIfDestroyOnNewClick()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
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
