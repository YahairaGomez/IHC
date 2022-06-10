using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambioEscena : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (sceneName == "exit")
        {
            Debug.Log("saliendo del juego");
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
