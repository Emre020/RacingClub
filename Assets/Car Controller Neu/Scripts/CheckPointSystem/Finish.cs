using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text YouWin;

    public int CheckPointCounter;

    bool checkPointReached = false;

    void Start()
    {
        YouWin.SetActive(false);    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            if (checkPointReached == true)
            {
                YouWin.SetActive(true);
                GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = false;
            }
        }

        CheckPointCounter++;

        if (CheckPointCounter == 6)
        {
            checkPointReached = true;
            Debug.Log("Finish unlocked!");
        }
    }
}
