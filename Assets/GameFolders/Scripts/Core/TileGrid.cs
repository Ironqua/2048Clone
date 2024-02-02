using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
 
public Row[] rows { get;private  set; }
public Cell[] cells { get; private set; }


[SerializeField] int size =>cells.Length;
[SerializeField] int height =>rows.Length;

[SerializeField] int width=>size/height;



void Awake()
{
    rows=GetComponentsInChildren<Row>();
    cells=GetComponentsInChildren<Cell>();

}
void Start()
{
    // y axis
    for(int x=0;x<rows.Length;x++)
    {
        // x axis
        for(int j=0;j<rows[x].cells.Length;j++)
        {
            rows[x].cells[j].coordinates=new Vector2Int(j,x);

        }
    }
}

public Cell GetRandomEmptyCell()
{
    int index=Random.Range(0,cells.Length);
    int startingIndex=index;

    while(cells[index].isOccupied)
    {
        index++;
        if(index>=cells.Length)
        {
            index=0;

        }
        if(index==startingIndex)
        {
            return null;
        }
    }
    return cells[index];

}

public Cell GetAdjacentCell(Cell cell,Vector2Int direction)
{
Vector2Int coordinates =cell.coordinates;
coordinates.x+=direction.x;
coordinates.y-=direction.y;
return GetCell(coordinates.x,coordinates.y);

}
public Cell GetCell(int x , int y)
{
if(x>=0&& x<width&&y>=0&&y<height)
{
    return rows[y].cells[x];

}
else
{
    return null;
}
}

#region GetSHW
public int GetSize()
{
    return size;
}

public int GetHeight(){
    return height;

}

public int GetWidth(){
    return width;
}
#endregion



}
