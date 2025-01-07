using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General Settings")] 
    [SerializeField] private int _playerNumber;
    private int _randomNumber;
    private int _roundCount;

    private void Start()
    {
        _randomNumber = Random.Range(1, _playerNumber);
        _roundCount = 0;
    }

    public void PlayRound()
    {
        _roundCount++;

        if (_roundCount == _randomNumber)
        {
            Debug.Log("Dead, gameover, tu pues bouuh");
            
            _randomNumber = Random.Range(1, _playerNumber);
            _roundCount = 0;
        }
        else
        {
            Debug.Log("Next Player");
        }
    }
}
