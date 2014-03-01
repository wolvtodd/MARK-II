using UnityEngine;
using System.Collections;

public class M_ItemKey : MonoBehaviour {

    private float   m_CurrentAngle;
    private bool    m_KeyGet;
    private float   m_KeyMatAlpha       = 1f;
    private float   m_KeyFadeOutSpeed   = 2f;
    private Vector3 m_KeyMoveOutScale   = new Vector3(3, 3, 3);
    private float   m_KeyScaleSpeed     = 2.5f;
    private float   m_KeyMoveOutSpeed   = 5f;

    void Start()
    {
        m_KeyGet        = false;
        m_KeyMatAlpha   = 1f;
    }

    void Update()
    {
        if (M_GameMain.GAME_PAUSED)
        {
            return;
        }
        RotateItem();
        if (m_KeyGet)
        {
            FadeOutKey();
            MoveOutKey();
        }
    }

    void RotateItem()
    {
        transform.eulerAngles = new Vector3(0,
                                            m_CurrentAngle,
                                            0);
        m_CurrentAngle += 128f * Time.deltaTime;
        if (m_CurrentAngle >= 360)
            m_CurrentAngle = 0;
    }

    void MoveOutKey()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, m_KeyMoveOutScale, m_KeyScaleSpeed * Time.deltaTime);
        transform.Translate(Vector3.back * m_KeyMoveOutSpeed * Time.deltaTime, Space.World);
    }

    void FadeOutKey()
    {
        var keyColor = this.GetComponentInChildren<Renderer>();
        m_KeyMatAlpha -= m_KeyFadeOutSpeed * Time.deltaTime;
        if (m_KeyMatAlpha <= 0f)
        {
            Destroy(this.gameObject);
            return;
        }
        keyColor.material.color = new Color(keyColor.material.color.r,
                                            keyColor.material.color.g,
                                            keyColor.material.color.b,
                                            m_KeyMatAlpha);
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            m_KeyGet = true;
            M_GameMain.INSTANCE.KeyGet++;
            M_GameMain.INSTANCE.GUIKeys[M_GameMain.INSTANCE.KeyGet - 1].GetComponent<M_GUIKeyMotor>().CanDropKey = true;
            Destroy(this.GetComponent<BoxCollider>());
        }
    }

}
