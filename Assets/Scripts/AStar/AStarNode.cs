using UnityEngine;

//格子类型
public enum E_Node_Type
{
    Walk,
    Stop,
}

///
/// A*格子类
/// 
public class AStarNode
{
    public int x;       //横纵坐标
    public int y;

    public float f;     //寻路消耗
    public float g;     //离起点的距离
    public float h;     //离终点的距离

    public AStarNode father;    //父节点

    public E_Node_Type type;

    public AStarNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
