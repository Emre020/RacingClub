using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWinner : MonoBehaviour
{
    //public Text YouWin;
    public string car;
    public Text YouLose;

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            Winner.theWinner = "You Lose!";
            //YouWin.SetActive(true);
            GameObject.Find("YellowCar 1").GetComponent<CarControllerAdvanced>().enabled = false;
            
        }
    }*/

    /*public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            YouLose.SetActive(true);
        }
    }*/
}
