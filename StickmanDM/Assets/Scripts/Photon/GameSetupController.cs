using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameSetupController : MonoBehaviour
{
    public List<GameObject> playerPrefabs;
    public Text ping;
    List<GameObject> players;

    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        ping.text = "Ping : " + PhotonNetwork.GetPing() + " ms";
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), new Vector3(0,1,0), Quaternion.identity); // "PhotonPlayer = player gameobject name"
  
    }

}
