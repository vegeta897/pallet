using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorkerForm : MonoBehaviour
{

    public InputField InpWage;
    public Text TxtStartTime;
    public Text TxtEndTime;
    public Slider SliStartTime;
    public Slider SliEndTime;
    public Button BtnHire;
    public Button BtnCancel;

    public void ShowHireForm()
    {
        gameObject.SetActive(true);
    }

    public void HideHireForm()
    {
        gameObject.SetActive(false);
    }
    public void ChangeStartTime()
    {
        int val = (int)SliStartTime.value;
        TxtStartTime.text = (val == 0 ? 12 : val > 12 ? val - 12 : val) + (val > 11 ? "p" : "a");
        BtnHire.interactable = SliStartTime.value != SliEndTime.value;
    }
    public void ChangeEndTime()
    {
        int val = (int)SliEndTime.value;
        TxtEndTime.text = (val == 0 ? 12 : val > 12 ? val - 12 : val) + (val > 11 ? "p" : "a");
        BtnHire.interactable = SliStartTime.value != SliEndTime.value;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}