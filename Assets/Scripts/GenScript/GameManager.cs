using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


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
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private Text _playerInfo;
    private int _playerCount;
    private int _deathChance;
    private int _randomNumber;
    private int _roundCount = 0;
    private int _lastPlayer;
    private List<int> list = new List<int>();
    private List<int> _players = new List<int>();

    [Header("Animation")]
    [SerializeField] private Animator _gunAnimator;
    //[SerializeField] private Animator _gunAnimator2;
    //[SerializeField] private SpriteScroller _reloadAnim;

    private void Start()
    {
        _playerNumber = _gameInfo.Players.Count;
        _playerCount = _playerNumber;
        _deathChance = 6;

        PreRound();
    }

    private void PreRound()
    {
        for (int i = 1; i < _playerCount; i++)
        {
            list.Add(i + 1);
            _players.Add(i + 1);
        }

        _roundCount++;
        Debug.Log("Le round commence !");
        MusicManager.Instance.PlayMusic("SlowHeartBeat");
        SoundManager.Instance.PlaySound2D("ReloadSound");
        DifferentUI();
    }

    public void Shoot()
    {
        _randomNumber = Random.Range(1, _deathChance);
        _gunAnimator.SetTrigger("Shoot");

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
            DifferentUI();
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
        list.Clear();
        if (_playerCount == 1)
        {
            _roundCount = 0;
            Debug.Log("Fin du round tout le monde est mort sauf toi, rappuyer sur le bouton pour rejouer");
        }
        else
        {
            PreRound();
        }

    }

    private void DifferentUI()
    {
        int index = Random.Range(1, _players.Count);

        if (_players.Count == 0)
        {
            _players = list;
        }

        for(int i =  0; i < _players.Count; i++)
        {
            if (index == i)
            {
                _playerInfo.text = _gameInfo.Players[index];
                _players.Remove(index);
            }
        }
    }
}

