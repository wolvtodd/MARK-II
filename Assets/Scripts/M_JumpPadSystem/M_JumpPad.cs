using UnityEngine;
using System.Collections;

public class M_JumpPad : MonoBehaviour
{
    /* クラス説明
     * 
     *      ジャンプパッド本体操作。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public M_JumpPadMain    ParentJumpPad;
    public GameObject       ElectricRenderers;
    public BoxCollider      JumpPadVolume;

    //[SerializeField]
    //private Transform       m_Hint;
    //private Vector3         m_HintPoint;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        ParentJumpPad = transform.parent.GetComponent<M_JumpPadMain>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "ElectricPlanes")
            {
                ElectricRenderers = child.gameObject;
            }
            if (child.gameObject.name == "JumpPad")
            {
                JumpPadVolume = child.GetComponent<BoxCollider>() as BoxCollider;
            }
        }
    }

    public void UpdateJumpPad()
    {
        if (ParentJumpPad.IsActivated)
        {
            foreach (Transform child in ElectricRenderers.transform)
            {
                child.renderer.material.renderQueue = 3003;
                child.renderer.enabled = true;
            }
            if (IsMark2OnPad())
            {
                Vector3 electricShockPosition                               = JumpPadVolume.transform.position + Vector3.up * JumpPadVolume.size.y / 2;
                GameObject.Instantiate(Resources.Load("Prefabs/Particles/Mark-II_electricShock") as GameObject, electricShockPosition, Quaternion.Euler(270f, 0f, 0f));
                M_Motor_Mark2.INSTANCE.VerticalSpeed                        = M_Motor_Mark2.INSTANCE.JumpSpeed;
                M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection   = M_PlayerControllerSupport.PlayerSelection.Mark2;
                ParentJumpPad.Mark2AddOnSpeed                               = 20f * M_Motor_Mark2.INSTANCE.CharDirection;
                ParentJumpPad.IsActivated                                   = false;
                ParentJumpPad.ToDeactive                                    = true;
            }
            if (IsMarkOnPad())
            {
                if(!M_Animator_Mark.INSTANCE.animation.IsPlaying("electricShock"))
                {
                    M_PlayerControllerSupport.INSTANCE.CurrentPlayerSelection = M_PlayerControllerSupport.PlayerSelection.Mark;
                    M_Motor_Mark.INSTANCE.IsDying = true;
                    M_Motor_Mark.INSTANCE.CanPlayDeathAnimation = true;
                    M_GameMain.INSTANCE.CurrentGameStatus = Const.GAME_STATUS.GameOver;
                }
            }
        }
        else
        {
            foreach (Transform child in ElectricRenderers.transform)
            {
                child.renderer.enabled = false;
            }
            if (M_Controller_Mark2.MARK2_CHARCONTROLLER.isGrounded)
            {
                ParentJumpPad.Mark2AddOnSpeed = 0f;
            }
        }
    }

    bool IsMark2OnPad()
    {
        var player          = GameObject.FindGameObjectWithTag("SubPlayer").transform as Transform;
        var playerPosition  = player.position;
        var playerHeight    = player.GetComponent<CharacterController>().height;
        var switchPosition  = JumpPadVolume.transform.position;
        var switchSize      = JumpPadVolume.size;

        if (playerPosition.x >= switchPosition.x - switchSize.x &&
            playerPosition.x <= switchPosition.x + switchSize.x &&
            playerPosition.y + (playerHeight / 3) >= switchPosition.y &&
            playerPosition.y + (playerHeight / 3) <= switchPosition.y + switchSize.y)
        {
            M_Motor_Mark2.INSTANCE.CurrentOnJumpPad = this.ParentJumpPad;
            return true;
        }
        
        return false;
    }

    bool IsMarkOnPad()
    {
        var player          = GameObject.FindGameObjectWithTag("Player").transform as Transform;
        var playerPosition  = player.position;
        var playerHeight    = player.GetComponent<CharacterController>().height;
        var switchPosition  = JumpPadVolume.transform.position;
        var switchSize      = JumpPadVolume.size;

        if (playerPosition.x >= switchPosition.x - switchSize.x &&
            playerPosition.x <= switchPosition.x + switchSize.x &&
            playerPosition.y + (playerHeight / 3) >= switchPosition.y &&
            playerPosition.y + (playerHeight / 3) <= switchPosition.y + switchSize.y)
        {
            return true;
        }
        return false;
    }

    #endregion
}
