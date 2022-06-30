using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BirdsSpawner : MonoBehaviour
{
    public GameObject birdPrefab;

    private float[] rangeY = {-1.09f, 2.62f};
    
    // Start is called before the first frame update
    void Start()
    {
        int whichCharacter = (int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"];
        // solo el atacante podrá instanciar los pajaritos
        if(whichCharacter == 0){
            Vector3 posBird1 = new Vector3(4.29f, Random.Range(rangeY[0],rangeY[1]), 0.0f);
            Vector3 posBird2 = new Vector3(8.31f, Random.Range(rangeY[0],rangeY[1]), 0.0f);

            // instanciamos dos pajaritos en el juego
            // pajarito 1
            PhotonNetwork.Instantiate(birdPrefab.name, posBird1, Quaternion.identity);
            // pajarito 2
            PhotonNetwork.Instantiate(birdPrefab.name, posBird2, Quaternion.identity);
        }
        

    }
    
}
