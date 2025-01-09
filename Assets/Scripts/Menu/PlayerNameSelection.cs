using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameInfo gameInfo;
    [SerializeField] InputField inputField;

    int playerLeft;
    int playerCount;

    private void Awake()
    {
        PlayerCountSelection.OnPlayerNumberChosen += BeginAskForName;
    }

    void BeginAskForName(int playerCount)
    {
        this.playerCount = playerCount;
        playerLeft = playerCount;
        gameInfo.Players.Capacity = playerCount;
        inputField.onValidateInput += SaveName;
        AskForName();
    }


    void AskForName()
    {
        if (playerLeft > 0)
        {
            playerLeft--;
            // Show ask name screen + reset
        }
        else
        {
            // Start game
        }
        
    }

    char SaveName(string text, int charIndex, char addedChar)
    {
        gameInfo.Players.Add((playerCount - playerLeft).ToString());
        return '1';
    }

}
