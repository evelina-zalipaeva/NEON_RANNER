using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Ссылки")]
    public Animator animator;

    [Header("Движение")]
    public float forwardSpeed = 10f;
    public float laneDistance = 2.5f;
    public float laneSwitchSpeed = 15f;

    [Header("Прыжок")]
    public float jumpForce = 8f;
    public float gravity = -25f;

    private CharacterController controller;
    private int desiredLane = 1;
    private float verticalVelocity;
    private bool isGrounded;
    private bool jumpPressed = false;
    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.timeScale == 0f || isDead) return;

        HandleInput();
        MovePlayer();
        UpdateAnimations();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            if (desiredLane > 0) desiredLane--;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            if (desiredLane < 2) desiredLane++;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    void MovePlayer()
    {
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;

        float targetX = (desiredLane - 1) * laneDistance;
        float xMove = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        Vector3 lateralMove = new Vector3(xMove - transform.position.x, 0, 0);

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
            if (jumpPressed)
            {
                verticalVelocity = jumpForce;
                jumpPressed = false;
                if (animator != null) animator.SetTrigger("Jump");
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 verticalMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);

        controller.Move(forwardMove + lateralMove + verticalMove);
    }

    void UpdateAnimations()
    {
        if (animator != null) animator.SetFloat("Speed", 1f);
    }

    // Ловит физическое столкновение с препятствием
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Проверяем, есть ли на объекте тег "Obstacle" (убедитесь, что он назначен машинам!)
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1. Мгновенно останавливаем скорость
        forwardSpeed = 0f;
        laneSwitchSpeed = 0f;

        // 2. Отключаем Character Controller, чтобы игрок не "толкал" машину и не скользил
        if (controller != null) controller.enabled = false;

        // 3. Включаем анимацию удара
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("Death");
        }

        // 4. МГНОВЕННО вызываем экран проигрыша (без задержек и корутин)
        UIManager ui = FindAnyObjectByType<UIManager>();
        if (ui != null)
        {
            ui.ShowGameOver();
        }
    }
}