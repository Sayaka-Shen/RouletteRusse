using UnityEngine;
using TMPro;



public class ChoosePlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nbrPlayer;
    private int _nbrPlayerIndex = 2;

    private void Start()
    {
        UpdateUI();
    }

    public void AddNbrPlayer()
    {
        _nbrPlayerIndex += 1;
        UpdateUI();
    }

    public void RemoveNbrPlayer()
    {
        _nbrPlayerIndex -= 1;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _nbrPlayer.text = _nbrPlayerIndex.ToString();
    }

    public void NameUI()
    {

    }

}
