using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    //Cambia de escena cuando presiono el botón


    void Start()
    {
        PlayerPrefs.SetInt("player_score", 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }

    //En vez de poner qué escena quiero cargar, carga la escena en la que estoy y añade +1 para ir a la sgte,
    // Esta func hará que la scene cargue rápido, y no se verá la animation, entonces creamos corutina
    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < 4)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    //Corutina

    IEnumerator LoadLevel(int levelIndex)
    {
        //Play Animation: Referencia a nuestro animator -> OJO: Con el nombre con el que lo guardaoms en ventana animations
        //Trigger parameter de animator
        transition.SetTrigger("Start");


        //wait
        //Le digo que se detenga un momentito (mientras se reproduce la animation)
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        //Ojo, trabajamos con index
        SceneManager.LoadScene(levelIndex);

    }



}
