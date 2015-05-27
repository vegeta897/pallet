using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIManager : MonoBehaviour
{

    public Text TxtMoney;
    public Text TxtWorkers;
    public Text TxtUntilPayday;
    public Text TxtPaydayAmount;
    public Text TxtStockAmount;
    public Button BtnHire;
    public Button BtnFire;
    public InputField InpWage;
    public GameObject PanPendingList;
    public GameObject PanPendingItems;
    public PendingActiveItem BtnPendingItem;
    public GameObject PanActiveList;
    public GameObject PanActiveItems;
    public PendingActiveItem BtnActiveItem;
    public PendingItemActions PanPendingItemActions;
    public PendingItemActions PanActiveItemActions;

    private Warehouse warehouse;

    public void SetWage()
    {
        warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text) * 100) / 100;
        InpWage.text = warehouse.Wage.ToString("F2");
    }

    public void AddPendingItem(Delivery newDelivery)
    {
        PendingActiveItem actionItem = Instantiate(BtnPendingItem) as PendingActiveItem;
        actionItem.transform.SetParent(PanPendingItems.transform, false);
        actionItem.Delivery = newDelivery;
        actionItem.ItemType = "pending";
        actionItem.UIManager = this;
    }
    public void SelectPendingActiveItem(int index, string itemType)
    {
        PanPendingItemActions.gameObject.SetActive(index >= 0 && itemType == "pending");
        PanActiveItemActions.gameObject.SetActive(index >= 0 && itemType == "active");
    }

    void Start()
    {
        warehouse = FindObjectOfType<Warehouse>();
        foreach (Transform child in PanPendingItems.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in PanActiveItems.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
        InpWage.textComponent.alignment = TextAnchor.MiddleRight;
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        TxtMoney.text = "$" + warehouse.Money.ToString("F2");
        TxtWorkers.text = "Workers: " + warehouse.Workers;
        TxtUntilPayday.text = "Next Payday: " + Mathf.CeilToInt(warehouse.NextPayday - Time.time);
        TxtPaydayAmount.text = "Payday Cost: " + (warehouse.Workers * warehouse.Wage).ToString("F2");
        TxtStockAmount.text = "Stock Amount: " + warehouse.Stock;
        BtnFire.interactable = warehouse.Workers > 0;
    }
}