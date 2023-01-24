using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishForEnemy : MonoBehaviour
{
    public Text YouLose;
    public Text EnemyBoss;

    // Start is called before the first frame update
    void Start()
    {
        YouLose.SetActive(false);
        EnemyBoss.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject.Find("YellowCar 1").GetComponent<Finish>().CheckPointCounter = -100;
            YouLose.SetActive(true);
            EnemyBoss.SetActive(true);
        }    
    }
}
