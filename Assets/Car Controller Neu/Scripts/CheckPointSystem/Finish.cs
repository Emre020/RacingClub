using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text YouWin;

    bool checkPointReached = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            if (checkPointReached == true)
            {

                //SceneManager.LoadScene("Finish");
            }
        }

        if (other.gameObject.name == "Checkpoint")
        {
            checkPointReached = true;
        }
    }
}
