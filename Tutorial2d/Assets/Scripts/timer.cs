using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class timer : MonoBehaviour
{   
    [SerializeField] GameObject panel;
    [SerializeField] Image timeImage;
    [SerializeField] Text timeText;
    [SerializeField] float duration, currentTime;
    [SerializeField] GameObject textDisplay;
    [SerializeField] GameObject textRecordatorio;


    private void Start()
    {
    }

    private void Update()
    {
        // solo cuando el usuario presiona la tecla "e" se inicia el juego
        if (Input.GetKeyUp("e"))
        {
            panel.SetActive(false);
            textRecordatorio.SetActive(false);
            currentTime = duration;
            timeText.text = "00:0"+ currentTime.ToString();
            StartCoroutine(TimeIEn());            
        }
    }

    IEnumerator TimeIEn()
    {
        while(currentTime >= 0)
        {
            if (currentTime < 10) {
                //textDisplay.GetComponent<Text>().text = "00:0" + currentTime;
                timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
                
                
                timeText.text = "00:0" + currentTime.ToString();
                yield return new WaitForSeconds(1f);
                currentTime--;
            }
            else
            {
                //textDisplay.GetComponent<Text>().text = "00:0" + currentTime;
                timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
                timeText.text = "00:" + currentTime.ToString();
                yield return new WaitForSeconds(1f);
                currentTime--;

            }
        }
        OpenPanel();
    }

    void OpenPanel()
    {
        timeText.text = "";
        panel.SetActive(true);
    }
}
