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

    //������� � ������ �� �������.
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

    //������������ ���� ��������� ����������, �� ������ �����������.
    //�� ������ �������� ������ �� ��������� ����������� �� ����� � ��������� � ������� ��������, ��������� �� �������� ������� ����.
    private void FixedUpdate()
    {
        movementVector = Vector3.forward * speed * bonus + new Vector3(position, 0, 0);
        ballRigidbody.velocity = movementVector;
    }

    //��� ��������� � ��������� ������� �������� ������� ��������.
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

    //�������� ������, ������ � ������� ��������� ������� �������� ��������������� ������.
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
                //��� �������� � ������� ��� �� 2 ������� ���������� ����������.
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

    //��� �������� � ��������� ��� ������������� ��� �� 2 ������� ���������� ����������.
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

    //��������, ������������ ��� � ������� ��������� ����� �����.
    private IEnumerator AfterDamageCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ballRenderer.material.color = Color.white;
        isDamaged = false;
    }

    //�������� ������� � ������� ����� ������� ���� ������������ � ������� ������ �������� ����.
    private void OnMouseMoveEvent(float deltaPosition)
    {
        if (isDragging)
        {
            position += deltaPosition * Time.deltaTime;
        }
    }

    //�������� �� �� ������ �� ����� ������ ����.
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

    //� ����� �� ����� ��������� ���� ���������� ��������, ������������� ��� �������� ����������.
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