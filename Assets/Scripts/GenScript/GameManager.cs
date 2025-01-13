using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int _playerNumber;
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private TextMeshProUGUI _playerInfo;
    private int _playerCount;
    private int _deathChance;
    private int _randomNumber;
    private int _roundCount = 0;

    [Header("Animation")]
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private SpriteScroller _reloadAnim;
    
    [Header("Music Speed")]
    [SerializeField] private float slowSpeedHeartBeat = 0.8f;
    [SerializeField] private float fastSpeedHeartBeat = 1.2f;

    private void Start()
    {
        VolumeButton.OnVolumeUp += Shoot;
        VolumeButton.OnVolumeDown += Shoot;
        
        _playerNumber = _gameInfo.Players.Count;
        _playerCount = _playerNumber;
        _deathChance = 6;

        PreRound();
    }

    private void PreRound()
    {
        _roundCount++;
        Debug.Log("Le round commence !");
        SoundManager.Instance.PlaySound2D("ReloadSound");
        _reloadAnim.StartScroll();
        DifferentUI();

        VolumeButton.Instance.m_bGetVolumeFromPhone = true;
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
            _reloadAnim.StartScroll();

            _playerCount--;
            _deathChance = 6;
            
            VolumeButton.Instance.m_bGetVolumeFromPhone = false;

            PostRound();
        }
        else // Vivant le type
        {
            _deathChance--;
            SoundManager.Instance.PlaySound2D("EmptyShotSound");
            Debug.Log("Next Player");

            if (_deathChance > 3)
            {
                MusicManager.Instance.PlayMusic("SlowHeartBeat");

                if (_deathChance == 6)
                {
                    MusicManager.Instance.MusicSource.pitch = slowSpeedHeartBeat;
                } 
                else if (_deathChance == 5)
                {
                    MusicManager.Instance.MusicSource.pitch = 1f;
                }
                else if (_deathChance == 4)
                {
                    MusicManager.Instance.MusicSource.pitch = fastSpeedHeartBeat;
                }
            }
            else if (_deathChance <= 3)
            {
                MusicManager.Instance.PlayMusic("FastHeartBeat");
                
                if (_deathChance == 3)
                {
                    MusicManager.Instance.MusicSource.pitch = slowSpeedHeartBeat;
                } 
                else if (_deathChance == 2)
                {
                    MusicManager.Instance.MusicSource.pitch = 1f;
                }
                else if (_deathChance == 1)
                {
                    MusicManager.Instance.MusicSource.pitch = fastSpeedHeartBeat;
                }
            }
        }
    }

    private void PostRound() // Ecran de relance pour le battle royale
    {
        Debug.Log($"Le player {_playerNumber - _playerCount} est mort! ");

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
        int index;
        if (_roundCount % 2 == 1) // Si _roundCount est impair
        {
            index = _deathChance;
        }
        else
        {
            index = 7 - _deathChance;
        }

        if (index >= 1 && index <= _gameInfo.name.Length)
        {
            _playerInfo.text = _gameInfo.name[index].ToString();
        }
    }
}

