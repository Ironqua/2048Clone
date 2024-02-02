using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
[SerializeField] private Tile tilePrefab;
[SerializeField] private TileStateSO[] tileStates;
[SerializeField] float animateduration=0.25f;
bool isWaiting;
private List<Tile> tiles=new List<Tile>();

private TileGrid grid;

void Awake()
{
    grid=GetComponentInChildren<TileGrid>();

}

void Start()
{
    CreateTile();
    CreateTile();
}


void Update()
{
 if(!isWaiting){
    if(Input.GetKeyDown(KeyCode.W)){
MoveTiles(Vector2Int.up,0,1,1,1);
    }
    else if(Input.GetKeyDown(KeyCode.S))
    {
MoveTiles(Vector2Int.down,0,1,grid.GetHeight()-2,-1);
    }
    else if(Input.GetKeyDown(KeyCode.A))
    {
MoveTiles(Vector2Int.left,1,1,0,1);
    }
    else if(Input.GetKeyDown(KeyCode.D))
    {
MoveTiles(Vector2Int.right,grid.GetWidth()-2,-1,0,1);

    }
 }

}

private void MoveTiles(Vector2Int direction,int startX, int incrementX, int startY,int incrementY)
{
    bool isChanged=false;
    for(int x=startX;x>=0&& x<grid.GetWidth();x+=incrementX)
    {
        for(int y=startY;y>=0&&y<grid.GetHeight();y+=incrementY)
        {
           Cell cell= grid.GetCell(x,y);
           if(cell.isOccupied){
            isChanged|=MoveTile(cell.tile,direction);
           }

        }
    }
    if(isChanged)
    {
StartCoroutine(waitforChanges());
    }

}

private bool MoveTile(Tile tile,Vector2Int direction)
{
Cell newCell=null;
Cell adjacentCell=grid.GetAdjacentCell(tile.cell,direction);

while(adjacentCell!=null)
{
    if(adjacentCell.isOccupied)
    {
        if(CanMerge(tile,adjacentCell.tile))
        {
            Merge(tile,adjacentCell.tile);
            return true;
        }
break;
    }
    newCell=adjacentCell;
    adjacentCell=grid.GetAdjacentCell(adjacentCell,direction);

}
if(newCell!=null)
{
    tile.MoveTo(newCell);
    return true;
}
return false;

}


private void Merge(Tile a,Tile b)
{
    tiles.Remove(a);
    a.Merge(b.cell);
    int index=Mathf.Clamp(IndexOF(b.tileState)+1,0,tileStates.Length-1);
    int number=b.number*2;

    b.SetState(tileStates[index],number);

    AnimateTiles(b,animateduration);

}

private void AnimateTiles(Tile tileAnimate,float animateduration)
{
    tileAnimate.gameObject.transform.DOScale(1.25f,animateduration).OnComplete(()=>
    {
        tileAnimate.gameObject.transform.DOScale(1f,animateduration);
    }
    );
}

private bool CanMerge(Tile a,Tile b)
{
    return a.number==b.number&&!b.isLocked;
}





public void CreateTile()
{
    Tile tile=Instantiate(tilePrefab,grid.transform);
    tile.SetState(tileStates[0],Consts.Numbers.NUMBER_2);
    tile.SpawnTile(grid.GetRandomEmptyCell());
    tiles.Add(tile);


}

private int IndexOF(TileStateSO tileState)
{
    for(int i=0;i<tileStates.Length;i++)
    {
        if(tileState==tileStates[i])
        {
            return i;
        }
    }

    return -1;
}

private IEnumerator waitforChanges()
{
isWaiting=true;

yield return new WaitForSeconds(0.1f);
isWaiting=false;
foreach(Tile tile in tiles){
    tile.isLocked=false;

}
if(tiles.Count!=grid.GetSize())
{
    CreateTile();
}
}







bool CheckForGameOver()
{

    if(tiles.Count!=grid.GetSize())
    {
        return false;
    }
    foreach(Tile tile in tiles)
    {
        Cell upcell=grid.GetAdjacentCell(tile.cell,Vector2Int.up);
        Cell downCell=grid.GetAdjacentCell(tile.cell,Vector2Int.down);
        Cell rightcell=grid.GetAdjacentCell(tile.cell,Vector2Int.right);
        Cell leftcell=grid.GetAdjacentCell(tile.cell,Vector2Int.left);



       if(upcell!=null&&CanMerge(tile,upcell.tile))
       {
        return false;
       } 
       else if(downCell!=null&&CanMerge(tile,downCell.tile))
       {
        return false;
       }
       else if(rightcell!=null&&CanMerge(tile,rightcell.tile))

       {
        return false;
       }
        else if(leftcell!=null&&CanMerge(tile,leftcell.tile))
        {
            return false;
        }    }

        return true;
}

}
