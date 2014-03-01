using UnityEngine;
using System.Collections;

public class M_BGMPlayer : MonoBehaviour
{
    /* クラス説明
     * 
     *      BGM
     * 
     *      Edited By   チンカエン
     * */

    private static M_BGMPlayer  m_INSTANCE = null;
    public static M_BGMPlayer   INSTANCE
    {
        get { return m_INSTANCE; }
    }

    void Start()
    {
        if (m_INSTANCE != null && m_INSTANCE != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            m_INSTANCE = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
