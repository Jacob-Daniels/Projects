using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    [Header("Components:")]
    MeshRenderer meshRend;

    [Header("Mouse Hover:")]
    public bool hovered = false;

    [Header("Object Materials:")]
    public Material material;
    private Color materialColour;
    private Color highlightColour = Color.yellow;

    private void Awake()
    {
        meshRend = GetComponent<MeshRenderer>();
        material = meshRend.material;
        materialColour = material.color;
    }

    private void Update()
    {
        if (hovered)
        {
            MouseHovered();
        } else
        {
            ExitHover();
        }
    }

    private void MouseHovered()
    {
        // Set hover colour
        material.color = highlightColour;
        meshRend.material.color = highlightColour;
    }

    private void ExitHover()
    {
        // Reset material
        meshRend.material.color = materialColour;
    }

    private void OnMouseExit()
    {
        hovered = false;
    }
}
