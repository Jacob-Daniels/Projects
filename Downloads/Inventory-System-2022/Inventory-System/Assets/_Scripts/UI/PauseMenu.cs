using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Singleton
    public static PauseMenu instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    [Header("Pause state:")]
    public GameObject pauseMenu;

    [Header("Settings Components:")]
    public Slider sensitivitySlider;

    private void Start()
    {
        sensitivitySlider.value = PlayerLook.instance.mouseSensitivity;
    }

    private void Update()
    {
        // Enable Pause Screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UserInterfaceManager.instance.UIArray[0].isActive)
            {
                Inventory.instance.CloseInventory();
            }
            if (UserInterfaceManager.instance.OpenUI("PauseMenu"))
            {
                PauseGame();
            }
        }
    }

    public void PauseGame() // Pause Game
    {
        MouseState(false);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()    // Resume Game
    {
        MouseState(true);
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void MouseState(bool lockMouse)  // Change mouse state
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void MainMenu()
    {
        ResumeGame();
        // Load first scene (Main Menu)
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
    }
}
