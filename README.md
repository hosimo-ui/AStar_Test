# Unity A* 寻路算法演示

## 项目简介

这是一个基于 Unity 的 A* 寻路算法演示项目，用于学习和测试 A* 路径搜索算法。

### 核心功能

- **网格化地图**：基于 5×5 网格的二维地图
- **A* 算法**：标准的 A* 路径搜索实现
- **四方向移动**：支持上、下、左、右四个方向的移动
- **启发式函数**：使用曼哈顿距离（Manhattan Distance）作为启发值
- **可视化演示**：通过立方体颜色直观展示起点、障碍物和寻路结果

### 技术栈

| 组件 | 版本 |
|------|------|
| Unity | 6000.3.10f1 |
| 渲染管线 | Universal Render Pipeline (URP) |
| 输入系统 | Input System 1.18.0 |
| AI 导航 | com.unity.ai.navigation 2.0.10 |

---

## 算法说明

### 核心数据结构

#### AStarNode（节点类）

```csharp
- x, y          // 网格坐标
- f             // 总代价 = g + h
- g             // 从起点到当前节点的代价
- h             // 从当前节点到终点的预估代价（启发值）
- father        // 父节点引用（用于路径重建）
- type          // 节点类型：Walk（可通行）或 Stop（障碍）
```

#### AStarMgr（管理器类）

```csharp
- nodes         // AStarNode[,] 二维数组，表示整个网格地图
- openList      // 待检测节点列表
- closeList     // 已检测节点列表
```

### 算法流程

1. **初始化**：创建指定大小的网格地图，随机生成约 20% 的障碍物
2. **起点入栈**：将起始节点加入 openList
3. **主循环**：
   - 从 openList 中取出 f 值最小的节点
   - 若取出的是终点，则路径找到，结束
   - 否则将该节点加入 closeList
   - 将其所有可通行的邻居节点加入 openList
4. **路径重建**：通过 father 指针链从终点回溯到起点

### 关键公式

- **曼哈顿距离**（启发函数）：`h = |end.x - node.x| + |end.y - node.y|`
- **总代价**：`f = g + h`
- **移动代价**：每移动一步，g 值增加 1

---

## 目录结构

```
A_star/
├── Assets/
│   ├── Scripts/
│   │   ├── AStar/
│   │   │   ├── AStarMgr.cs      # A* 算法管理器（核心寻路逻辑）
│   │   │   └── AStarNode.cs     # 节点类（存储节点数据）
│   │   └── Test/
│   │       └── TestAStar.cs     # 测试脚本（可视化演示）
│   ├── Scenes/
│   │   └── SampleScene.unity    # 演示场景
│   └── Resources/
│       └── UI/
│           ├── Red.mat          # 障碍物材质（红色）
│           ├── Yellow.mat        # 起点材质（黄色）
│           └── Green.mat         # 路径材质（绿色）
├── Packages/
├── ProjectSettings/
├── .vscode/
└── A_star.slnx                  # Unity 解决方案文件
```

### 核心文件说明

| 文件 | 作用 |
|------|------|
| `AStarNode.cs` | 定义寻路网格中的单个节点，包含坐标、代价和类型 |
| `AStarMgr.cs` | A* 算法核心实现，管理地图和寻路逻辑 |
| `TestAStar.cs` | 演示脚本，挂载在 Camera 上，处理点击交互和可视化 |
| `SampleScene.unity` | Unity 场景文件，包含测试环境 |

---

## 使用方法

1. 在 Unity 中打开该项目
2. 打开 `Assets/Scenes/SampleScene.unity` 场景
3. 运行游戏
4. **操作步骤**：
   - 第一次点击立方体：设置为起点（变为黄色）
   - 第二次点击立方体：设置为终点，自动计算路径（路径显示为绿色）
