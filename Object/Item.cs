using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameItem;
    public string typeItem;
    [Multiline(10)]
    public string description;
    public int price;
    public int mass;
    public int addHealth;
    public string pathSprite;
    public string pathPrefab;
    public bool isStackable;

    public Vector3 position;
    public Vector3 rotation;
    public int damage;
}
