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

    public CountDownForStart countDownForStart;

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
            GameObject.Find("YellowCar 1").GetComponent<CarSoundController>().enabled = false;

            if (checkPointReached == true)
            {
                YouWin.SetActive(true);

                GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = false;
                GameObject.Find("YellowCar 1").GetComponent<CarSoundController>().enabled = false;
            }

            countDownForStart.StopTimer();

            /* else if (other.CompareTag ("Enemy"))
             {
                 GameObject.Find("YellowCar 1").GetComponent<Finish>().CheckPointCounter = -100;
             }*/

            if (CheckPointCounter >= 5 && countDownForStart.currentTime <= countDownForStart.TimeForWin)
            {
                checkPointReached = true;
                YouWin.SetActive(true);
                Debug.Log("Finish unlocked!");
            }
            else
            {
                YouLose.SetActive(true);
                //EnemyBoss.SetActive(true);
            }
        }

        CheckPointCounter++;

        /*if (CheckPointCounter == 5 && countDownForStart.currentTime <= countDownForStart.TimeForWin)
        {
            checkPointReached = true;
            Debug.Log("Finish unlocked!");
        }
        else
        {
            YouLose.SetActive(true);
            //EnemyBoss.SetActive(true);
        }*/
    }
}
