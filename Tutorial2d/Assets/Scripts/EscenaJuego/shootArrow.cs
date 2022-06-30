using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class shootArrow : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public float LaunchForce;
    public GameObject Arrow;
    public Text myScoreText;


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
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
        {
            actions[speech.text].Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        // solo el master puede disparar una flecha
        if (Input.GetKeyUp("space") && (int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
        {
            shoot();
        }
        
    }


    void shoot()
    {
        // GameObject ArrowIns = Instantiate(Arrow, transform.position, transform.rotation);
        // instanciando con photon
        GameObject ArrowIns = PhotonNetwork.Instantiate(Arrow.name, transform.position, transform.rotation);
        ScoreManager scoreManager = ArrowIns.GetComponent<ScoreManager>();
        scoreManager.MyscoreText = myScoreText;
        ArrowIns.GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce);
        // aumentamos los disparos del atacante
        if (scoreManager.GetComponent<PhotonView>().IsMine)
        {
            scoreManager.GetComponent<PhotonView>().RPC("AddDisparoAtacante", RpcTarget.AllBuffered);
        }
    }
}
