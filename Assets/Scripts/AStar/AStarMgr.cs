using UnityEngine;
using System.Collections.Generic;

public class AStarMgr
{
    private static AStarMgr instance;

    public static AStarMgr Instance
    {
        get
        {
            if(instance == null)
                instance = new AStarMgr();
            return instance;
        }
    } 

    private int mapW;   //地图宽高
    private int mapH;

    public AStarNode[,] nodes;     //格子对象数组，地图
    private List<AStarNode> openList = new List<AStarNode>();   //开启列表
    private List<AStarNode> closeList = new List<AStarNode>();  //关闭列表

    public void InitMapInfo(int w, int h)
    {
        mapW = w;
        mapH = h;

        nodes = new AStarNode[w, h];

        for(int i = 0; i < w; i++)
        {
            for(int j = 0; j < h; j++)
            {
                //真正项目中，从地图配置文件读取地图信息
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? E_Node_Type.Stop : E_Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }
    
    public List<AStarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        //实际项目中传入的往往是坐标系中的位置
        //省略换算步骤，认为传入的点是格子坐标

        if (startPos.x < 0 || startPos.x >= mapW || 
            startPos.y < 0 || startPos.y >= mapH || 
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("开始或结束点在地图范围外");
            return null;
        }    
            

        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];

        if(start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("开始或结束点是阻挡");
            return null;
        }
        
        //清空上一次的相关数据，避免影响本次寻路
        closeList.Clear();
        openList.Clear();

        start.father = null;
        start.f = 0; 
        start.g = 0; 
        start.h = 0;
        openList.Add(start);

        while(openList.Count > 0)
        {
            //按f值升序排列
            openList.Sort(SortOpenList);
            //放入close表
            closeList.Add(openList[0]);
            //取f最小点作为新起点
            start = openList[0];
            //将f最小点移出open表
            openList.RemoveAt(0);

            if(start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }

                path.Reverse();
                return path;
            }

            FindNearlyNodeToOpenList(start.x, start.y + 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y, start, end);
            FindNearlyNodeToOpenList(start.x, start.y - 1, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, start, end);
        }
        
        return null;
    }

    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if(a.f > b.f) return 1;
        else return -1;
    }
    //放入开启列表函数
    private void FindNearlyNodeToOpenList(int x, int y, AStarNode father, AStarNode end)
    {
        if (x < 0 || x >= mapW ||
            y < 0 || y >= mapH)
            return;

        AStarNode node = nodes[x, y];

        if (node == null ||
            node.type == E_Node_Type.Stop ||
            closeList.Contains(node))
            return;

        float newG = father.g + 1;

        if(!openList.Contains(node) || newG < node.g)
        {
            node.g = newG;
            node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
            node.f = node.g + node.h;
            node.father = father;

            if(!openList.Contains(node))
            {
                openList.Add(node);
            }
        }
    }
}
