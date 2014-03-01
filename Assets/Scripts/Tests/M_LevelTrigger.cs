using UnityEngine;
using System.Collections;

public class M_LevelTrigger : MonoBehaviour
{
    #region Fields

    #endregion


    #region Properties

    #endregion


    #region Function

    void Update()
    {
        RotateLevel();
    }

    void RotateLevel()
    {
        transform.Rotate(0,0,-10 * Time.deltaTime);
    }

    #endregion
}
