using UnityEngine;

public class PlayerNameSelection : MonoBehaviour
{
    [SerializeField] ScriptableObject gameInfo;

    int playerLeft;

    private void Awake()
    {
        PlayerCountSelection.OnPlayerNumberChosen += BeginAskForName;
    }

    void BeginAskForName(int playerCount)
    {
        playerLeft = playerCount;
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
}
