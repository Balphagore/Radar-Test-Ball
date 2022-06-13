using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives;
    [SerializeField]
    private int points;

    public delegate void UpdateLivesHandle(int lives);
    public static event UpdateLivesHandle UpdateLivesEvent;
    public delegate void UpdatePointsHandle(int points);
    public static event UpdatePointsHandle UpdatePointsEvent;
    public delegate void BoostBallSpeedHandle(float value);
    public static event BoostBallSpeedHandle BoostBallSpeedEvent;
    public delegate void UpdateResultHandle(string result);
    public static event UpdateResultHandle UpdateResultEvent;

    //ѕодпись и отпись от ивентов.
    private void OnEnable()
    {
        Ball.TakeDamageEvent += OnTakeDamageEvent;
        Ball.AddPointEvent += OnAddPointEvent;
        Ball.EndTriggerEvent += OnEndTriggerEvent;
    }

    private void OnDisable()
    {
        Ball.TakeDamageEvent -= OnTakeDamageEvent;
        Ball.AddPointEvent -= OnAddPointEvent;
        Ball.EndTriggerEvent -= OnEndTriggerEvent;
    }

    //—брос таймскейла и обновление значений жизней и очков.
    private void Start()
    {
        Time.timeScale = 1f;
        UpdateLivesEvent?.Invoke(lives);
        UpdatePointsEvent?.Invoke(points);
    }

    //—нижение жизней в ответ на ивент м€ча. –естарт сцены с задержкой, если они упали до 0.
    private void OnTakeDamageEvent(int damage)
    {
        lives -= damage;
        UpdateLivesEvent?.Invoke(lives);
        if (lives <= 0)
        {
            UpdateResultEvent?.Invoke("Lose");
            StartCoroutine("RestartCoroutine", 5);
        }
    }

    //ƒобавление очков и ускорение м€ча, когда они достигают определенного уровн€.
    private void OnAddPointEvent(int points)
    {
        this.points += points;
        UpdatePointsEvent?.Invoke(this.points);
        switch (this.points)
        {
            case 10:
                BoostBallSpeedEvent?.Invoke(1.5f);
                break;
            case 25:
                BoostBallSpeedEvent?.Invoke(2f);
                break;
            case 50:
                BoostBallSpeedEvent?.Invoke(3f);
                break;
            case 100:
                BoostBallSpeedEvent?.Invoke(4f);
                break;
        }
    }

    //–естарт уровн€, когда м€ч вызывает ивент при контакте с триггером в конце дорожки.
    private void OnEndTriggerEvent()
    {
        UpdateResultEvent?.Invoke("Win");
        StartCoroutine("RestartCoroutine", 5);
    }

    private IEnumerator RestartCoroutine(float delay)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(0);
    }
}