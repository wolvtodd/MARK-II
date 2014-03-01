using UnityEngine;
using System.Collections;

public class TriggerTest : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log("enter" + otherCollider.name);
    }

}
