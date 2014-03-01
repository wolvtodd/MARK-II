using UnityEngine;
using System.Collections;

public class M_InDoorMain : MonoBehaviour
{
    /* クラス説明
     * 
     *      入り口の処理。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    private GameObject  Door;
    private GameObject  DoorModel;
    private const int   m_RenderQueue           = 3003;
    private const float m_DesiredShutEulerAngle = 0f;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        Door        = GameObject.Find("InDoor") as GameObject;
        DoorModel   = GameObject.Find("InDoorModel") as GameObject;

        DoorModel.renderer.material.renderQueue = m_RenderQueue;
    }

    void Update()
    {
        RotateDoor();
    }

    void RotateDoor()
    {
        if (M_GameMain.INSTANCE.CurrentGameStatus == Const.GAME_STATUS.Playing)
        {
            if (Door.transform.localEulerAngles.y <= 360 && Door.transform.localEulerAngles.y >= 270)
            {
                Door.transform.localEulerAngles = Vector3.zero;
                Destroy(this);
                return;
            }
            Door.transform.Rotate(0, -360 * Time.deltaTime, 0);
        }
    }

    #endregion
}
