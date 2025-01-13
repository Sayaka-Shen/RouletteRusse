using UnityEngine;
using TMPro;



public class ChoosePlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nbrPlayer;
    [SerializeField] private TextMeshProUGUI _roundPlayer;
    private int _nbrPlayerIndex = 2;
    private int _roundIndex = 1;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            NextRound();
        }
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

    public void NextRound()
    {
        _roundIndex++;
        _roundPlayer.text = _roundIndex.ToString();
    }

}
