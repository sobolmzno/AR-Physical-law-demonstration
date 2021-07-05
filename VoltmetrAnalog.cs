using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;/* Use default UI and TMPro plugin */
using TMPro;

public class VoltmetrAnalog : MonoBehaviour
{
    /* Class that control Voltmetr. We get values from some Circle Controller and then change behavior of Voltmetr by this class
        Voltmetr object - 3D prefab that have Value face, arm pointer and show current value of resistance in electric circle 
         */



    [SerializeField]
    private GameObject CircleObj; /*Object of our circle(better use some prefab )*/
    public Transform armTransform; /*Transform for move our voltmetr arm*/

    [SerializeField]
    private TextMeshPro countB;/*Counter (UI field) of volts*/

    private int CountVolts = 0; /*Current value of volts*/
    private float MinDegree = 60; /*minimum value of degree (0 Volts)*/
    private float CurrDegree = 0; /*Current degree of arm*/
    private float DegreeTo = 0; /*Degree of destination*/
    private float NextDegree = 0; /*Degree to move*/
    private float stepOfAngle = 6f; /*Step of transform*/
    private bool cStatus = false; /*false mean that circle is switch off*/
    private CircleController cController; /*reference to our circle controller*/
    private void Awake()
    {
        /*Do first check and set our controller to default condition*/
        UpdateIsCurrent(); 
        CurrDegree = MinDegree;
        armTransform.localRotation = Quaternion.Euler(0,0, CurrDegree);
    }

    private void Update()
    {
        /*Update condition in realtime*/
        UpdateIsCurrent(); /*Check circle status*/
        UpdateCountText();/*Update UI element*/
        if (cStatus == true)
        {
            /*Do work while circle is switch on*/
            Debug.Log("Its work " + cStatus);
            doWork(); 
            
        }
        else
        {
            /*Do work if  circle was switched off*/
            Debug.Log("Its not work " + cStatus);
            resetWork();
        }

    }

    private void doWork()
    {
        /*Just check our values and update */
        UpdateCount();
        UpdateCountText();
        updateDegreeTo();
        updateNextDegree();
        UpdateArmPointer();
    }
    private void resetWork()
    {
        /*reset nums of volts and then recalculate our condition*/
        CountVolts = 0;
        UpdateCountText();
        updateDegreeTo();
        updateNextDegree();
        UpdateArmPointer();
    }

    private void UpdateIsCurrent()
    {
        /*Get circle status from circle controller*/
        cController = CircleObj.GetComponent<CircleController>();
        cStatus = cController.isCurrent;
    }
    private void UpdateCount()
    {
        /*Get resistance value from circle controller*/
        cController = CircleObj.GetComponent<CircleController>();
        CountVolts = cController.napruga;
    }
    private void updateDegreeTo()
    {
        /*Calculate value of destination degree */
        if (CountVolts == 0)/*Min value*/
        {
            DegreeTo = MinDegree;
        }
        if (CountVolts == 10)
        {
            DegreeTo = 0;
        }
        if(CountVolts < 10 | CountVolts >10)
        {
            DegreeTo = MinDegree - (CountVolts * stepOfAngle);
        }
        if (CountVolts > 20)/*Max value*/
        {
            DegreeTo = MinDegree - (20 * stepOfAngle);
        }
    }
    private void updateNextDegree()
    {/*Get next degree if current degree value != destination degree*/
        if(CurrDegree > DegreeTo)
        {
            NextDegree = CurrDegree - 1;
        }
        if (CurrDegree < DegreeTo)
        {
            NextDegree = CurrDegree + 1;
        }

    }
    private void UpdateArmPointer()
    {
        /*Set transform of arm to move it*/
        if (CurrDegree != DegreeTo)
        {
            armTransform.localRotation = Quaternion.Euler(0, 0, NextDegree);
            CurrDegree = NextDegree;
        }

    }

    private void UpdateCountText()
    {
        /*Set UI text*/
        string newText;
        TextMeshPro nText = countB.GetComponentInChildren<TextMeshPro>();
        newText = CountVolts.ToString();
        nText.text = "Volts now: " + newText + " V";
    }


}
