using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownForStart : MonoBehaviour
{
    public GameObject countDownToStart;

    private void Start()
    {
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

        GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = true;
    }
}
