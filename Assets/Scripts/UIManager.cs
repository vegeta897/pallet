using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{

    public Text TxtMoney;
    public Text TxtWorkers;
    public Text TxtUntilPayday;
    public Text TxtPaydayAmount;
    public Button BtnHire;
    public Button BtnFire;
    public InputField InpWage;

    private Warehouse warehouse;

    public void SetWage()
    {
        warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text)*100)/100;
        InpWage.text = warehouse.Wage.ToString("F2");
    }

	void Start () 
    {
        warehouse = FindObjectOfType<Warehouse>();
	}
	
    void LateUpdate()
    {
        TxtMoney.text = "$" + warehouse.Money.ToString("F2");
        TxtWorkers.text = "Workers: " + warehouse.Workers;
        TxtUntilPayday.text = "Next Payday: " + Mathf.CeilToInt(warehouse.NextPayday - Time.time);
        TxtPaydayAmount.text = "Payday Cost: " + (warehouse.Workers * warehouse.Wage).ToString("F2");
        BtnFire.interactable = warehouse.Workers > 0;
	}
}