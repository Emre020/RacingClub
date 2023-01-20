using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextRacer : MonoBehaviour
{
    public Text FindARacer;

    void Start()
    {
        FindARacer.SetActive(true);
    }

    void Update()
    {
         Destroy(gameObject, 5f);
    }
}
