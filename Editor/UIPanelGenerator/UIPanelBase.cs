using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIPanelGenerator
{
    //[CreateAssetMenu]
    [Serializable]
    public class UIPanelBase : ScriptableObject
    {
        public List<PanelBaseInfo> m_PanelBaseInfos = new List<PanelBaseInfo>();

        [Serializable]
        public class PanelBaseInfo
        {
            #region UIPanel属性
            /// <summary>
            /// UIPanel的名字
            /// </summary>
            public string m_Name;
            /// <summary>
            /// 是否为名字自动添加后缀
            /// </summary>
            public bool m_IsAddSuffix;
            /// <summary>
            /// UIPanel的ID
            /// </summary>
            public int m_Id;
            /// <summary>
            /// 是否在面板中显示了ID的注释
            /// </summary>
            public bool m_IsShowIdComment;
            /// <summary>
            /// UIPanel的ID为新加ID，则为此ID添加注释
            /// </summary>
            public string m_IdComment;
            /// <summary>
            /// UIPanel的Depth
            /// </summary>
            public int m_Depth;
            /// <summary>
            /// UIPanel的文件夹名
            /// </summary>
            public string m_FolderName;
            /// <summary>
            /// UIPanel的级别
            /// </summary>
            public PanelLevel m_Level;
            /// <summary>
            /// 是否添加关闭按钮
            /// </summary>
            public bool m_IsAddCloseButton = true;
            /// <summary>
            /// 是否添加用来退出的背景
            /// </summary>
            public bool m_IsAddCloseBackground = true;
            /// <summary>
            /// 是否创建对应的Panel脚本
            /// </summary>
            public bool m_IsCreatePanelScript;
            /// <summary>
            /// 是否创建对应的Manager脚本
            /// </summary>
            public bool m_IsCreateManagerScript;
            /// <summary>
            /// UIPanel的预制体
            /// </summary>
            public GameObject m_Prefab;
            /// <summary>
            /// UIPanel的预制体文件路径
            /// </summary>
            public string m_PrefabPath;
            /// <summary>
            /// UIPanel的Panel脚本文件夹路径
            /// </summary>
            public string m_PanelScriptPath;
            /// <summary>
            /// UIPanel的Manager脚本文件夹路径
            /// </summary>
            public string m_ManagerScriptPath;
            /// <summary>
            /// UIPanel的锚点
            /// </summary>
            public int m_Side;
            /// <summary>
            /// 是否创建了Prefab
            /// </summary>
            public bool m_IsPrefabCreated;
            /// <summary>
            /// 是否在窗口中折叠了起来
            /// </summary>
            public bool m_IsFoldedup;
            /// <summary>
            /// 复选框是否可以更改
            /// </summary>
            public bool m_IsCheckboxCanChange;
            /// <summary>
            /// 复选框是否被选择
            /// </summary>
            public bool m_IsCheckboxSelected;

            [Flags]
            public enum AnchorSide
            {
                Nothing = 0,
                TopLeft = 1 << 0,
                Top = 1 << 1,
                TopRight = 1 << 2,
                Left = 1 << 3,
                Center = 1 << 4,
                Right = 1 << 5,
                BottomLeft = 1 << 6,
                Bottom = 1 << 7,
                BottomRight = 1 << 8,
                Everything = 1 << 9 - 1
            }

            public enum PanelLevel
            {
                一级面板,
                二级面板
            }
            #endregion

            /// <summary>
            /// EditorWindow面板中的序号
            /// </summary>
            [HideInInspector] public int m_SortId;
            /// <summary>
            /// 储存每个锚点是否被选择的信息
            /// </summary>
            [HideInInspector] public List<bool> m_IsSelectedList = new List<bool>();
            /// <summary>
            /// 锚点在图表中的位置
            /// </summary>
            [HideInInspector]
            public List<Vector3Int> m_AnchorPositions = new List<Vector3Int>()
            {
                new Vector3Int(-1,  1,  0),
                new Vector3Int( 0,  1,  0),
                new Vector3Int( 1,  1,  0),
                new Vector3Int(-1,  0,  0),
                new Vector3Int( 0,  0,  0),
                new Vector3Int( 1,  0,  0),
                new Vector3Int(-1, -1,  0),
                new Vector3Int( 0, -1,  0),
                new Vector3Int( 1, -1,  0),
            };
            /// <summary>
            /// 用字典存储管理每个位置与其是否被选择的信息
            /// </summary>
            /// <returns></returns>
            public Dictionary<Vector3Int, bool> GetAnchors()
            {
                Dictionary<Vector3Int, bool> dict = new Dictionary<Vector3Int, bool>();

                for ( int i = 0; i < m_IsSelectedList.Count && i < m_AnchorPositions.Count; i++ )
                    dict.Add( m_AnchorPositions[ i ], m_IsSelectedList[ i ] );

                return dict;
            }

            public void UpdateListFromDictionary( Dictionary<Vector3Int, bool> dict )
            {
                m_IsSelectedList = dict.Values.ToList();
                m_Side = 0;
                for ( int i = 0; i < m_IsSelectedList.Count; i++ )
                {
                    if ( m_IsSelectedList[ i ] )
                        m_Side += ( int )Mathf.Pow( 2, i );
                }
            }
        }
    }
}