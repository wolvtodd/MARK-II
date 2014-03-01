using UnityEngine;
using System.Collections;

public class M_ParticleSystem : MonoBehaviour
{
    /* クラス説明
     * 
     *      パーティクルシステムに関する処理
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public static M_ParticleSystem INSTANCE;

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

    void Update()
    {
        KillMeWhenIFinish();
    }

    void KillMeWhenIFinish()
    {
        if (!this.particleSystem.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }

    #endregion
}
