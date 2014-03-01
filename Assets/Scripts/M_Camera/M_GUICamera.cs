using UnityEngine;
using System.Collections;

public class M_GUICamera : MonoBehaviour
{
    /* クラス説明
     * 
     *      GUI用カメラの設定の行い
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    ////static:////
    //public:
    public static M_GUICamera INSTANCE
    {
        get { return m_INSTANCE; }
    }

    public struct GUIRect
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
        public Vector3 LowerLeft;
    }

    public static GUIRect CURRENT_GUI_RECT;

    //private:
    private static M_GUICamera m_INSTANCE = null;


    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        SingletonThisClass();
        CURRENT_GUI_RECT = GUI_RECT(transform.position);
    }

    void SingletonThisClass()
    {
        if (m_INSTANCE != null && m_INSTANCE != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            m_INSTANCE = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    public static GUIRect GUI_RECT(Vector3 cameraPos)
    {
        var tempGUIRect = new GUIRect();
        var curGUICamera = GameObject.FindWithTag("GUICamera");
        var halfHeight = curGUICamera.camera.orthographicSize;
        var halfWidth = halfHeight * curGUICamera.camera.aspect;

        tempGUIRect.UpperLeft = cameraPos;
        tempGUIRect.UpperLeft += Vector3.left * halfWidth;
        tempGUIRect.UpperLeft += Vector3.up * halfHeight;

        tempGUIRect.UpperRight = cameraPos;
        tempGUIRect.UpperRight += Vector3.right * halfWidth;
        tempGUIRect.UpperRight += Vector3.up * halfHeight;

        tempGUIRect.LowerRight = cameraPos;
        tempGUIRect.LowerRight += Vector3.right * halfWidth;
        tempGUIRect.LowerRight += Vector3.down * halfHeight;

        tempGUIRect.LowerLeft = cameraPos;
        tempGUIRect.LowerLeft += Vector3.left * halfWidth;
        tempGUIRect.LowerLeft += Vector3.down * halfHeight;

        return tempGUIRect;
    }

    public static Vector2 WorldMousePosition()
    {
        var mouseX = Input.mousePosition.x;
        var mouseY = Input.mousePosition.y;

        mouseX = mouseX / Screen.width;
        mouseX = 1 - mouseX;
        mouseX = CURRENT_GUI_RECT.UpperRight.x - (CURRENT_GUI_RECT.UpperRight.x - CURRENT_GUI_RECT.UpperLeft.x) * mouseX;


        mouseY = mouseY / Screen.height;
        mouseY = 1 - mouseY;
        mouseY = CURRENT_GUI_RECT.UpperRight.y - (CURRENT_GUI_RECT.UpperRight.y - CURRENT_GUI_RECT.LowerLeft.y) * mouseY;

        return new Vector2(mouseX, mouseY);
    }

    #endregion
}
