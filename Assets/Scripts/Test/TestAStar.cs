using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public Material red;
    public Material yellow;
    public Material green;
    public Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    public Vector2 beginPos = Vector2.right * -1;
    //起点
    public int beginX = -3;
    public int beginY = 5;

    //每个立方体的偏移位置
    public int offsetX = 2;
    public int offsetY = 2;

    //地图格子宽高
    public int mapW = 5;
    public int mapH = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AStarMgr.Instance.InitMapInfo(mapH, mapW);
        for(int i = 0; i < mapW; i ++)
        {
            for(int j = 0; j < mapH; j ++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);

                //名字
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);

                AStarNode node = AStarMgr.Instance.nodes[i, j];
                switch (node.type)
                {
                    case E_Node_Type.Stop:
                        obj.GetComponent<MeshRenderer>().material = red;
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out info, 1000))
            {
                Debug.Log("命中物体：" + info.collider.gameObject.name);
                if(beginPos == Vector2.right * -1)
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                }
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));

                    List<AStarNode> list = AStarMgr.Instance.FindPath(beginPos, endPos);

                    if(list != null)
                    {
                        for(int i = 0; i < list.Count; i ++)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("未命中任何物体");
            }
        }
    }
}
