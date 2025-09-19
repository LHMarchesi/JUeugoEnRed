using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private PhotonView photonView;

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
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