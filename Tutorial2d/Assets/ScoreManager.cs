using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    public Text MyscoreText;

   
    void AddPoint()
    {
        int scoreNum = PlayerPrefs.GetInt("player_score");
        scoreNum++;
        PlayerPrefs.SetInt("player_score", scoreNum);
        MyscoreText.text = "Puntaje: " + scoreNum;
    }
   private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Choque")
        {
            AddPoint();
            
        }
        
       
    }
}
