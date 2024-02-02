using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Cell[] cells {get; private set; }
    void Awake()
    {
        cells=GetComponentsInChildren<Cell>();
        
    }
}
