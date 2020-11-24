using Photon.Pun;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameSetupController : MonoBehaviour
{

    List<GameObject> players;

    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayer()
    {
        GameObject x = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), new Vector3(0,1,0), Quaternion.identity); // "PhotonPlayer = player gameobject name"
  
    }

}
