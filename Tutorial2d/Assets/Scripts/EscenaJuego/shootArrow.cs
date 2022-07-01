using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class shootArrow : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public float LaunchForce;
    public GameObject Arrow;
    public Text myScoreText;

    // solo se usara photon en la escena del juego
    string sceneName;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
        actions.Add("fuego", shoot);
        actions.Add("dispara", shoot);
        actions.Add("flecha", shoot);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("Keyword: "+ speech.text);
        // solo el atacante puede disparar las flechas
        // recordemos que las propiedades del jugador son
        // 0: para el atacante, 1: para el defensor
        // {
        //     personaje: [0,1]
        // }
        if (sceneName == "Nivel1")
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
            {
                actions[speech.text].Invoke();
            }            
        }
        else
        {
            actions[speech.text].Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        // solo el master puede disparar una flecha
        if (Input.GetKeyUp("space"))
        {
            if (sceneName == "Nivel1")
            {
                if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
                {
                    shoot();
                }                
            }
            else
            {
                shoot();
            }
        }
    }


    void shoot()
    {
        // instanciando con photon
        
        if (sceneName == "Nivel1")
        {
            GameObject ArrowIns = PhotonNetwork.Instantiate(Arrow.name, transform.position, transform.rotation);
            ArrowIns.GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce);

            ScoreManager scoreManager = ArrowIns.GetComponent<ScoreManager>();
            scoreManager.MyscoreText = myScoreText;
            // aumentamos los disparos del atacante
            if (scoreManager.GetComponent<PhotonView>().IsMine)
            {
                scoreManager.GetComponent<PhotonView>().RPC("AddDisparoAtacante", RpcTarget.AllBuffered);
            }            
        }
        else
        {
            GameObject ArrowIns = Instantiate(Arrow, transform.position, transform.rotation);
            ArrowIns.GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce);
        }
    }
}
