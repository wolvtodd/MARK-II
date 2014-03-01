using UnityEngine;
using System.Collections;

public class M_MainMenuCamera : MonoBehaviour
{
    /* クラス説明
     * 
     *      MainMenu用のカメラ処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */
    private float  m_LimitX                = 2.0f;
    private float  m_LimitY                = 2.5f;

    private Vector3         m_CameraPosVectorBase   = Vector3.zero;
    private Vector3         m_CameraPosVector       = Vector3.zero;
    
    #endregion

    #region Function

    void Start()
    {
        m_CameraPosVectorBase   = Vector3.zero;
        m_CameraPosVector       = Vector3.zero;
    }

    void CalculateMove()
    {
        float tempBaseX = Mathf.Lerp(m_CameraPosVectorBase.x, M_MainMenuController.CONTROLLER.DesiredCameraPos, 100.0f);
        m_CameraPosVectorBase = Vector3.right * tempBaseX;

        m_CameraPosVector = this.transform.position;
        m_CameraPosVector.x = Mathf.Lerp(m_CameraPosVector.x, m_LimitX + m_CameraPosVectorBase.x, 0.5f * Time.deltaTime);
        m_CameraPosVector.y = Mathf.Lerp(m_CameraPosVector.y, m_LimitY + m_CameraPosVectorBase.y, 0.5f * Time.deltaTime);
        if (Mathf.Abs(m_CameraPosVector.x - (m_LimitX + m_CameraPosVectorBase.x)) <= 0.25f)
        {
            m_LimitX = -m_LimitX;
        }
        if (Mathf.Abs(m_CameraPosVector.y - (m_LimitY + m_CameraPosVectorBase.y)) <= 0.25f)
        {
            m_LimitY = -m_LimitY;
        }
    }

    void Update()
    {
        CalculateMove();
        this.transform.position = m_CameraPosVector;
    }

    public void SetCameraPositionBase(float x)
    {
        m_CameraPosVectorBase = new Vector3(x, 0.0f, 0.0f);
    }

    #endregion
}