using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    #region Singleton
    public static Controls instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    [Header("Hotbar Keys:")]
    public KeyCode[] hotBarKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    [Header("Interact Keys:")]
    public KeyCode[] interactKeys = { KeyCode.Mouse0, KeyCode.E };

    [Header("Player Controls:")]
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode[] movement = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };

    [Header("Inventory:")]
    public KeyCode inventory = KeyCode.Tab;
    public KeyCode[] inventoryShortcuts = { KeyCode.LeftShift};
}
