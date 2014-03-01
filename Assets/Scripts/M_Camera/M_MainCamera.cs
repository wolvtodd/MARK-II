using UnityEngine;
using System.Collections;

/* クラス説明
 * 
 *      すべてのカメラワーク調整
 *      targetLookAt調整
 *      ズームファンクション
 * 
 *      Edited By   チンカエン
 * */

public class M_MainCamera : MonoBehaviour
{
    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_MainCamera  INSTANCE;                                                                                           //このクラスの実例
    
    private const float         m_CAMERA_LIMIT                                                                      = 40f;          //カメラの移動範囲
    private const float         m_TARGETLOOKAT_SMOOTH_X                                                             = 0.3f;         //方向が回転するとき、targetLookAtのスムーズ時間
    private const float         m_TARGETLOOKAT_SMOOTH_Y                                                             = 0.3f;
    private const float         m_ZOOM_SPEED                                                                        = 60f;          //カメラズームのスピード
    private const float         m_CAM_POS_Z_SMOOTH                                                                  = 0.3f;         //カメラのスムーズ時間
    private const float         m_CAM_POS_Z_MIN                                                                     = -15;          //カメラの最小Z座標
    private const float         m_CAM_POS_Z_MAX                                                                     = -90;          //カメラの最大Z座標
    private const float         m_TARGETLOOKAT_DISTANCE_MIN                                                         = 0;            //targetLookAtの最小移動距離
    private const float         m_TARGETLOOKAT_DISTANCE_MAX                                                         = 5;            //targetLookAtの最大移動距離
    private const float         m_TARGETLOOKAT_CLIMBING_SMOOTH_X                                                    = 0.2f;
    private const float         m_TARGETLOOKAT_CLIMBING_SMOOTH_Y                                                    = 0.2f;
    

    public Transform            TargetLookAt;                                                                                       //カメラが注目ターゲット
    public float                VelTargetLookAtSmoothX                                                              = 0.1f;         //方向が回転するとき、targetLookAtのスムーズ速度
    public float                VelTargetLookAtSmoothY                                                              = 0.1f;
    public float                TargetBeyondBase                                                                    = 6.5f;         //targetLookAtのY方向修正
    public float                ZoomRate                                                                            = 0f;    
    public bool                 CheckIfTwoPlayersAreInSight;

