using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text YouWin;
    public Text YouLose;

    public Image Continue;
    public Image Restart;
    //public Text EnemyBoss;

    public float delayPanel = 7.0f;

    public int CheckPointCounter;

    bool checkPointReached = false;

    public CountDownForStart countDownForStart;

    void Start()
    {
        YouLose.SetActive(false);
        //EnemyBoss.SetActive(false);
        YouWin.SetActive(false);
        Continue.SetActive(false);
        Restart.SetActive(false);
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

                StartCoroutine(LoadPanel(delayPanel));

                Continue.SetActive(true);
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
                Continue.SetActive(true);
                StartCoroutine(LoadPanel(delayPanel));
                
                Debug.Log("Finish unlocked!");
            }
            else
            {
                YouLose.SetActive(true);
                StartCoroutine(LoadPanel(delayPanel));
                Restart.SetActive(true);
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

    IEnumerator LoadPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
