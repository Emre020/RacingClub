using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchLevel : MonoBehaviour
{
    public Finish script;

    public Text SwitchScene;
    public Image FadeInToLevel;
    public Image ShowEnemyText;

    public float delay = 15f;
    public float delayEnemyText = 15f;
    public string OpenLevel = "Level1";

    private GameObject[] cars;

    void Start()
    {
        SwitchScene.SetActive(false);
        FadeInToLevel.SetActive(false);
        ShowEnemyText.SetActive(false);

        cars = GameObject.FindGameObjectsWithTag("Car");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag ("Car"))
        {
            SwitchScene.SetActive(true);

            if (Input.GetKeyDown(KeyCode.R))
            {
                cars = GameObject.FindGameObjectsWithTag("Car");

                foreach (GameObject car in cars)
                {
                    car.GetComponent<CarControllerAdvanced>()/*.enabled = false*/;
                }

                //GameObject.Find("StartCar").GetComponent<CarControllerAdvanced>().enabled = false;
                ShowEnemyText.SetActive(true);
                StartCoroutine(LoadEnemytextAfterDelay(delay));
                FadeInToLevel.SetActive(true);
                StartCoroutine(LoadLevelAfterDelay(delay));
                //SceneManager.LoadScene("Level1");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            SwitchScene.SetActive(false);
        }
    }

    IEnumerator LoadLevelAfterDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level1");
    }

    IEnumerator LoadEnemytextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowEnemyText.SetActive(true);
    }
}
