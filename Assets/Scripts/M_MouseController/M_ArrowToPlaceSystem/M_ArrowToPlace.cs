using UnityEngine;
using System.Collections;

public class M_ArrowToPlace : MonoBehaviour
{
    /* クラス説明
     * 
     *      行きたいところに矢印を生成します
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    protected float     Gravity                   = 2f;
    protected float     MatAlpha                  = 1f;
    protected Vector3   EndPosition               = Vector3.zero;
    protected Vector3   MoveVector                = Vector3.zero;

    protected int       ArrowRotateRate           = 0;
    protected int       QueueArrow                = 3003;
    protected bool      CanDestroy                = false;
    protected float     MatAlphaDecreaseSpeed     = 1f;
    protected float     DistanceToPlayer          = 0f;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    public virtual void ApplyGravityAndDrop()
    {
        MoveVector.y -= Gravity * Time.deltaTime;
        transform.Translate(MoveVector, Space.World);
        ArrowRotateRate = Random.Range(0, 9);
    }

    public virtual void StickIntoGroundAndRotate()
    {
        if (ArrowRotateRate < 5f)
        {
            transform.Rotate(Vector3.forward * 540f * Time.deltaTime);
            if (transform.localEulerAngles.z > 15f)
            {
                MatAlphaDecreaseSpeed = 1f;
                transform.localEulerAngles = new Vector3(0, 180, 15);
                FadeOutAndKillMe();
            }
        }
        else
        {
            transform.Rotate(Vector3.back * 540f * Time.deltaTime);
            if (transform.localEulerAngles.z < 345f)
            {
                MatAlphaDecreaseSpeed = 1f;
                transform.localEulerAngles = new Vector3(0, 180, 345);
            }
        }
    }

    public virtual void FadeOutAndKillMe()
    {
        if (CanDestroy)
        {
            MatAlpha -= MatAlphaDecreaseSpeed * Time.deltaTime;
            this.gameObject.renderer.material.color = new Color(this.gameObject.renderer.material.color.r,
                                                                this.gameObject.renderer.material.color.g,
                                                                this.gameObject.renderer.material.color.b,
                                                                MatAlpha);
            if (MatAlpha <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public virtual void CheckIfDestroyOnCloseToPlayer()
    {
        Vector3 playerPosition;

        playerPosition = M_Controller_Mark.MARK_CHARCONTROLLER.transform.position;

        if (Mathf.Abs(this.gameObject.transform.position.x - playerPosition.x) < 2.5f)
        {
            MatAlphaDecreaseSpeed = 1f;
            CanDestroy = true;
        }
    }

    #endregion
}
