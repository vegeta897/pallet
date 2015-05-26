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
    public GameObject PanActionList;
    public GameObject PanActionItems;
    public ActionItem PanActionItem;

    private Warehouse warehouse;

    public void SetWage()
    {
        warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text)*100)/100;
        InpWage.text = warehouse.Wage.ToString("F2");
    }

    public void AddActionItem(Delivery newDelivery)
    {
        ActionItem actionItem = Instantiate(PanActionItem) as ActionItem;
        actionItem.transform.SetParent(PanActionItems.transform, false);
        actionItem.Delivery = newDelivery;
    }

	void Start () 
    {
        warehouse = FindObjectOfType<Warehouse>();
        foreach (Transform child in PanActionItems.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
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