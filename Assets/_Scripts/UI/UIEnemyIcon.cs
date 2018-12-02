using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyIcon : MonoBehaviour {

    private static float targetMargin = 0.4f;
    
    public void SetOrientation(Vector3 playerPos, Vector3 enemyPos)
    {
        var camera = GameManager.Instance.gameCamera.Camera;
        Vector2 vPos = camera.WorldToViewportPoint(enemyPos);
        Vector2 center = Vector2.one / 2;

        if (vPos.x <= 0 || vPos.x >= 1 || vPos.y <= 0 || vPos.y >= 1)
        {
            gameObject.SetActive(true);

            var worldDelta = enemyPos - playerPos;
            var temp = new Vector2(worldDelta.x, worldDelta.z);

            transform.position = camera.ViewportToScreenPoint(center + 
                temp / Mathf.Max(Mathf.Abs(temp.x), Mathf.Abs(temp.y)) * targetMargin);
            transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(temp, Vector2.up));
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
