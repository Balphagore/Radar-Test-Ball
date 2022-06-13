using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(0f, 100f)]
    [SerializeField]
    private float speed;
    private float bonus = 1f;

    private float position;
    private bool isDragging;
    private bool isDamaged;
    private Vector3 movementVector;

    [Header("References")]
    [SerializeField]
    private Rigidbody ballRigidbody;
    [SerializeField]
    private Renderer ballRenderer;

    public delegate void OnTakeDamageHandle(int damage);
    public static event OnTakeDamageHandle TakeDamageEvent;
    public delegate void AddPointHandle(int points);
    public static event AddPointHandle AddPointEvent;
    public delegate void EndTriggerHandle();
    public static event EndTriggerHandle EndTriggerEvent;

    //Подпись и отпись от ивентов.
    private void OnEnable()
    {
        PlayerInput.MouseMoveEvent += OnMouseMoveEvent;
        PlayerInput.MouseClickEvent += OnMouseClickEvent;
        GameController.BoostBallSpeedEvent += OnBoostBallSpeedEvent;
    }

    private void OnDisable()
    {
        PlayerInput.MouseMoveEvent -= OnMouseMoveEvent;
        PlayerInput.MouseClickEvent -= OnMouseClickEvent;
        GameController.BoostBallSpeedEvent -= OnBoostBallSpeedEvent;
    }

    //Передвижение мяча полностью физическое, но жестко фиксировано.
    //Он всегда движется вперед со скоростью помноженной на бонус и добавляет в стороны скорость, зависящую от смещения зажатой мыши.
    private void FixedUpdate()
    {
        movementVector = Vector3.forward * speed * bonus + new Vector3(position, 0, 0);
        ballRigidbody.velocity = movementVector;
    }

    //При коллизиях с бортиками дорожки обнуляем боковую скорость.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Left" && position < 0)
        {
            position = 0;
        }
        else
        {
            if (collision.collider.gameObject.tag == "Right" && position > 0)
            {
                position = 0;
            }
        }
    }

    //Триггеры бонуса, штрафа и области окончания дорожки вызывают соответствующие ивенты.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndTrigger")
        {
            EndTriggerEvent?.Invoke();
        }
        else
        {
            if (other.gameObject.tag == "Penalty")
            {
                //При коллизии с штрафом мяч на 2 секунды становится неуязвимым.
                TakeDamageEvent?.Invoke(1);
                ballRenderer.material.color = Color.red;
                StartCoroutine("AfterDamageCoroutine", 2);
                isDamaged = true;
            }
            else
            {
                if (other.gameObject.tag == "Bonus")
                {
                    AddPointEvent?.Invoke(1);
                }
            }
            Destroy(other.gameObject);
        }
    }

    //При коллизии с бортиками или препятствиями мяч на 2 секунды становится неуязвимым.
    private void OnCollisionStay(Collision collision)
    {
        if ((collision.collider.gameObject.tag == "Obstacle" || collision.collider.gameObject.tag == "Left" || collision.collider.gameObject.tag == "Right") && !isDamaged)
        {
            TakeDamageEvent?.Invoke(1);
            ballRenderer.material.color = Color.red;
            StartCoroutine("AfterDamageCoroutine", 2);
            isDamaged = true;
        }
    }

    //Корутина, возвращающая мяч в обычное состояние через время.
    private IEnumerator AfterDamageCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ballRenderer.material.color = Color.white;
        isDamaged = false;
    }

    //Смещение курсора с зажатой левой кнопкой мыши превращается в боковой вектор движения мяча.
    private void OnMouseMoveEvent(float deltaPosition)
    {
        if (isDragging)
        {
            position += deltaPosition * Time.deltaTime;
        }
    }

    //Проверка на то зажата ли левая кнопка мыши.
    private void OnMouseClickEvent(bool isEnded)
    {
        if (!isDragging)
        {
            if (!isEnded)
            {
                isDragging = true;
            }
        }
        else
        {
            if (isEnded)
            {
                isDragging = false;
            }
        }
    }

    //В ответ на ивент ускорения мяча вызывается корутина, увеличивающая его скорость постепенно.
    private void OnBoostBallSpeedEvent(float value)
    {
        StartCoroutine("BoostCoroutine", value);
    }

    private IEnumerator BoostCoroutine(float value)
    {
        float time = 0f;
        float boostTime = 2f;
        float boost = value - bonus;
        while (time <= boostTime)
        {
            time += Time.deltaTime;
            bonus += boost * Time.deltaTime / boostTime;
            yield return null;
        }
        bonus = value;
    }
}