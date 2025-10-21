using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissorView : Singleton<RockPaperScissorView>
{
    private PhotonView photonView;
    [SerializeField] TextMeshProUGUI determineWinner;

    private Dictionary<int, ChoiceSelected> playerChoices = new Dictionary<int, ChoiceSelected>();

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // ?? Llamado cuando el jugador hace una elección
    public void SubmitSelection(ChoiceSelected selection)
    {
        // Enviar la elección a todos los jugadores
        photonView.RPC(nameof(ReceiveSelectionRPC), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, (int)selection);
    }

    [PunRPC]
    public void ReceiveSelectionRPC(int playerId, int selection)
    {
        ChoiceSelected choice = (ChoiceSelected)selection;
        playerChoices[playerId] = choice;
        Debug.Log($"Player {playerId} chose {choice}");

        // Si ambos jugadores ya eligieron, determinar resultado
        if (playerChoices.Count >= 2)
        {
            DetermineResult();
        }
    }

    void DetermineResult()
    {
        if (playerChoices.Count < 2)
            return;

        var enumerator = playerChoices.GetEnumerator();
        enumerator.MoveNext();
        var player1 = enumerator.Current;
        enumerator.MoveNext();
        var player2 = enumerator.Current;

        string result = CompareChoices(player1.Value, player2.Value, player1.Key, player2.Key);
        determineWinner.gameObject.SetActive(true);
        determineWinner.text = result;

         // Reset para la siguiente ronda
         playerChoices.Clear();
    }

    string CompareChoices(ChoiceSelected a, ChoiceSelected b, int idA, int idB)
    {
        string nameA = PhotonNetwork.CurrentRoom.GetPlayer(idA).NickName;
        string nameB = PhotonNetwork.CurrentRoom.GetPlayer(idB).NickName;

        if (a == b)
            return $"It's a tie! ({nameA} and {nameB} both chose {a})";

        if ((a == ChoiceSelected.Rock && b == ChoiceSelected.Scissors) ||
            (a == ChoiceSelected.Paper && b == ChoiceSelected.Rock) ||
            (a == ChoiceSelected.Scissors && b == ChoiceSelected.Paper))
        {
            return $"{nameA} wins! ({a} beats {b})";
        }
        else
        {
            return $"{nameB} wins! ({b} beats {a})";
        }
    }
}