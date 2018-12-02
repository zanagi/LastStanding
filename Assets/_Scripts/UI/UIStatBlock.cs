using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBlock : MonoBehaviour {
    
    [SerializeField] private Image healthBlock;
    [SerializeField] private Text levelText;


    public void SetValues(Vector3 enemyPos, float healthRatio, Vector3 margin)
    {
        var camera = GameManager.Instance.gameCamera.Camera;
        transform.position = camera.WorldToScreenPoint(enemyPos + margin);
        healthBlock.fillAmount = healthRatio;
    }

    public void SetValues(Vector3 enemyPos, float healthRatio, Vector3 margin, int level)
    {
        levelText.text = level.ToString();
        SetValues(enemyPos, healthRatio, margin);
    }
}
