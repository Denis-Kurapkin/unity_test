using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInventory : MonoBehaviour
{

    HeroStats heroStats;
    Animator anim;


    public int money;


    public List<Item> food = new List<Item>();
    public List<Item> weapon = new List<Item>();

    public int typeOutput;

    public Drag mainWeapon;
    public GameObject weaponInHand;
    public Vector3 positionLeg;
    public Vector3 rotationLeg;
    public Transform rHand;
    public Transform leg;
    public Vector3 positionHand;
    public Vector3 rotationHand;

    public int weight;
    public int maxWeight;
    public Slider sliderWeinght;

    public List<Drag> drag;
    public GameObject inventory;

    public GameObject cell;
    public Transform cellParent;

    public AudioClip useApple;
    AudioSource audioSource;


    public GameObject descriptionObj;
    public Text descriptionItem;

    [HideInInspector]
    public bool Trade;
    public Dealer dealer;




    void Start()
    {
        typeOutput = 1;
        heroStats = GetComponent<HeroStats>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sliderWeinght.maxValue = maxWeight;
    }

    void Update()
    {
        //typeOutput = 1;
        InventoryActive();

        if (weaponInHand != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (anim.GetBool("Equip"))
                {
                    anim.SetBool("Equip", false);
                    anim.SetFloat("TypeEquip", 1);
                }
                else
                {
                    anim.SetBool("Equip", true);
                    anim.SetFloat("TypeEquip", 0);
                }
            }
        }
    }

    public void InventoryActive()
    {
        if(Input.GetKeyDown(KeyCode.I) && !Trade)
        {
            if (inventory.activeSelf)
            {
                InventoryDisable(); //������� ��������� ���� �� �������
            }
            else
            {
                InventoryEnabled();  //�������� ���������
            }
        }
    }

    public void InventoryDisable()
    {
        foreach (Drag drag in drag)
            Destroy(drag.gameObject); //�������
        drag.Clear();

        inventory.SetActive(false);
        descriptionObj.SetActive(false);
    }

    public void InventoryEnabled()
    {

        inventory.SetActive(true);

        foreach (Drag drag in drag)
            Destroy(drag.gameObject);
        drag.Clear();

        if (typeOutput == 1)
        {
            for (int i = 0; i < food.Count; i++)
            {
                GameObject newCell = Instantiate(cell);
                newCell.transform.SetParent(cellParent, false);
                drag.Add(newCell.GetComponent<Drag>());
            }


            for (int i = 0; i < food.Count; i++)
            {
                Item it = food[i];
                for (int j = 0; j < drag.Count; j++)
                {
                    if (drag[j].ownerItem != "")
                    {

                        if (food[i].isStackable)
                        {
                            if (drag[j].item.nameItem == it.nameItem)
                            {
                                drag[j].countItem++;
                                drag[j].count.text = drag[j].count.ToString();
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        drag[j].item = it;
                        drag[j].image.sprite = Resources.Load<Sprite>(it.pathSprite);
                        drag[j].ownerItem = "myItem";
                        drag[j].countItem++;
                        drag[j].count.text = "" + drag[j].countItem;
                        drag[j].heroInventory = this;
                        drag[j].descriptionObj = descriptionObj;
                        drag[j].descriptionItem = descriptionItem;
                        break;
                    }
                }
            }
        }
        else if(typeOutput == 2)
        {

            for (int i = 0; i < weapon.Count; i++)
            {
                GameObject newCell = Instantiate(cell);
                newCell.transform.SetParent(cellParent, false);
                drag.Add(newCell.GetComponent<Drag>());
            }

            for (int i = 0; i < weapon.Count; i++)
            {
                Item it = weapon[i];
                for (int j = 0; j < drag.Count; j++)
                {
                    if (drag[j].ownerItem != "")
                    {

                        if (weapon[i].isStackable)
                        {
                            if (drag[j].item.nameItem == it.nameItem)
                            {
                                drag[j].countItem++;
                                drag[j].count.text = drag[j].count.ToString();
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        drag[j].item = it;
                        drag[j].image.sprite = Resources.Load<Sprite>(it.pathSprite);
                        drag[j].ownerItem = "myItem";
                        drag[j].countItem++;
                        drag[j].count.text = "" + drag[j].countItem;
                        drag[j].heroInventory = this;
                        drag[j].descriptionObj = descriptionObj;
                        drag[j].descriptionItem = descriptionItem;
                        break;
                    }
                }
            }
        }
        for(int i = drag.Count - 1; i >= 0; i--)
        {
            if(drag[i].ownerItem == "")
            {
                Destroy(drag[i].gameObject);
                drag.RemoveAt(i);
            }
        }

    }


    public void OutputFood()
    {
        typeOutput = 1;
        InventoryEnabled();
    }
    public void OutputWeapon()
    {
        typeOutput = 2;
        InventoryEnabled();
    }

    public void RemoveItem(Drag drag)
    {
        Item it = drag.item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab)); //�������� �������� �� ����������� � ����
        newObj.transform.position = transform.position + transform.forward;        //����� ������� ��� ������������ ��������
        weight -= it.mass;
        sliderWeinght.value = weight;
        food.Remove(it);
        weapon.Remove(it);
       // newObj.tag = "Item";
        InventoryEnabled(); //���������� ���������

    }

    public void UseItem(Drag drag) //������������� �����
    {
        Item it = drag.item; //������ ������ Item ���������� it 

        if(it.typeItem == "Food") //���� ��� ����� "���" ��
        {
            heroStats.AddHealth(drag.item.addHealth); //�������������� ��
            food.Remove(drag.item); //�������� ���
            weight -= it.mass; //���������� ���� ���������
            sliderWeinght.value = weight; //���������� ���� �� ��������


            if (it.nameItem == "Green Apple" || it.nameItem == "Red Apple")
            {
                audioSource.PlayOneShot(useApple); //���� ���� ������� ��� ������� ������ �� ��������������� �����
            }
        }
        else if(it.typeItem == "Weapon") //���� ��� ����� "������"
        {
            if(drag.ownerItem == "myItem") //���� ���������� �������� "myItem", �.� �� ��������� � ���������
            {
                if (weaponInHand == null) //���� � ����� (weaponInHand - ������ ����� � ����������) ����� ��:
                {
                    GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab)); //�������� �������� ������� � �������� ����
                    newObj.transform.SetParent(leg); //��������� ���� � ����
                    newObj.transform.localPosition = positionLeg; //���������������� �� ����� ����
                    newObj.transform.localRotation = Quaternion.Euler(rotationLeg); //�������� �� ����� ����
                    newObj.GetComponent<Rigidbody>().isKinematic = true; //��������� ����������
                    newObj.GetComponent<BoxCollider>().enabled = false; //���������� ���� ����������
                    weaponInHand = newObj; //����������� � ������ ���� �������-����

                    mainWeapon.item = it; //�������� ������� (����������� � ������) ���������� ��������� �� ��������� ���
                    mainWeapon.image.sprite = Resources.Load<Sprite>(it.pathSprite); //������� ������
                    mainWeapon.ownerItem = "myWeapon"; //���������� ���������� ����� � ������������� myWeapon
                    mainWeapon.countItem++; //���-�� ������ � ���� ������������� (����� ������ ����� �� ����)
                    mainWeapon.count.text = ""; //�����-������� ���������� ������
                    mainWeapon.heroInventory = this; //
                    weapon.Remove(it); //�������� ������ �� ������
                }
            }
            else if (drag.ownerItem == "myWeapon") //���� �� ���������� �������� ����� (�.� ��� ��������� � ���� � ����� myWeapon)
            {
                weapon.Add(drag.item); //� ������ ��������� ����������� ������

                Destroy(weaponInHand); //������ � ���� ������������
                weaponInHand = null; //weaponInHand ����������

                mainWeapon.item = null; //������ ����������
                mainWeapon.image.sprite = mainWeapon.defaultSprite; //���������� ��������� ������ � ������ ���������
                mainWeapon.ownerItem = ""; 
                mainWeapon.countItem--;
                mainWeapon.count.text = "";
                mainWeapon.heroInventory = null;
            }


        }

        InventoryEnabled();  
    }

    public void Buy(Drag drag)
    {
        print("buy");
        Item it = drag.item;

        if (it.price <= money)
        {
            if (weight + it.mass <= maxWeight)
            {
                money -= it.price;
                weight += it.mass;
                sliderWeinght.value = weight;

                if (it.typeItem == "Food")
                {
                    food.Add(it);
                    dealer.item.Remove(it);

                    InventoryEnabled();
                    dealer.enabledInventory();
                }

                if (it.typeItem == "Weapon")
                {
                    weapon.Add(it);
                    dealer.item.Remove(it);

                    InventoryEnabled();
                    dealer.enabledInventory();
                }
            }
        }
    }

    public void TakeSword()
    {
        weaponInHand.transform.SetParent(rHand);
        weaponInHand.transform.localPosition = positionHand;
        weaponInHand.transform.localRotation = Quaternion.Euler(rotationHand);
    }

    public void InsertSword()
    {
        weaponInHand.transform.SetParent(leg);
        weaponInHand.transform.localPosition = positionLeg;
        weaponInHand.transform.localRotation = Quaternion.Euler(rotationLeg);
    }

}
