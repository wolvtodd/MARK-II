using UnityEngine;
using System.Collections;

/* クラス説明
 * 
 *      すべてのサブカメラワーク調整
 *      サブtargetLookAt調整
 *      ズームファンクション
 * 
 *      Edited By   チンカエン
 * */

public class M_SubCamera : MonoBehaviour
{
    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_SubCamera   INSTANCE;                                                                                           //このクラスを実例する
    
    private const float         m_SUBCAMERA_LIMIT                                                                   = 35f;          //サブカメラの最大移動範囲
    private const float         m_TARGETLOOKAT_SMOOTH_X                                                             = 0.3f;         //TargetLookAtX方向でのスムーズ時間
    private const float         m_TARGETLOOKAT_SMOOTH_Y                                                             = 0.3f;         //TargetLookAtY方向でのスムーズ時間
    private const float         m_CAMERA_DISTANCE_Z                                                                 = -20f;         //サブカメラZ方向の位置
    private const float         m_TARGETLOOKAT_BASE_Y_BEYOND                                                        = 2f;           //TargetLookAtY方向補正
    private const float         m_TARGETLOOKAT_DISTANCE                                                             = 5f;           //TargetLookAtのプレーヤへの距離
    private const float         m_SUBCAMERA_SHOW_RECT_X                                                             = 0.67f;        //サブカメラスクリーンを出す条件
    private const float         m_SUBCAMERA_HIDE_RECT_X                                                             = 0.9999f;      //サブカメラスクリーンをハイド条件
    private const float         m_SUBCAMERA_INTERPOLATE_SPEED                                                       = 2f;           //サブカメラスクリーンの移動速度

    public Transform            TargetLookAt;                                                                                       //カメラが注目ターゲット
    public float                VelTargetLookAtSmoothX                                                              = 0.2f;         //方向が回転するとき、targetLookAtのスムーズ速度
    public float                VelTargetLookAtSmoothY                                                              = 0.2f;
    
    private float               m_CurrentSubCameraRectX                                                             = 0f;
    private float               m_CurrentTargetLookAtDistance                                                       = 0f;
    private float               m_TargetPosX                                                                        = 0f;
    private float               m_TargetPosY                                                                        = 0f;
    private Vector3             m_DesiredTargetLookAtPosition                                                       = Vector3.zero;
    private Vector3             m_TargetLookAtBase                                                                  = Vector3.zero; //targetLookAtの基準

    #endregion

    #region Functions

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;
    }

    /* *
     * 毎一フレームで実行します
     * LateUpdate()は、Update()ファンクションが実行した後で実行します
     * */

    void LateUpdate()
    {
        InterpolateSubCamera();
        ProcessTransformLookAt();
        SetSubCameraPosition();
        transform.LookAt(TargetLookAt);
    }

    /* *
     * カメラワークメソッド
     * */

    //カメラの位置を処理します
    void SetSubCameraPosition()
    {
        transform.position = TargetLookAt.position +
                             new Vector3(0, m_TARGETLOOKAT_BASE_Y_BEYOND, m_CAMERA_DISTANCE_Z);                                     //サブカメラの位置を設定する
    }

    //targetLookAtの位置を処理します
    void ProcessTransformLookAt()
    {
        SetTargetLookAtBase();
        SetTargetLookAtPosition();
        InterpolateTargetLookAt();
        SetTargetLookAtLimit();
    }

    void SetTargetLookAtPosition()                                                                                                  //プレーヤの方向によってTargetLookAtの位置を確認する
    {
        if ((M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark &&
             M_Motor_Mark2.INSTANCE.RotationYtoTurnTo == 90) ||
            (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2 &&
             M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 90))
        {
            m_CurrentTargetLookAtDistance = m_TARGETLOOKAT_DISTANCE;
        }
        if ((M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark &&
             M_Motor_Mark2.INSTANCE.RotationYtoTurnTo == 270) ||
            (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2 &&
             M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 270))
        {
            m_CurrentTargetLookAtDistance = -m_TARGETLOOKAT_DISTANCE;
        }

        m_DesiredTargetLookAtPosition = m_TargetLookAtBase +
                                        new Vector3(m_CurrentTargetLookAtDistance, 0, 0);
    }

    void InterpolateTargetLookAt()                                                                                                  //TargetLookAtの位置を補間する
    {
        m_TargetPosX = Mathf.SmoothDamp(TargetLookAt.position.x,
                                        m_DesiredTargetLookAtPosition.x,
                                        ref VelTargetLookAtSmoothX,
                                        m_TARGETLOOKAT_SMOOTH_X);
        m_TargetPosY = Mathf.SmoothDamp(TargetLookAt.position.y,
                                        m_TargetLookAtBase.y,
                                        ref VelTargetLookAtSmoothY,
                                        m_TARGETLOOKAT_SMOOTH_Y);
        TargetLookAt.transform.position = new Vector3(m_TargetPosX, m_TargetPosY, TargetLookAt.position.z);
    }

    void SetTargetLookAtBase()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
            m_TargetLookAtBase = M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position +
                                 Vector3.up * M_Controller_Mark2.MARK2_CHARCONTROLLER.height * 0.7f;
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
            m_TargetLookAtBase = M_Controller_Mark.MARK_CHARCONTROLLER.transform.position +
                                 new Vector3(0, M_MainCamera.INSTANCE.TargetBeyondBase, 0);
    }

    void InterpolateSubCamera()
    {
        if (M_MainCamera.INSTANCE.CheckIfTwoPlayersAreInSight == false)
        {
            this.camera.enabled = true;
            m_CurrentSubCameraRectX -= Time.deltaTime * m_SUBCAMERA_INTERPOLATE_SPEED;
            if (m_CurrentSubCameraRectX < m_SUBCAMERA_SHOW_RECT_X)
                m_CurrentSubCameraRectX = m_SUBCAMERA_SHOW_RECT_X;
        }
        if (M_MainCamera.INSTANCE.CheckIfTwoPlayersAreInSight == true)
        {
            m_CurrentSubCameraRectX += Time.deltaTime * m_SUBCAMERA_INTERPOLATE_SPEED;
            if (m_CurrentSubCameraRectX >= m_SUBCAMERA_HIDE_RECT_X)
            {
                m_CurrentSubCameraRectX = m_SUBCAMERA_HIDE_RECT_X;
                this.camera.enabled = false;
            }
        }
        this.camera.rect = new Rect(m_CurrentSubCameraRectX, 0.05f, 0.3f, 0.3f);
    }

    void SetTargetLookAtLimit()                                                                                                     //カメラの視点リミットを設定する
    {
        if (TargetLookAt.position.x > m_SUBCAMERA_LIMIT)
            TargetLookAt.position = new Vector3(m_SUBCAMERA_LIMIT,
                                                TargetLookAt.position.y,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.x < -m_SUBCAMERA_LIMIT)
            TargetLookAt.position = new Vector3(-m_SUBCAMERA_LIMIT,
                                                TargetLookAt.position.y,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.y > m_SUBCAMERA_LIMIT)
            TargetLookAt.position = new Vector3(TargetLookAt.position.x,
                                                m_SUBCAMERA_LIMIT,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.y < -m_SUBCAMERA_LIMIT)
            TargetLookAt.position = new Vector3(TargetLookAt.position.x,
                                                -m_SUBCAMERA_LIMIT,
                                                TargetLookAt.position.z);
    }

    #endregion
}