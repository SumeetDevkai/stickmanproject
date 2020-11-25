using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Finisher : MonoBehaviour
{
    public Text t;

    // Start is called before the first frame update
    void Start()
    {
        t.text = "WINNER : " + PlayerPrefs.GetString("NickName");
        Invoke("MM", 7f);
    }

    private void MM()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
