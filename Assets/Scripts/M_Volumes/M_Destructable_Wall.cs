using UnityEngine;
using System.Collections;

public class M_Destructable_Wall : MonoBehaviour
{
    /* クラス説明
     * 
     *      破壊できる壁の設定
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private bool canWallBroken = false;
    private GameObject m_BreakSmoke;
    private GameObject m_BreakFire;

    private enum wallDestructionStatus
    {
        STANDING,
        FALLING
    }
    private wallDestructionStatus currentWallStatus = wallDestructionStatus.STANDING;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    void Start()
    {
        canWallBroken = false;
        currentWallStatus = wallDestructionStatus.STANDING;
        m_BreakSmoke = Resources.Load("Prefabs/Particles/BreakSmoke") as GameObject;
        m_BreakFire = Resources.Load("Prefabs/Particles/BreakFire") as GameObject;
    }

    void Update()
    {
        WallStatus();
    }

    void WallStatus()
    {
        if (currentWallStatus == wallDestructionStatus.STANDING && CanDestroyWall())
        {
            this.gameObject.collider.enabled = false;
            GameObject.Instantiate(m_BreakSmoke, this.transform.position + Vector3.up * 5, Quaternion.identity);
            GameObject.Instantiate(m_BreakFire, this.transform.position + Vector3.up * 5, Quaternion.identity);
            currentWallStatus = wallDestructionStatus.FALLING;
        }
        else if (currentWallStatus == wallDestructionStatus.FALLING)
        {
            ProcessDestruction();
        }
    }

    void ProcessDestruction()
    {
        var desiredZ        = Mathf.Lerp(this.transform.localEulerAngles.z, 90, 0.2f);
        var meshRenderer    = GetComponentsInChildren<Renderer>();
        var desiredAlpha    = Mathf.Lerp(meshRenderer[0].material.color.a, 0, 0.1f);
        if (this.transform.localEulerAngles.z >= 89)
        {
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x,
                                                          this.transform.localEulerAngles.y,
                                                          90);
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                if (meshRenderer[i].material.color.a <= 0.01f)
                {
                    GameObject.Destroy(this.gameObject);
                    return;
                }
                meshRenderer[i].material.color = new Color(meshRenderer[0].material.color.r,
                                                           meshRenderer[0].material.color.g,
                                                           meshRenderer[0].material.color.b,
                                                           desiredAlpha);
            }
            return;

        }
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x,
                                                      this.transform.localEulerAngles.y,
                                                      desiredZ);
    }

    bool CanDestroyWall()
    {
        if (canWallBroken && M_Motor_Mark2.INSTANCE.IsMark2Attacking)
        {
            return true;
        }
        else
            return false;
    }

    void OnTriggerStay(Collider attackVolume)
    {
        if (attackVolume.tag == "AttackVolume")
        {
            canWallBroken = true;
        }
    }

    void OnTriggerExit(Collider attackVolume)
    {
        if (attackVolume.tag == "AttackVolume")
        {
            canWallBroken = false;
        }
    }

    #endregion
}
