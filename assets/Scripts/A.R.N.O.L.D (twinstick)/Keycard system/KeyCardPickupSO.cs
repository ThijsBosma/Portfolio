using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "KeyCard" ,menuName = "KeyCardPickup")]
public class KeyCardPickupSO : ScriptableObject
{
    public string _KeycardType;
    public Sprite _KeycardImage;
}
