using UnityEngine;
using System.Collections;

public class M_SmokeOnMarkII : MonoBehaviour
{
    /* クラス説明
     * 
     *      マークＩＩの煙
     * 
     *      Edited By   チンカエン
     * */

    #region Fields

    /* *
     * すべてのparamを宣言します
     * */

    public Transform[]      Smokes;
    public GameObject       SmokeParticle;
    public GameObject       AllSmokes;

    private float           startTime;
    private float           passingTime;
    private float           limitTime;
    private Color           smokeColor;

    #endregion

    #region Function

    /* *
     * 初期化に関するメソッド
     * */

    //クラスを初期化します
    void Start()
    {
        startTime       = 0;
        passingTime     = startTime;
        limitTime       = Random.Range(0.25f, 0.5f);
        Smokes          = new Transform[3];
        AllSmokes       = new GameObject();
        AllSmokes.name  = "Smokes";
        for (int i = 0; i < Smokes.Length; i++)
        {
            Smokes[i]   = GameObject.Find("SmokePoint_" + (i + 1).ToString()).transform;
        }
        SmokeParticle   = Resources.Load("Prefabs/Particles/Mark-II_Smoke") as GameObject;
        smokeColor      = Color.white;
    }

    void Update()
    {
        passingTime += Time.deltaTime;
        if (passingTime - startTime >= limitTime)
        {
            for (int i = 0; i < Smokes.Length; i++)
            {

                var smoke = GameObject.Instantiate(SmokeParticle,
                                       Smokes[i].transform.position,
                                       SmokeParticle.transform.rotation) as GameObject;
                smoke.transform.parent = AllSmokes.transform;
            }
            passingTime = 0;
            startTime   = passingTime;
            limitTime   = Random.Range(0.25f, 0.5f);
        }

        if (!IsMark2Fading())
        {
            SmokeParticle.GetComponent<ParticleSystem>().startColor = smokeColor;
        }
        else
        {
            SmokeParticle.GetComponent<ParticleSystem>().startColor = new Color(smokeColor.r, smokeColor.g, smokeColor.b, smokeColor.a / 10);
        }
    }

    bool IsMark2Fading()
    {
        if (M_PlayerControllerSupport.INSTANCE.CurrentPlayerFadeState == M_PlayerControllerSupport.PlayerFadeState.fadeMark2)
        {
            return true;
        }
        return false;
    }

    #endregion
}
