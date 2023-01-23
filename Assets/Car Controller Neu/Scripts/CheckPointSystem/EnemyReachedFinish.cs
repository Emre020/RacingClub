using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyReachedFinish : MonoBehaviour
{
    public Text EnemyBoss;
     void Start()
    {
        EnemyBoss.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBoss.SetActive(true);
        }
    }  
}
