# UIPanelGenerator
基于 Unity2018.4.12f1 引擎、NGUI3.0 插件和暂不开放的 .NET Framework 3.5 C# 底层框架开发。  
CreateUIPanel：用来生成Panel的主代码  
UIPanelBase：存储UIPanel属性，另EditorWindow属性  


## CreateUIPanel需更改  
CreateUIPanelPrefabs方法中`string fatherFolderPath = "Assets/Resources/UI/NewPanel";` //更改为你的预制体存储位置，同理更改CheckFolder方法  
CreateManagerScript方法中`string fatherFolderPath = "Assets/Code/Modules/Logic";` //更改为你的Manager脚本存储位置  
CreatePanelScript方法中`string fatherFolderPath = "Assets/Code/Modules/Panel";` //更改为你的Panel脚本存储位置  

## CreateUIPanel涉及知识  
### [ReorderableList](https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/)  
以后有时间的话自己填坑做教程
