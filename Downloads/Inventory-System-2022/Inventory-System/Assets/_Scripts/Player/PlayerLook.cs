using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook instance;

    [Header("Components:")]
    public Transform player;

    [Header("Mouse Settings:")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        // Lock and hide mouse in game
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Grab mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (!UserInterfaceManager.instance.isOpen) // Allow player to look if game isnt paused
        {
            // Flip and allow rotation on Y axis
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // Rotate player and camera on X axis
            player.Rotate(Vector3.up * mouseX);
        }
    }

    public void MouseSensitivity(float sliderValue)
    {
        mouseSensitivity = sliderValue;
    }
}
