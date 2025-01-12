using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Init,
        StartRound,
        EndRound
    }

    [Header("General Settings")] 
    [SerializeField] private int _playerNumber;
    private int _playerCount;
    private int _deathChance;
    private int _randomNumber;

    private void Start()
    {
        _playerCount = _playerNumber;
        _deathChance = 6;
        
        PreRound();
    }

    private void PreRound()
    {
        Debug.Log("Le round commence !");
        MusicManager.Instance.PlayMusic("SlowHeartBeat");
        SoundManager.Instance.PlaySound2D("ReloadSound");
    }

    public void Shoot()
    {
        _randomNumber = Random.Range(1, _deathChance);

        // Mort le bro
        if (_randomNumber == 1)
        {
            SoundManager.Instance.PlaySound2D("ShotSound");
            SoundManager.Instance.PlaySound2D("FallBodySound");
            SoundManager.Instance.PlaySound2D("ReloadSound");

            _playerCount--;
            _deathChance = 6;
            
            PostRound();
        }
        else // Vivant le type
        {
            _deathChance--;
            SoundManager.Instance.PlaySound2D("EmptyShotSound");
            Debug.Log("Next Player");
            
            if (_deathChance <= 4)
            {
                MusicManager.Instance.PlayMusic("FastHeartBeat");
            }
        }
    }

    private void PostRound() // Ecran de relance pour le battle royale
    {
        Debug.Log($"Le player {_playerNumber - _playerCount} est mort! ");

        if(_playerCount == 1)
        {
            Debug.Log("Fin du round tout le monde est mort sauf toi, rappuyer sur le bouton pour rejouer");
        }
        else
        {
            PreRound();    
        }
        
    }
}
