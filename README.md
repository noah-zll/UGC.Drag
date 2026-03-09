# UGC.Drag 通用拖拽/放置组件（UGUI）

[![Unity 2020.3+](https://img.shields.io/badge/Unity-2020.3%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)

UGC.Drag 是一个基于 Unity UGUI（EventSystem + GraphicRaycaster）的通用拖拽/放置（Drag & Drop）组件包，提供可复用的拖拽源（DragSource）与投放目标（DropTarget），并通过统一的拖拽上下文（DragContext）与事件回调，快速实现背包、装备槽、编辑器面板、列表/网格等常见 UI 拖拽交互。

## 功能特点

- ✅ 完整拖拽链路：BeginDrag / Drag / EndDrag 封装，统一结果回传
- ✅ 拖拽视觉：默认“影子/替身”视觉，跟随指针移动且不阻挡射线
- ✅ 目标命中：Hover Enter/Exit 回调，目标可按规则决定是否接收
- ✅ 跨 Canvas：支持跨多个 GraphicRaycaster 命中（RaycastAll）
- ✅ 易扩展：接口化，便于实现重排、交换、合并、跨容器移动

## 安装

### 方式一：通过 Package Manager 本地安装
1. 打开 `Window > Package Manager`
2. 点击 `+` 选择 `Add package from disk...`
3. 选择 `UGC.Drag/package.json`

### 方式二：通过 Git URL 安装
1. 打开 `Window > Package Manager`
2. 点击 `+` 选择 `Add package from git URL...`
3. 输入仓库地址并确认

## 快速入门

1. 场景包含 `EventSystem` 与带 `GraphicRaycaster` 的 `Canvas`
2. 在 UI 根节点挂 `DragService`，设置：
   - `raycaster`：Canvas 的 `GraphicRaycaster`
   - `visualRoot`：用于挂拖拽视觉的父节点（建议同 Canvas）
   - `useEventSystemRaycastAll`：勾选以支持跨多个 Canvas 命中
3. 可拖拽物体上挂 `DragSource`，把 `service` 指向上面的 `DragService`
4. 接收区域上挂 `DropTarget`；若需规则，额外实现并绑定 `IDropTarget`

## 样例

本包提供了“两区域往返拖放”的最小样例：

- 在 Package Manager 的 UGC.Drag 条目内导入 `Two Areas Drag-Drop Demo`
- 结构包含两个接收区与若干可拖拽项，可在区域之间互相拖放

## API 概览

### DragService
- `visualRoot`：拖拽视觉挂载父节点
- `raycaster`：默认的 GraphicRaycaster
- `useEventSystemRaycastAll`：使用 `EventSystem.current.RaycastAll` 聚合多 Raycaster
- `IsDragging` / `LastContext`：拖拽状态与最近上下文

### DragSource
- `service`：拖拽调度器引用
- `hideWhileDragging`：拖拽期间隐藏源物体（拖拽视觉仍可见）
- 事件：`onDragStarted`、`onDragEnded`
- 可选：实现 `IDragSource` 提供自定义 payload 与校验

### DropTarget
- `handler`：可绑定实现 `IDropTarget` 的脚本以声明接收规则与处理
- 事件：`onDragEnter`、`onDragExit`、`onDrop`

### 接口与数据
- `IDragSource`：`CanBeginDrag`、`BuildPayload`、`OnDragStarted/Ended`
- `IDropTarget`：`CanAccept`、`OnDragEnter/Exit`、`OnDrop`
- `IDragVisualFactory`：自定义拖拽视觉的生成
- `DragContext` / `DragResult`：拖拽上下文与结果

## 注意事项

- 拖拽视觉默认移除克隆中的 `DragSource/DropTarget` 并关闭 `raycastTarget`，避免干扰命中
- 跨 Canvas 拖放时，确保启用 `useEventSystemRaycastAll` 或自行指定 `raycaster`
- World Space Canvas 需要正确的事件相机以保证坐标换算

## 兼容性

- Unity 2020.3 或更高版本
- UGUI（UnityEngine.UI）与 EventSystem

## 许可证

MIT（详见 LICENSE.md）

