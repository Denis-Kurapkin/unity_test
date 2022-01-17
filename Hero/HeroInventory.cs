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
                InventoryDisable(); //скрытие инвентар€ если он активен
            }
            else
            {
                InventoryEnabled();  //открытие инвентар€
            }
        }
    }

    public void InventoryDisable()
    {
        foreach (Drag drag in drag)
            Destroy(drag.gameObject); //очистка
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
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab)); //создание предмета из полученного с пола
        newObj.transform.position = transform.position + transform.forward;        //нова€ позици€ дл€ выброшенного предмета
        weight -= it.mass;
        sliderWeinght.value = weight;
        food.Remove(it);
        weapon.Remove(it);
       // newObj.tag = "Item";
        InventoryEnabled(); //обновление инвентар€

    }

    public void UseItem(Drag drag) //использование итема
    {
        Item it = drag.item; //объект класса Item называетс€ it 

        if(it.typeItem == "Food") //если тип итема "еда" то
        {
            heroStats.AddHealth(drag.item.addHealth); //восстановление хп
            food.Remove(drag.item); //удаление еды
            weight -= it.mass; //уменьшение веса инвентар€
            sliderWeinght.value = weight; //уменьшение веса на ползунке


            if (it.nameItem == "Green Apple" || it.nameItem == "Red Apple")
            {
                audioSource.PlayOneShot(useApple); //если итем зеленое или красное €блоко то воспроизводитс€ саунд
            }
        }
        else if(it.typeItem == "Weapon") //если тип итема "оружие"
        {
            if(drag.ownerItem == "myItem") //если владельцем €вл€етс€ "myItem", т.е он находитс€ в инвентаре
            {
                if (weaponInHand == null) //если в руках (weaponInHand - €чейка р€дом с персонажем) пусто то:
                {
                    GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab)); //создание игрового объекта с префабом меча
                    newObj.transform.SetParent(leg); //установка меча в ногу
                    newObj.transform.localPosition = positionLeg; //позиционирование по левой ноге
                    newObj.transform.localRotation = Quaternion.Euler(rotationLeg); //вращение по левой ноге
                    newObj.GetComponent<Rigidbody>().isKinematic = true; //включение кинематики
                    newObj.GetComponent<BoxCollider>().enabled = false; //отключение бокс коллайдера
                    weaponInHand = newObj; //перемещение в €чейку руки объекта-меча

                    mainWeapon.item = it; //основным оружием (наход€щимс€ в €чейке) становитс€ кликнутый из инвентар€ меч
                    mainWeapon.image.sprite = Resources.Load<Sprite>(it.pathSprite); //рисовка иконки
                    mainWeapon.ownerItem = "myWeapon"; //владельцем становитс€ игрок и присваиваетс€ myWeapon
                    mainWeapon.countItem++; //кол-во итемов в руке увеличиваетс€ (можно убрать нахуй не надо)
                    mainWeapon.count.text = ""; //текст-счетчик становитс€ пустым
                    mainWeapon.heroInventory = this; //
                    weapon.Remove(it); //удаление оружи€ из списка
                }
            }
            else if (drag.ownerItem == "myWeapon") //если же владельцем €вл€етс€ игрок (т.е меч находитс€ в руке с тегом myWeapon)
            {
                weapon.Add(drag.item); //в €чейку инвентар€ добавл€етс€ оружие

                Destroy(weaponInHand); //оружие в руке уничтожаетс€
                weaponInHand = null; //weaponInHand занул€етс€

                mainWeapon.item = null; //€чейка занул€етс€
                mainWeapon.image.sprite = mainWeapon.defaultSprite; //становитс€ дефолтный спрайт и прочие обнулени€
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
