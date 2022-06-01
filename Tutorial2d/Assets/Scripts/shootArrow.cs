using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        actions[speech.text].Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            shoot();
        }
        
    }


    void shoot()
    {
        GameObject ArrowIns = Instantiate(Arrow, transform.position, transform.rotation);
        ArrowIns.GetComponent<ScoreManager>().MyscoreText = myScoreText;
        ArrowIns.GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce);
        
    }
}
