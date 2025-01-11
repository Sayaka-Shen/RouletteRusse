using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNameSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameInfo gameInfo;
    [SerializeField] GameObject inputFieldWrapper;
    [SerializeField] GameObject inputFieldPrefab;
    List<InputField> inputFieldList = new();
    [SerializeField] CanvasGroup canvasGroup;

    [Header("Params")]
    [SerializeField] float fadeDuration;
    [SerializeField] string gameSceneString;

    AsyncOperation nextSceneLoading;

    private void Awake()
    {
        PlayerCountSelection.OnPlayerNumberChosen += BeginAskForName;
        nextSceneLoading = SceneManager.LoadSceneAsync(gameSceneString, LoadSceneMode.Single);
        nextSceneLoading.allowSceneActivation = false;

        VolumeButton.OnVolumeDown += () => { Debug.Log("Down"); };
        VolumeButton.OnVolumeUp += () => { Debug.Log("Up"); };
    }

    void BeginAskForName(int playerCount)
    {
        Debug.Log(playerCount);

        foreach (Transform child in inputFieldWrapper.transform) // Clear prev children
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= playerCount; i++)
        {
            InputField inputField = Instantiate(inputFieldPrefab, inputFieldWrapper.transform).GetComponent<InputField>();
            if (inputField.placeholder is Text)
            {
                Text text = (Text)inputField.placeholder;
                text.text = $"Player {i}";
            }
            inputFieldList.Add(inputField);
        }
    }

    public void SaveNames()
    {
        foreach (InputField inputField in inputFieldList)
        {
            gameInfo.Players.Add(inputField.text);
        }

        canvasGroup.DOFade(0, fadeDuration).onKill += () => 
        {
            nextSceneLoading.allowSceneActivation = true;
        };
    }
}
