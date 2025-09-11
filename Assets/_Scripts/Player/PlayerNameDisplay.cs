using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviourPun
{
    [SerializeField] private TMP_Text nameText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Este es mi jugador ? uso mi NickName
            nameText.text = PhotonNetwork.NickName;
        }
        else
        {
            // Es un jugador remoto ? uso el NickName del dueño de este objeto
            nameText.text = photonView.Owner.NickName;
        }
    }
}