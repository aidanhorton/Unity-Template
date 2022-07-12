using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public float RunningSpeed = 6;
    public float WalkingSpeed = 4;
    public float Sensitivity = 3;
    public Vector2 maxMinX = new(-80, 80);
    
    public Transform cameraContainer;
    
    private Vector2 rot;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        rot.x += Input.GetAxis("Mouse X") * this.Sensitivity;
        rot.y += Input.GetAxis("Mouse Y") * this.Sensitivity;

        rot.y = Mathf.Clamp(rot.y, maxMinX.x, maxMinX.y);

        transform.localEulerAngles = new Vector3(0, rot.x, 0);
        cameraContainer.localEulerAngles = new Vector3(-rot.y, 0, 0);

        var running = Input.GetKey(KeyCode.LeftShift);
        var curSpeedX = Input.GetAxis("Vertical");
        var curSpeedZ = Input.GetAxis("Horizontal");

        var movementVector = (transform.forward * curSpeedX + transform.right * curSpeedZ) * (running ? this.RunningSpeed : this.WalkingSpeed);

        controller.SimpleMove(movementVector);
    }
}