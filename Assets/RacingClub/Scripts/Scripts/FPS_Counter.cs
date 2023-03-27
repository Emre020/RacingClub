using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
    public GameObject ShowFps;
    public Text fpsText;
    public float deltaTime;


    /*private void Start()
    {
        ShowFps.SetActive(false);

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowFps.SetActive(true);
        }
    }
    */

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
