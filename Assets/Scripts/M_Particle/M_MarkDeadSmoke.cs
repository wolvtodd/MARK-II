using UnityEngine;
using System.Collections;

public class M_MarkDeadSmoke : MonoBehaviour
{
    /* クラス説明
     * 
     *      マークが電死するときの煙
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    private ParticleSystem[] smokes;

    #endregion



    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    void Start()
    {
        smokes = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < smokes.Length; i++)
        {
            if (smokes[i].gameObject.GetComponent<M_SingleShotParticle>() == null)
            {
                smokes[i].gameObject.AddComponent<M_SingleShotParticle>();
            }
        }
    }

    void Update()
    {
        if (CanGameOver())
        {
            M_GameMain.INSTANCE.CurrentGameStatus = Const.GAME_STATUS.FadeOut;
        }
    }

    bool CanGameOver()
    {
        for (int i = 0; i < smokes.Length; i++)
        {
            if (smokes[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    #endregion
}
