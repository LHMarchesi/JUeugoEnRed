using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;

    private PhotonView photonView;
    private PlayerContext context;
    private Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        photonView = GetComponentInParent<PhotonView>();
        context = GetComponentInParent<PlayerContext>();
       
        cam = GetComponent<Camera>();
        if(!photonView.IsMine)
        { 
            cam.enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float mouseX = context.HandleInputs.GetLookVector2().x * mouseSensitivity * Time.deltaTime;
            float mouseY = context.HandleInputs.GetLookVector2().y * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

}
