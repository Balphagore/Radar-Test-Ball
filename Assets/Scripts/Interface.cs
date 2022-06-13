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

    //Подпись и отпись от ивентов.
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

    //Сокрытие надписи, которая выводится при победе или поражении.
    private void Start()
    {
        resultText.gameObject.SetActive(false);
    }

    //Вывод обновленного количества жизней после их изменения в GameController.
    private void OnUpdateLivesEvent(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    //Аналогично для очков.
    private void OnUpdatePointsEvent(int points)
    {
        pointsText.text = "Points: " + points;
    }

    //Вывод на экран надписи о победе или поражении.
    private void OnUpdateResultEvent(string result)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "You " + result + "!";
    }
}