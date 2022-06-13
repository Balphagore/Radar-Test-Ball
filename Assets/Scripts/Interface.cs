using UnityEngine;
using TMPro;

public class Interface : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI pointsText;
    [SerializeField]
    private TextMeshProUGUI livesText;
    [SerializeField]
    private TextMeshProUGUI resultText;

    //������� � ������ �� �������.
    private void OnEnable()
    {
        GameController.UpdateLivesEvent += OnUpdateLivesEvent;
        GameController.UpdatePointsEvent += OnUpdatePointsEvent;
        GameController.UpdateResultEvent += OnUpdateResultEvent;
    }

    private void OnDisable()
    {
        GameController.UpdateLivesEvent -= OnUpdateLivesEvent;
        GameController.UpdatePointsEvent -= OnUpdatePointsEvent;
        GameController.UpdateResultEvent -= OnUpdateResultEvent;
    }

    //�������� �������, ������� ��������� ��� ������ ��� ���������.
    private void Start()
    {
        resultText.gameObject.SetActive(false);
    }

    //����� ������������ ���������� ������ ����� �� ��������� � GameController.
    private void OnUpdateLivesEvent(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    //���������� ��� �����.
    private void OnUpdatePointsEvent(int points)
    {
        pointsText.text = "Points: " + points;
    }

    //����� �� ����� ������� � ������ ��� ���������.
    private void OnUpdateResultEvent(string result)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "You " + result + "!";
    }
}