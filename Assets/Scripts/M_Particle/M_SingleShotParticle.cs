using UnityEngine;
using System.Collections;

public class M_SingleShotParticle : MonoBehaviour
{
    /* クラス説明
     * 
     *      パーティクルがループして、消える。
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    private ParticleSystem thisParticle;

    #endregion

    #region Function

    void Start()
    {
        thisParticle        = this.GetComponent<ParticleSystem>() as ParticleSystem;
        thisParticle.loop   = false;
    }

    void Update()
    {
        if (!thisParticle.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }

    #endregion
}
