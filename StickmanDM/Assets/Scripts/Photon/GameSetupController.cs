using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameSetupController : MonoBehaviour
{
    GameObject[] objs;
    int[] scorez;
    public Text[] scores, names;
    public int GameTimeLimit;

    public List<GameObject> playerPrefabs;
    public Text ping, timerr;
    List<GameObject> players;

    void Start()
    {
        scorez = new int[scores.Length];

        PlayStartSound();
        Invoke("CreatePlayer", 3f);
        //CreatePlayer();
        
        GameEnd();
    }

    private void GameEnd()
    {
        timerr.text = GameTimeLimit + "";
        if(GameTimeLimit == 0)
        {
            //FinishGame

            string max = FindMaxInArr();

            PlayerPrefs.SetString("Winner", max);

            PhotonNetwork.LoadLevel(3);
        }
        else
        {
            GameTimeLimit--;
            Invoke("GameEnd", 1f);
        }
    }

    private string FindMaxInArr()
    {
        int max = int.MinValue,
            index = -1, maxInd = 0;
        foreach (int item in scorez)
        {
            index++;
            if (item > max)
            { 
                max = item;
                maxInd = index;
            }
        }
        return objs[maxInd].GetComponent<PlayerMovement>().GetNickName();
    }

    // Update is called once per frame
    void Update()
    {
        ping.text = "Ping : " + PhotonNetwork.GetPing() + " ms";
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), new Vector3(0,1,0), Quaternion.identity); // "PhotonPlayer = player gameobject name"

        PlayBGSound();

        Invoke("GiveScores", 4);
    }

    private void GiveScores()
    {
        objs = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < objs.Length; i++)
        {
            scores[i].text = objs[i].GetComponent<PlayerMovement>().GetPlayerScore().ToString();
            scorez[i] = objs[i].GetComponent<PlayerMovement>().GetPlayerScore();
            names[i].text = objs[i].GetComponent<PlayerMovement>().GetNickName().ToString();
        }

        Invoke("GiveScores", 0.1f);
    }

    public AudioSource BGSound;
    private void PlayBGSound()
    {
        BGSound.Play();
    }

    public AudioSource[] sources;
    private void PlayStartSound()
    {
        sources[Random.Range(0, sources.Length)].Play();
    }
}
