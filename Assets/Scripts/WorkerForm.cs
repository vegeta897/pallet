using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorkerForm : MonoBehaviour
{

    public InputField InpWage;
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

    void Start()
    {

    }

    void Update()
    {

    }
}