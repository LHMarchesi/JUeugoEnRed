using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int PlayerID;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float jumpHeight;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;


    private Vector3 velocity;
    private bool isGrounded;

    private PhotonView photonView;
    private PlayerContext playerContext;
    private Vector3 conveyorSpeed;

    public float RunningSpeed { get => runningSpeed; private set { } }
    public float WalkingSpeed { get => walkingSpeed; private set { } }


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerContext = GetComponent<PlayerContext>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = playerContext.HandleInputs.GetMoveVector2().x;
            float z = playerContext.HandleInputs.GetMoveVector2().y;
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move((move + conveyorSpeed) * currentSpeed * Time.deltaTime);


            if (playerContext.HandleInputs.IsJumping() && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void ChangeSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    public void SetConveyorSpeed(Vector3 speed)
    {
        conveyorSpeed = speed;
    }
}
