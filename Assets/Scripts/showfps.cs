using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showfps : MonoBehaviour
{
    private Text fpsText;

    private void Start()
    {
         fpsText = GetComponent<Text>();

    }

    private void Update()
    {
        fpsText.text = 1.0f / Time.deltaTime +"";
    }
}
