using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserInterfaceManager : MonoBehaviour
{
    #region Singleton
    public static UserInterfaceManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [Header("UI Types:")]
    public UIState[] UIArray;
    public bool isOpen;

    private void Update()
    {
        if (CheckToOpenUI())
        {
            isOpen = false;
        } else
        {
            isOpen = true;
        }
    }

    public bool OpenUI(string name) // Open ui element
    {
        if (CheckToOpenUI())
        {
            foreach (UIState uiElement in UIArray)
            {
                if (uiElement.UIName == name)
                {
                    uiElement.isActive = true;
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckToOpenUI() // Check no other UI elements are open
    {
        foreach (UIState uiElement in UIArray)
        {
            if (uiElement.isActive)
            {
                return false;
            }
        }
        return true;
    }

    public void CloseUI(string name)    // Close a ui system
    {
        foreach (UIState uiElement in UIArray)
        {
            if (uiElement.UIName == name)
            {
                uiElement.isActive = false;
            }
        }
    }
}
