using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {

    [SerializeField] private Image barImage;

	public void SetFill(float fill)
    {
        barImage.fillAmount = fill; 
    }
}
