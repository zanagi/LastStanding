using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {

    [SerializeField] private Image barImage;
    [SerializeField] private float fillChangeSpeed = 0.01f;
    [SerializeField] private Transform scaleTransform;
    private float targetFill;

    public void SetFill(float fill, bool instant = false)
    {
        if(instant)
        {
            barImage.fillAmount = fill;
        } 
        targetFill = fill; 
    }

    private void Update()
    {
        if(barImage.fillAmount < targetFill)
        {
            barImage.fillAmount += Mathf.Min(targetFill - barImage.fillAmount, fillChangeSpeed);
        } else if(barImage.fillAmount > targetFill)
        {
            barImage.fillAmount -= Mathf.Min(barImage.fillAmount - targetFill, fillChangeSpeed);
        } else
        {
            return;
        }

        if(scaleTransform)
        {
            scaleTransform.localScale = Vector3.one * barImage.fillAmount;
        }
    }
}
