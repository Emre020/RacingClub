using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchLevel : MonoBehaviour
{
    public Text SwitchScene;

    void Start()
    {
        SwitchScene.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag ("Car"))
        {
            SwitchScene.SetActive(true);

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Level1");
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
}