    public struct ClipPlanePoints
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
    }                                                                                                                               //カメラ視点範囲の点

    private float               m_DistanceXforTargetMoveTo                                                          = 0f;           //targetLookAtの移動距離
    private float               m_CamPosX                                                                           = 0f;           //カメラのX座標
    private float               m_CamPosY                                                                           = 0f;           //カメラのY座標
    private float               m_CamPosZ                                                                           = -30f;           //カメラのZ座標
    private float               m_TargetPosX                                                                        = 0f;           //targetLookAtのX座標
    private float               m_TargetPosY                                                                        = 0f;
    private float               m_TargetLookAtDistance                                                              = 0f;           //targetLookAtの移動距離
    private float               m_TargetLookAtClimbingX                                                             = 0f;
    private float               m_TargetLookAtClimbingY                                                             = 0f;
    private float               m_VelTargetLookAtClimbingX                                                          = 0.1f;
    private float               m_VelTargetLookAtClimbingY                                                          = 0.1f;
    private Vector3             m_TargetLookAtBase                                                                  = Vector3.zero; //targetLookAtの基準
    private Vector3             m_TargetLookAtBaseMax                                                               = Vector3.zero; //targetLookAtの最大基準

    #endregion;


    #region Function;

    /* *
     * 初期化に関するメソッド
     * */

    //unityを起動するときに実行します
    void Awake()
    {
        INSTANCE = this;                                                                                                            //このクラスを実例します
    }

    //クラスを初期化します
    void Start()
    {
        Reset();
    }

    //クラスを初期化に関するメソッド
    void Reset()
    {
        m_CamPosX = TargetLookAt.position.x;
        m_CamPosY = TargetLookAt.position.y;

        Camera.mainCamera.transform.position = new Vector3(m_CamPosX,
                                                           m_CamPosY,
                                                           m_CamPosZ);
    }

    /* *
     * 毎一フレームで実行します
     * LateUpdate()は、Update()ファンクションが実行した後で実行します
     * */

    void LateUpdate()
    {
        transform.LookAt(TargetLookAt);
        ProcessTargetLookAtMoveDistance();
        InterpolateTargetDistance(m_DistanceXforTargetMoveTo);
        UpdateCameraPosition();
    }

    /* *
     * カメラワークメソッド
     * */

    //カメラワークを処理します
    void UpdateCameraPosition()
    {
        SetTargetLookAtLimit();
        ProcessCameraPosition();
    }

    //カメラの位置を処理します
    void ProcessCameraPosition()
    {
        Camera.mainCamera.transform.position = new Vector3(TargetLookAt.position.x,                                                 //カメラのX座標　＝　targetLookAtのX座標
                                                           TargetLookAt.position.y,                                                 //カメラのY座標　＝　targetLookAtのY座標
                                                           m_CamPosZ);                                                              //カメラのZ座標　＝　カメラのZ座標
    }

    //targetLookAtの位置に移動することを補間します
    void InterpolateTargetDistance(float distance)
    {
        if ((M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark &&
            M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 90) ||
            (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2 &&
            M_Motor_Mark2.INSTANCE.RotationYtoTurnTo == 90))                                                                        //targetLookAtを右方向に移動したら
            m_DistanceXforTargetMoveTo = m_TargetLookAtBase.x + m_TargetLookAtDistance;                                             //targetLookAtの移動距離を計算します
        if ((M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark &&
             M_Motor_Mark.INSTANCE.RotationYtoTurnTo == 270) ||
            (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2 && 
            M_Motor_Mark2.INSTANCE.RotationYtoTurnTo == 270))                                                                       //targetLookAtを左方向に移動したら
            m_DistanceXforTargetMoveTo = m_TargetLookAtBase.x - m_TargetLookAtDistance;                                             //targetLookAtの移動距離を計算します

        m_TargetPosX = Mathf.SmoothDamp(TargetLookAt.transform.position.x,                                                          //毎フレームでのtargetLookAtの位置をスムーズで計算します
                                        distance,
                                        ref VelTargetLookAtSmoothX,
                                        m_TARGETLOOKAT_SMOOTH_X);
        m_TargetPosY = Mathf.SmoothDamp(TargetLookAt.transform.position.y,
                                        m_TargetLookAtBase.y,
                                        ref VelTargetLookAtSmoothY,
                                        m_TARGETLOOKAT_SMOOTH_Y);
        TargetLookAt.transform.position = new Vector3(m_TargetPosX,                                                                 //targetLookAtの座標を処理します
                                                      m_TargetPosY,
                                                      TargetLookAt.transform.position.z);
    }

    //カメラの視点範囲をズームします
    //void Zoom()
    //{
    //    var deadZone = 0.01f;                                                                                                       //マウススクロールウィーるの反応範囲を設定します

    //    if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > deadZone)                                                                         //反応範囲内となったら
    //    {
    //        m_DesiredCamPosZ += Input.GetAxis("Mouse ScrollWheel") * m_ZOOM_SPEED;                                                  //次のカメラZ座標を計算します
    //        m_DesiredCamPosZ += M_MousePlayerController.INSTANCE.CameraZoomRate;
    //        m_DesiredCamPosZ = Mathf.Clamp(m_DesiredCamPosZ,                                                                        //次のカメラZ座標を範囲内でのことを確認します
    //                                       m_CAM_POS_Z_MAX,
    //                                       m_CAM_POS_Z_MIN);
    //    }

    //    m_CamPosZ = Mathf.SmoothDamp(m_CamPosZ,                                                                                     //カメラZ座標から
    //                                 m_DesiredCamPosZ,                                                                              //次のカメラZ座標に
    //                                 ref m_VelCamPosZSmooth,                                                                        //スムーズします
    //                                 m_CAM_POS_Z_SMOOTH);

    //    m_CamPosZ = Mathf.Clamp(m_CamPosZ,                                                                                          //カメラZ座標を範囲内でのことを確認します
    //                            m_CAM_POS_Z_MAX,
    //                            m_CAM_POS_Z_MIN);
    //}

    //ズームしているときtargetLookAtの位置を処理します
    void ProcessTargetLookAtMoveDistance()
    {
        ZoomRate = GetZoomRate(ZoomRate);
        m_TargetLookAtBase = CalculateTargetLookAtBasePosition(m_TargetLookAtBase);
        m_TargetLookAtDistance = CalculateTargetLookAtDistance(m_TargetLookAtDistance);
    }

    //ズーム時、すべてのデータの変更率を計算します
    float GetZoomRate(float rate)
    {
        rate = (m_CAM_POS_Z_MAX - m_CamPosZ) / (m_CAM_POS_Z_MAX - m_CAM_POS_Z_MIN);

        return rate;
    }

    //ズームしているときtargetLookAtの移動距離を計算します
    float CalculateTargetLookAtDistance(float moveDistance)
    {
        moveDistance = Mathf.Clamp(m_TARGETLOOKAT_DISTANCE_MAX * ZoomRate,
                                   m_TARGETLOOKAT_DISTANCE_MIN, 
                                   m_TARGETLOOKAT_DISTANCE_MAX);

        return moveDistance;
    }

    //targetLookAtの基準座標を計算します
    Vector3 CalculateTargetLookAtBasePosition(Vector3 position)
    {
        if (M_Animator_Mark.INSTANCE.CurrentMarkAnimeState != M_Animator_Mark.PlayerState.CLIMBING)
        {
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
                m_TargetLookAtBaseMax = new Vector3(M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.x,                     //targetLookAtのリミットは、(0, 0, 0)から、
                                                    M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.y + 
                                                    TargetBeyondBase,                                                               //キャラクター自分の現在座標までとなります。
                                                    M_Controller_Mark.MARK_CHARCONTROLLER.transform.position.z);
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
                m_TargetLookAtBaseMax = new Vector3(M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x,                   //targetLookAtのリミットは、(0, 0, 0)から、
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y + 
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.height * 0.7f,
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.z);
        }
        else
        {
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark)
            {
                SmoothCameraWhenCLIMBING();
                m_TargetLookAtBaseMax = new Vector3(m_TargetLookAtClimbingX,
                                                    m_TargetLookAtClimbingY,
                                                    TargetLookAt.position.z);
            }
            if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection == M_PlayerControllerSupport.PlayerSelection.Mark2)
            {
                m_TargetLookAtBaseMax = new Vector3(M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.x,                   //targetLookAtのリミットは、(0, 0, 0)から、
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.y + 
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.height * 0.7f,                          //キャラクター自分の現在座標までとなります。
                                                    M_Controller_Mark2.MARK2_CHARCONTROLLER.transform.position.z);
            }
        }
        position = m_TargetLookAtBaseMax * ZoomRate;

        return position;
    }

    //targetLookAtのリミットを設定します
    void SetTargetLookAtLimit()
    {
        if (TargetLookAt.position.x > m_CAMERA_LIMIT * ZoomRate)
            TargetLookAt.position = new Vector3(m_CAMERA_LIMIT * ZoomRate,
                                                TargetLookAt.position.y,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.x < -m_CAMERA_LIMIT * ZoomRate)
            TargetLookAt.position = new Vector3(-m_CAMERA_LIMIT * ZoomRate,
                                                TargetLookAt.position.y,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.y > m_CAMERA_LIMIT * ZoomRate)
            TargetLookAt.position = new Vector3(TargetLookAt.position.x,
                                                m_CAMERA_LIMIT * ZoomRate,
                                                TargetLookAt.position.z);
        if (TargetLookAt.position.y < -m_CAMERA_LIMIT * ZoomRate)
            TargetLookAt.position = new Vector3(TargetLookAt.position.x,
                                                -m_CAMERA_LIMIT * ZoomRate,
                                                TargetLookAt.position.z);
    }

    void SmoothCameraWhenCLIMBING()
    {
        m_TargetLookAtClimbingX = Mathf.SmoothDamp(m_TargetLookAtBaseMax.x,
                                                   M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.x,
                                                   ref m_VelTargetLookAtClimbingX,
                                                   m_TARGETLOOKAT_CLIMBING_SMOOTH_X);
        m_TargetLookAtClimbingY = Mathf.SmoothDamp(m_TargetLookAtBaseMax.y,
                                                   M_ClimPointSenser.INSTANCE.ClimbPoint.transform.position.y +
                                                   TargetBeyondBase,
                                                   ref m_VelTargetLookAtClimbingY,
                                                   m_TARGETLOOKAT_CLIMBING_SMOOTH_Y);
    }

    public static ClipPlanePoints CLIPPLANEPOINTS(Vector3 pos)
    {
        var clipPlanePoints     = new ClipPlanePoints();

        var transform   = Camera.mainCamera.transform;
        var halfFOV     = (Camera.mainCamera.fieldOfView / 2) * Mathf.Rad2Deg;
        var aspect      = Camera.mainCamera.aspect;
        var distance    = Mathf.Abs(Camera.mainCamera.transform.position.z) + 22.5f *(1 - M_MainCamera.INSTANCE.ZoomRate);
        var height      = distance * Mathf.Tan(halfFOV);
        var width       = height * aspect;

        clipPlanePoints.LowerLeft  = pos - transform.right * width;
        clipPlanePoints.LowerLeft -= transform.up * height;

        clipPlanePoints.LowerRight  = pos + transform.right * width;
        clipPlanePoints.LowerRight -= transform.up * height;

        clipPlanePoints.UpperLeft  = pos - transform.right * width;
        clipPlanePoints.UpperLeft += transform.up * height;

        clipPlanePoints.UpperRight  = pos + transform.right * width;
        clipPlanePoints.UpperRight += transform.up * height;

        return clipPlanePoints;
    }

    #endregion 
}
