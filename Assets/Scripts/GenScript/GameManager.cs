using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
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
    
    [Header("Music Speed")]
    [SerializeField] private float slowSpeedHeartBeat = 0.8f;
    [SerializeField] private float fastSpeedHeartBeat = 1.2f;
    
    [Header("Android Flashlight")]
    private bool _active;
    private AndroidJavaObject _camera1;
    
    
    // Event 
    public static event Action<int> OnDeathChanceChange;

    private void Start()
    {
        CameraFlashlight();
        
        VolumeButton.OnVolumeUp += Shoot;
        VolumeButton.OnVolumeDown += Shoot;
        
        _playerNumber = _gameInfo.Players.Count;
        _playerCount = _playerNumber;
        _deathChance = 6;

        _retryGame.SetActive(false);
        _players = new List<string>(_gameInfo.Players);

        Debug.Log("AAAAAA");
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
        
        VolumeButton.Instance.m_bGetVolumeFromPhone = true;
    }

    public void Shoot()
    {
        _randomNumber = Random.Range(1, _deathChance + 1);

        // Mort le bro
        if (_randomNumber == 1)
        {
            Handheld.Vibrate();
            SoundManager.Instance.PlaySound2D("ShotSound");
            SoundManager.Instance.PlaySound2D("FallBodySound");
            SoundManager.Instance.PlaySound2D("ReloadSound");
            _gunAnimator.SetTrigger("Shoot");

            StartCoroutine(FlashLightToggle());

            _playerCount--;
            _deathChance = 6;

            _flashAnimator.SetTrigger("Flash");
            _players.Remove(_player);
            
            OnDeathChanceChange?.Invoke(_deathChance);
            
            VolumeButton.Instance.m_bGetVolumeFromPhone = false;

            PostRound();
        }
        else // Vivant le type
        {
            _deathChance--;
            OnDeathChanceChange?.Invoke(_deathChance);
            SoundManager.Instance.PlaySound2D("EmptyShotSound");
            Debug.Log("Next Player");

            DifferentUI(); 
            
            if (_deathChance > 3)
            {
                if (_deathChance == 5)
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
    
    private void CameraFlashlight()
    {
        AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");

        int camID = 0;
        _camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", camID);

        if (_camera1 != null)
        {
            AndroidJavaObject cameraParameters = _camera1.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode", "torch");
            _camera1.Call("setParameters", cameraParameters);
        }
        else
        {
            Debug.LogError("[CameraParametersAndroid] Camera not available");
        }

    }

    IEnumerator FlashLightToggle()
    {
        if (_camera1 != null)
        {
            _camera1.Call("startPreview");
            _active = true;
        }
        
        yield return new WaitForSeconds(0.2f);
        
        if (_camera1 != null)
        {
            _camera1.Call("stopPreview");
            _active = false;
        }
        
    }
}

