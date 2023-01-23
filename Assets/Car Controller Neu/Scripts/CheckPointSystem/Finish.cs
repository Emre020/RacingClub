using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text YouWin;
    public Text YouLose;
    //public Text EnemyBoss;

    public int CheckPointCounter;

    bool checkPointReached = false;

    void Start()
    {
        YouLose.SetActive(false);
        //EnemyBoss.SetActive(false);
        YouWin.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = false;
            if (checkPointReached == true)
            {
                YouWin.SetActive(true);
                GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = false;
            } 
            else if (other.CompareTag ("Enemy"))
            {
                GameObject.Find("YellowCar 1").GetComponent<Finish>().CheckPointCounter = -100;
            }
        }

        CheckPointCounter++;

        if (CheckPointCounter == 5)
        {
            checkPointReached = true;
            Debug.Log("Finish unlocked!");
        }
        else
        {
            YouLose.SetActive(true);
            //EnemyBoss.SetActive(true);
        }
    }
}
