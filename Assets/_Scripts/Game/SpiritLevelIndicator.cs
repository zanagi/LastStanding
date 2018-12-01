using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritLevelIndicator : MonoBehaviour {

    [SerializeField] private TextMesh text;

    public void SetLevel(int level)
    {
        text.text = level.ToString();
    }
}
