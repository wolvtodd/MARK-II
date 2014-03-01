using UnityEngine;
using System.Collections;

public class M_GlobalSetting : MonoBehaviour
{
    /* クラス説明
     * 
     *      グローバル変数設定。
     * 
     *      Edited By   チンカエン
     * */

    private static M_GlobalSetting m_INSTANCE = null;
    public static M_GlobalSetting INSTANCE
    {
        get { return m_INSTANCE; }
    }

    public enum Language
    {
        English,
        Japanese,
        Chinese
    }

    static private Language m_GlobalLanguage;
    static private Vector2 m_LanguageOffset;

    void Start()
    {
        m_GlobalLanguage = Language.English;
        m_LanguageOffset = new Vector2(0.0f, (1.0f / 3) * 2);
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

    public static void SetGlobalLanguage(Language language)
    {
        m_GlobalLanguage = language;
        switch (language)
        {
            case Language.English:
                m_LanguageOffset = new Vector2(0.0f, (1.0f / 3) * 2);
                break;
            case Language.Japanese:
                m_LanguageOffset = new Vector2(0.0f, (1.0f / 3) * 1);
                break;
            case Language.Chinese:
                m_LanguageOffset = new Vector2(0.0f, (1.0f / 3) * 0);
                break;
            default:
                return;
        }
    }

    public static Vector2 GetLanguageOffset()
    {
        return m_LanguageOffset;
    }
}
