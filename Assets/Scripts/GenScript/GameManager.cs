using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;


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
    [SerializeField] private GameObject _retryGame;
    private int _playerCount;
    private int _deathChance;
    private int _randomNumber;
    private int _roundCount = 0;
    private string _player;
    private List<int> list = new List<int>();
    private List<string> _players = new List<string>();

    [Header("Animation")]
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private Animator _flashAnimator;
    //[SerializeField] private Animator _gunAnimator2;
    //[SerializeField] private SpriteScroller _reloadAnim;

    private void Start()
    {
        _playerNumber = _gameInfo.Players.Count;
        _playerCount = _playerNumber;
        _deathChance = 6;

        _retryGame.SetActive(false);
        _players = _gameInfo.Players.ToArray().ToList();

        PreRound();
    }

    private void PreRound()
    {

        for (int i = 0; i < _playerCount; i++)
        {
            list.Add(i);
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

        // Mort le bro
        if (_randomNumber == 1)
        {
            SoundManager.Instance.PlaySound2D("ShotSound");
            SoundManager.Instance.PlaySound2D("FallBodySound");
            SoundManager.Instance.PlaySound2D("ReloadSound");
            _gunAnimator.SetTrigger("Shoot");

            _playerCount--;
            _deathChance = 6;

            _flashAnimator.SetTrigger("Flash");
            _players.Remove(_player);

            PostRound();
        }
        else // Vivant le type
        {
            _deathChance--;
            SoundManager.Instance.PlaySound2D("EmptyShotSound");
            Debug.Log("Next Player");

            DifferentUI();
            if (_deathChance <= 4)
            {
                MusicManager.Instance.PlayMusic("FastHeartBeat");
            }
        }
    }

    private void PostRound() // Ecran de relance pour le battle royale
    {
        Debug.Log($"Le player {_playerInfo.text.ToString()} est mort! ");
        list.Clear();
        DifferentUI();

        if (_playerCount == 1)
        {
            _roundCount = 0;
            Debug.Log($"Fin du round tout le monde est mort sauf {_players[0]}, rappuyer sur le bouton pour rejouer");
            _retryGame.SetActive(true);
        }
        else
        {
            PreRound();
        }
    }

    private void DifferentUI()
    {
        int index = Random.Range(0, _players.Count);

        for(int i =  0; i < _players.Count; i++)
        {
            if (index == i)
            {
                _playerInfo.text = _gameInfo.Players[index];
                _player = _playerInfo.text.ToString();
            }
        }
    }
}

