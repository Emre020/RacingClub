using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public static string theWinner = "";

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
