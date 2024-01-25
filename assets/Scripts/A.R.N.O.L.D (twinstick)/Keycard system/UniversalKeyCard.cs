using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UniversalKeyCard : MonoBehaviour
{
    public KeyCardPickupSO _KeyCardPickUp;
    public Image _MyKeycardImage;

    private void Start()
    {
        Image image = FindObjectOfType<Image>();

        _MyKeycardImage = image;
    }
}
