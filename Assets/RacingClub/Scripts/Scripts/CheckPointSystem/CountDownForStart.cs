using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CountDownForStart : MonoBehaviour
{
    Finish script;
    private GameObject[] cars;

    public GameObject countDownToStart;

    bool timerActive = false;
    public float currentTime;
    public float TimeForWin;
    public Text currentTimeText;

    private void Start()
    {
        GameObject.Find("StartCar").GetComponent<CarControllerAdvanced>().enabled = false;
        StartCoroutine(CountDownRoutine());
    }

    IEnumerator CountDownRoutine()
    {

        yield return new WaitForSeconds(0.5f);
        countDownToStart.GetComponent<Text>().text = "3";
        countDownToStart.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        countDownToStart.SetActive(false);
        countDownToStart.GetComponent<Text>().text = "2";
        countDownToStart.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        countDownToStart.SetActive(false);
        countDownToStart.GetComponent<Text>().text = "1";
        countDownToStart.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        countDownToStart.SetActive(false);
        countDownToStart.GetComponent<Text>().text = "GO!";
        countDownToStart.SetActive(true);
        countDownToStart.SetActive(false);

        
        GameObject.Find("StartCar").GetComponent<CarControllerAdvanced>().enabled = true;
        StartTimer();
        GameObject.Find("StartCar").GetComponent<Finish>().CheckPointCounter = 0;
        GameObject.Find("StartCar").GetComponent<CountDownForStart>().currentTime = 0;
    }

    void Update()
    {
        if (timerActive == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:ff");
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
        //timerActive.color = Color.green;
    }
}

