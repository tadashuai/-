using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace UIPanelGenerator
{
    public class CreateUIPanel : EditorWindow
    {
        #region Texture Base64
        private const string s_NotSelected = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABoSURBVDhPnY3BDcAgDAOZhS14dP1O0x2C/LBEgiNSHvfwyZabmV0jZRUpq2zi6f0DJwdcQOEdwwDLypF0zHLMa9+NQRxkQ+ACOT2STVw/q8eY1346ZlE54sYAhVhSDrjwFymrSFnD2gTZpls2OvFUHAAAAABJRU5ErkJggg==";
        private const string s_Selected = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAvUlEQVQokWMYwuD///9E4Sdf7vyfcCXvP4gNB8RofPP92X+3Hdz/TTcx/H/85dZ/ojXDNDpsY/l/7f1JTJu3PJr7/9vvL/+J0Yii+dHnm/8NNjD8D9wj/Z8YjRg2b30097/9Vma4Ae9/vMKpEaaZEaYZBLY9nv+/63IqgxKvDsOn3+8Y3v58zjDN8iiDpoAZI3paYmRkxAywrY/m/XfcxvrfcRsbVhuxOhsZr7rX///06904NaJoHmKAgYEBAPXfQeZCL9VyAAAAAElFTkSuQmCC";
        private const string s_NoneElement = "/9j/4AAQSkZJRgABAQEASABIAAD/2wBDABsSFBcUERsXFhceHBsgKEIrKCUlKFE6PTBCYFVlZF9VXVtqeJmBanGQc1tdhbWGkJ6jq62rZ4C8ybqmx5moq6T/2wBDARweHigjKE4rK06kbl1upKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKSkpKT/wAARCACLAIcDASIAAhEBAxEB/8QAGgAAAgMBAQAAAAAAAAAAAAAAAgMAAQQFBv/EAC0QAAIBAgQEBgIDAQEAAAAAAAABAgMRBBIhMRNBUVIFIkJxgZEyYRQjMxVi/8QAFwEBAQEBAAAAAAAAAAAAAAAAAAECA//EABgRAQEBAQEAAAAAAAAAAAAAAAABERIC/9oADAMBAAIRAxEAPwDqkF8WHci+JDuGKIgPEh3InEh3L7ICIDxIdy+ycSHcvsAyAcSHcvsnEh3L7AIgPEj3L7JxI9yGAiA8SHcicSHcgCIBxY9yJxYdyAMgHFh3IriR7kAwgviR7kQDm5yZxdyXOgZnJnFXJcgapt6EzNC4zcWmnqjZSnGurTSzewGdTfUmdmv+JCWusS44SnHV+YnQx5mTMzbwab1yoqWGhLbQdQZMz6lZn1NDwsltqA6WX85KJdgWm2S7JOtSp6K8jRhkq0c+WyAz+YmpPEsROjJQo5U7XuzmTxeKesptewHS1Ic+PiGKpr81L3RANOYlwLkuUFclwbkAO4/CyfFUVzMqZv8AC6d5yqP07GaOg1ZasXLT5E4qVeE3KM049rFyqzlFSatoc2jnKzsRyZjjiptt06TnyvyDjWrS3p2+QDqYhqNm7GCVVqTUmzZWpqavfUw1qM76FC9ZzsdCFVxiknojJTjl9w7m5GaT4lLNUjK/IzRk7a6oPFNOdrmZya0Kg5wg35ZNEFORAjoFg3JcrQtS0wLlpgGlc62Ajkw977nJppzmkdqlTcKCi2Y9VYqSUpXlscriVqmJdPN5U9Toyu1a5keHcJOUXq9zCmU1GneMVZFydxaut0WmAMpNASlcKbQmTLALepVwbgVKqprfU6ITin/YZpNjJ123sC87V8unsGSyFqDeyIQb7kKIaUVyFJNuyTfsOhha0mrU5fRNGjwyg6tbNbSJ2ZRWWwnA4X+PS38zWo2d1qmYqstSLTEzb6GmWae0RFWFSPpMqQ2+gLk+SClnvsgHKXQAW29wLX0Db6i5aFgTWbpRbfwYJScm23c7Eqca9Fq2pyqtJwk10NM0laM0VL3UZPlewEaEpDakpwko5k/LZvmVAKooxsrogE1aVk7kA9C/DaD2Uo/IH/OUV5Wm/wD0dHQmhjW2fDU5Uo5Zxh7xRoi1cqxcVqZVdStFLTcyVK7TTu99jRUwsJ62af6Zmn4dGU82eaa/ZUbFOMY2TE1aqatcXLDVfTV+0Klh8StnBgVNc7iJ3TG8DE81H7AlRr9ifyAtyuC7MudKrzptC02m09LFDKcssrCcbDzKf2FfVMZXSnT1ZWWaNlFyetjLOV25P4Cr1bvKtkIbNIpt3IUmQD2lirFSrU1vNfYmWNpR53OTofqRGOXiEfTC4qXiE3+MUgOqmRyitW0cWWLrS9dvYVKpOW8myjtTxNGO9SP2Jnj6EdpOXwchspsDoy8Rj6YMTLHye0UjHcFsIfPFVZeoTdt3KuWigojG7wa/QtEzFRzal1UaKYddf2ti2ysoQhCjr5m+ZV2yiNpbs5ui7lXLSfQlmXEUU2lzKkn1EuCg82rfLUvIc5JASn0KkwCYaLM+pLghAEggUFcC7gtlNgylZBGXEP8AsYkKq7zbKRplVyB5G1oiFGieKk/xVhTnOcknLdiwqX+sfdGWnXjokiNEiWaiFyiIr3VO65GliMR/myhCrxlpzJmM0vyNVNLKtDKpctSQMtwSB2a3Mpz/AGLKZFMzgzldWB5FFQmcXEEZVFmkWm1syFEA/9k=";
        #endregion

        private static Texture2D[] s_IsSelectedIcons;
        public static Texture2D[] isSelectedIcons
        {
            get
            {
                if ( s_IsSelectedIcons == null )
                {
                    s_IsSelectedIcons = new Texture2D[ 2 ];
                    s_IsSelectedIcons[ 0 ] = Base64ToTexture( s_NotSelected );
                    s_IsSelectedIcons[ 1 ] = Base64ToTexture( s_Selected );
                }
                return s_IsSelectedIcons;
            }
        }

        private static Texture2D s_NoneElementIcon;
        public static Texture2D noneElementIcon
        {
            get
            {
                if ( s_NoneElementIcon == null )
                {
                    s_NoneElementIcon = Base64ToTexture( s_NoneElement );
                }
                return s_NoneElementIcon;
            }
        }

        public UIPanelBase m_PanelBase;
        public ReorderableList m_ReorderableList;
        public static string m_Prefab1Path = "Assets/Editor/UIPanelGenerator/BaseAssets/PrefabBase/UIPanelBase1.prefab";
        public static string m_Prefab2Path = "Assets/Editor/UIPanelGenerator/BaseAssets/PrefabBase/UIPanelBase2.prefab";
        public static string m_PanelScriptPath = "Assets/Editor/UIPanelGenerator/BaseAssets/ScriptBase/PanelScriptBase.cs";
        public static string m_ManagerScriptPath = "Assets/Editor/UIPanelGenerator/BaseAssets/ScriptBase/ManagerScriptBase.cs";
        public static string m_ProjectPath = null;

        private Vector2 scrollPosition;
        private bool isShowTips, isTheSameName, isShowCheckbox, isAllSelected;
        private int currentIndex, preIndex = -1;

        public const float k_DefaultElementHeight = 60f;    // 每个元素默认高(长)度
        public const float k_PaddingBetweenEelements = 26f; // 每个元素间的间距
        public const float k_SingleLineHeight = 16f;        // 每个属性单行高(长)度
        public const float k_LabelWidth = 80f;              // 属性名文字Label宽度

        [MenuItem( "UIPanelGenerator/CreateUIPanel" )]
        static void OpenWindow()
        {
            EditorWindow window = GetWindow<CreateUIPanel>();
            window.titleContent.text = "CreateUIPanel";
            window.minSize = new Vector2( 270, 360 );
            window.Show();
        }

        private void OnEnable()
        {
            m_ProjectPath = Directory.GetCurrentDirectory().Replace( "\\", "/" ) + "/";

            m_PanelBase = AssetDatabase.LoadAssetAtPath<UIPanelBase>( "Assets/Editor/UIPanelGenerator/PanelBase.asset" );
            if ( m_PanelBase != null )
            {
                m_PanelBase.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;

                m_ReorderableList = new ReorderableList( m_PanelBase.m_PanelBaseInfos, typeof( UIPanelBase.PanelBaseInfo ), true, true, true, true );

                m_ReorderableList.drawHeaderCallback = OnDrawHeader;
                m_ReorderableList.drawElementCallback = OnDrawElement;
                m_ReorderableList.onChangedCallback = ListUpdated;
                m_ReorderableList.onAddCallback = OnAddElement;
                m_ReorderableList.onRemoveCallback = OnRemoveElement;
                m_ReorderableList.drawNoneElementCallback = OnDrawNoneElement;
                m_ReorderableList.onMouseUpCallback = OnMouseUp;
                m_ReorderableList.elementHeight = k_DefaultElementHeight;
                m_ReorderableList.elementHeightCallback = GetElementHeight;
                //m_ReorderableList.drawElementBackgroundCallback = OnDrawElementBackGround;
            }
            else
            {
                Debug.LogError( "未找到文件！ Assets/Editor/UIPanelGenerator/PanelBase.asset" );
            }
        }

        private void OnDisable()
        {
            isShowTips = false;
            isTheSameName = false;
            isShowCheckbox = false;
            isAllSelected = false;
            currentIndex = -1;
            preIndex = -1;
            m_PanelBase.m_PanelBaseInfos.Clear();
            DestroyImmediate( isSelectedIcons[ 0 ] );
            DestroyImmediate( isSelectedIcons[ 1 ] );
            DestroyImmediate( s_IsSelectedIcons[ 0 ] );
            DestroyImmediate( s_IsSelectedIcons[ 1 ] );
            DestroyImmediate( noneElementIcon );
            DestroyImmediate( s_NoneElementIcon );
            AssetDatabase.Refresh();
        }

        private void OnDrawElementBackGround( Rect rect, int index, bool isActive, bool isFocused )
        {

        }

        public float GetElementHeight( int index )
        {
            UIPanelBase.PanelBaseInfo info = m_PanelBase.m_PanelBaseInfos[ index ];
            if ( !info.m_IsFoldedup )
            {
                float inspectorHeight = k_DefaultElementHeight + 6 * k_SingleLineHeight;
                if ( info.m_IsShowIdComment )
                    inspectorHeight += k_SingleLineHeight;
                if ( info.m_Level == UIPanelBase.PanelBaseInfo.PanelLevel.二级面板 )
                    inspectorHeight += 2 * k_SingleLineHeight;
                return Mathf.Max( inspectorHeight, k_DefaultElementHeight );
            }
            else
            {
                return k_SingleLineHeight + 4f;
            }
        }

        private void OnMouseUp( ReorderableList list )
        {
            if ( currentIndex != list.index )
            {
                preIndex = currentIndex;
                currentIndex = list.index;
            }

            if ( isShowCheckbox && m_PanelBase.m_PanelBaseInfos[ currentIndex ].m_IsCheckboxCanChange )
            {
                m_PanelBase.m_PanelBaseInfos[ currentIndex ].m_IsCheckboxSelected = !m_PanelBase.m_PanelBaseInfos[ currentIndex ].m_IsCheckboxSelected;
            }

            if ( preIndex != -1 && preIndex < m_PanelBase.m_PanelBaseInfos.Count )
                m_PanelBase.m_PanelBaseInfos[ preIndex ].m_IsCheckboxCanChange = false;
            m_PanelBase.m_PanelBaseInfos[ currentIndex ].m_IsCheckboxCanChange = true;
        }

        private void OnLocateSelected( object _info )
        {
            UIPanelBase.PanelBaseInfo info = _info as UIPanelBase.PanelBaseInfo;
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>( info.m_PrefabPath );
        }

        private void OnDuplicateSelected( object _info )
        {
            UIPanelBase.PanelBaseInfo info = _info as UIPanelBase.PanelBaseInfo;

            UIPanelBase.PanelBaseInfo newInfo = new UIPanelBase.PanelBaseInfo();
            newInfo.m_Name = info.m_Name;
            newInfo.m_Id = info.m_Id;
            newInfo.m_IdComment = info.m_IdComment;
            newInfo.m_IsShowIdComment = info.m_IsShowIdComment;
            newInfo.m_FolderName = info.m_FolderName;
            newInfo.m_Level = info.m_Level;
            newInfo.m_IsAddCloseButton = info.m_IsAddCloseButton;
            newInfo.m_IsAddCloseBackground = info.m_IsAddCloseBackground;
            newInfo.m_IsAddSuffix = info.m_IsAddSuffix;
            newInfo.m_IsCreatePanelScript = info.m_IsCreatePanelScript;
            newInfo.m_IsCreateManagerScript = info.m_IsCreateManagerScript;
            newInfo.m_Side = info.m_Side;
            newInfo.m_Depth = info.m_Depth;
            newInfo.m_SortId = info.m_SortId;
            newInfo.m_IsSelectedList = info.m_IsSelectedList;
            m_PanelBase.m_PanelBaseInfos.Add( newInfo );
        }

        private void OnDeleteSelected()
        {
            OnRemoveElement( m_ReorderableList );
        }

        private void OnDrawNoneElement( Rect rect )
        {
            Rect iconRect = new Rect( rect.xMax - rect.height + 2f, rect.yMin + 2f, rect.height - 4f, rect.height - 4f );
            Rect labelRect = new Rect( rect.xMin, rect.yMin + 2f, rect.width - iconRect.width, rect.height - 4f );

            GUILayout.BeginHorizontal();
            string tempTip = "\n当前面板列表为空\n\n点击右下角的加号即可添加新面板";
            GUI.Label( labelRect, tempTip );
            GUI.DrawTexture( iconRect, noneElementIcon );
            GUILayout.EndHorizontal();
        }

        private void OnRemoveElement( ReorderableList list )
        {
            string currentUIPanelName = m_PanelBase.m_PanelBaseInfos[ list.index ].m_Name;
            string message = string.Format( "是否确定删除 “ {0} ” 面板", currentUIPanelName );
            if ( EditorUtility.DisplayDialog( "警告", message, "确定", "取消" ) )
            {
                ReorderableList.defaultBehaviours.DoRemoveButton( list );
            }
        }

        private void ListUpdated( ReorderableList list )
        {
            for ( int i = 0; i < m_PanelBase.m_PanelBaseInfos.Count; i++ )
            {
                m_PanelBase.m_PanelBaseInfos[ i ].m_SortId = i;
            }
        }

        private void OnAddElement( ReorderableList list )
        {

            UIPanelBase.PanelBaseInfo info = new UIPanelBase.PanelBaseInfo();
            info.m_Name = string.Format( "UIPanel{0}", list.count );
            info.m_Id = 0;
            for ( int i = 0; i < 9; i++ )
            {
                info.m_IsSelectedList.Add( false );
            }
            m_PanelBase.m_PanelBaseInfos.Add( info );
        }

        private void OnDrawElement( Rect rect, int index, bool isActive, bool isFocused )
        {
            UIPanelBase.PanelBaseInfo info = m_PanelBase.m_PanelBaseInfos[ index ];

            float yPos = rect.yMin + 2f;
            float height = rect.height - k_PaddingBetweenEelements;
            Vector2 matrixSize = 3 * k_SingleLineHeight * Vector2.one;

            Rect headerRect = new Rect( rect.xMin, yPos, rect.width, k_SingleLineHeight );
            Rect matrixRect = new Rect( rect.xMax - matrixSize.x - 5f, yPos + k_SingleLineHeight, matrixSize.x, matrixSize.y );
            Rect inspectorRect = new Rect( rect.xMin, yPos + k_SingleLineHeight, rect.width - matrixRect.width - 15f, height );

            RuleHeaderOnGUI( headerRect, info );
            if ( !info.m_IsFoldedup )
            {
                RuleInspectorOnGUI( inspectorRect, info );
                if ( info.m_Level == UIPanelBase.PanelBaseInfo.PanelLevel.一级面板 )
                    RuleMatrixOnGUI( matrixRect, info );
            }
        }

        private void RuleHeaderOnGUI( Rect rect, UIPanelBase.PanelBaseInfo info )
        {
            if ( isShowCheckbox )
            {
                Rect toggleRect = new Rect( new Rect( rect.xMax - 2 * k_SingleLineHeight, rect.yMin - 3f, k_SingleLineHeight, k_SingleLineHeight ) );
                info.m_IsCheckboxSelected = EditorGUI.Toggle( toggleRect, info.m_IsCheckboxSelected );
                string tip = string.Format( "“ {0}{1} ” 面板复选框{2}被选择", info.m_Name, info.m_IsAddSuffix ? "Panel" : "", info.m_IsCheckboxSelected ? "已" : "未" );
                if ( toggleRect.Contains( Event.current.mousePosition ) )
                {
                    GUI.Label( toggleRect, new GUIContent( "", tip ) );
                }
            }

            Rect labelRect = new Rect( rect.xMin, rect.yMin, rect.width - k_SingleLineHeight, k_SingleLineHeight );
            GUI.Label( labelRect, info.m_Name + ( info.m_IsAddSuffix ? "Panel" : "" ), EditorStyles.boldLabel );
            if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 && labelRect.Contains( Event.current.mousePosition ) )
            {
                Event.current.Use();
                info.m_IsFoldedup = !info.m_IsFoldedup;
            }


            GUI.Label( new Rect( rect.xMax - k_SingleLineHeight, rect.yMin - 1f, k_SingleLineHeight + 10f, k_SingleLineHeight + 10f ), EditorGUIUtility.IconContent( "d__Popup" ) );
            Rect checkRect = new Rect( rect.xMax - k_SingleLineHeight, rect.yMin, k_SingleLineHeight, k_SingleLineHeight );
            if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 && checkRect.Contains( Event.current.mousePosition ) )
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem( new GUIContent( "Delete" ), false, OnDeleteSelected );
                menu.AddItem( new GUIContent( "Duplicate" ), false, OnDuplicateSelected, info );
                if ( info.m_IsPrefabCreated )
                    menu.AddItem( new GUIContent( "Locate Prefab" ), false, OnLocateSelected, info );
                menu.ShowAsContext();
            }
        }

        private void RuleMatrixOnGUI( Rect rect, UIPanelBase.PanelBaseInfo info )
        {
            Handles.color = EditorGUIUtility.isProSkin ? new Color( 1f, 1f, 1f, 0.2f ) : new Color( 0f, 0f, 0f, 0.2f );

            for ( int y = 0; y <= 3; y++ )
            {
                float dy = rect.yMin + y * k_SingleLineHeight;
                Handles.DrawLine( new Vector3( rect.xMin, dy ), new Vector3( rect.xMax, dy ) );
            }
            for ( int x = 0; x <= 3; x++ )
            {
                float dx = rect.xMin + x * k_SingleLineHeight;
                Handles.DrawLine( new Vector3( dx, rect.yMin ), new Vector3( dx, rect.yMax ) );
            }

            var anchors = info.GetAnchors();

            for ( int y = -1; y < 2; y++ )
            {
                for ( int x = -1; x < 2; x++ )
                {
                    Vector3Int pos = new Vector3Int( x, y, 0 );
                    Rect iconRect = new Rect( rect.xMin + ( x + 1 ) * k_SingleLineHeight + 1, rect.yMin + ( 1 - y ) * k_SingleLineHeight + 1, k_SingleLineHeight - 2, k_SingleLineHeight - 2 );
                    RuleMatrixIconOnGUI( info, anchors, pos, iconRect );
                }
            }
        }

        private void RuleMatrixIconOnGUI( UIPanelBase.PanelBaseInfo info, Dictionary<Vector3Int, bool> anchors, Vector3Int pos, Rect rect )
        {
            if ( anchors.ContainsKey( pos ) )
            {
                DrawAnchorOnGUI( rect, pos, anchors[ pos ] );
                TooltipOnGUI( rect, anchors[ pos ] );
                AnchorUpdate( rect, info, anchors, pos );
            }

        }

        private void AnchorUpdate( Rect rect, UIPanelBase.PanelBaseInfo info, Dictionary<Vector3Int, bool> anchors, Vector3Int position )
        {
            if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains( Event.current.mousePosition ) )
            {
                if ( anchors.ContainsKey( position ) )
                {
                    anchors[ position ] = !anchors[ position ];
                }

                info.UpdateListFromDictionary( anchors );
            }
        }

        private void TooltipOnGUI( Rect rect, bool anchorIsSelected )
        {
            GUI.Label( rect, new GUIContent( "", anchorIsSelected.ToString() ) );
        }

        private void DrawAnchorOnGUI( Rect rect, Vector3Int position, bool anchorIsSelected )
        {
            switch ( anchorIsSelected )
            {
                case true:
                    GUI.DrawTexture( rect, isSelectedIcons[ 1 ] );
                    break;
                case false:
                    GUI.DrawTexture( rect, isSelectedIcons[ 0 ] );
                    break;
            }
        }

        public void RuleInspectorOnGUI( Rect rect, UIPanelBase.PanelBaseInfo info )
        {
            if ( info.m_IsPrefabCreated )
            {
                GUIStyle style = new GUIStyle();
                style.normal.background = null;
                style.normal.textColor = Color.red;
                style.fontSize = 12;
                GUI.Label( new Rect( rect.xMin - 14f, rect.yMin, 12f, 48f ), "预\n制\n体\n创\n建\n成\n功", style );
            }

            Handles.color = EditorGUIUtility.isProSkin ? new Color( 1f, 1f, 1f, 0.6f ) : new Color( 0f, 0f, 0f, 0.6f );

            float y = rect.yMin;
            GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), "Panel Name" );
            info.m_Name = EditorGUI.TextField( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth - k_SingleLineHeight, k_SingleLineHeight ), info.m_Name );
            info.m_IsAddSuffix = EditorGUI.Toggle( new Rect( rect.xMin + rect.width - k_SingleLineHeight + 1f, y - 2f, k_SingleLineHeight - 4f, k_SingleLineHeight - 4f ), info.m_IsAddSuffix );
            GUI.Label( new Rect( rect.xMin + rect.width - k_SingleLineHeight + 1f, y - 2f, k_SingleLineHeight - 4f, k_SingleLineHeight - 4f ), new GUIContent( "", "是否需要后台自动为文件添加后缀\n注意：并不给文件夹添加后缀！" ) );
            y += k_SingleLineHeight;

            info.m_Id = EditorGUI.IntField( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight ), info.m_Id );
            if ( !Enum.IsDefined( typeof( TPanelType ), ( TPanelType )info.m_Id ) )
            {
                info.m_IsShowIdComment = EditorGUI.Foldout( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), info.m_IsShowIdComment, "Panel ID" );
                if ( info.m_IsShowIdComment )
                {
                    y += k_SingleLineHeight;
                    GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), new GUIContent( "新ID功能注释", "为上面的新ID作功能注释" ) );
                    info.m_IdComment = EditorGUI.TextField( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight ), info.m_IdComment );
                }
            }
            else
            {
                GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), "Panel ID" );
            }
            y += k_SingleLineHeight;

            GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), "Panel Depth" );
            info.m_Depth = EditorGUI.IntField( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight ), info.m_Depth );
            y += k_SingleLineHeight;

            GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), "Panel Folder" );
            if ( GUI.Button( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight ), "选择文件夹" ) )
            {
                string path = EditorUtility.SaveFolderPanel( "选择文件夹", m_ProjectPath + "Assets/Resources/UI/NewPanel", "XXXPanel" );
                if ( path.Length != 0 )
                {
                    info.m_FolderName = path.Replace( m_ProjectPath, "" );
                }
            }

            y += k_SingleLineHeight;

            EditorGUI.TextField( new Rect( rect.xMin, y, rect.width, k_SingleLineHeight ), info.m_FolderName );
            y += k_SingleLineHeight;

            GUI.Label( new Rect( rect.xMin, y, k_LabelWidth, k_SingleLineHeight ), "Panel Level" );
            info.m_Level = ( UIPanelBase.PanelBaseInfo.PanelLevel )EditorGUI.EnumPopup( new Rect( rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight ), info.m_Level, "ExposablePopupMenu" );
            y += k_SingleLineHeight;

            if ( info.m_Level == UIPanelBase.PanelBaseInfo.PanelLevel.二级面板 )
            {
                info.m_IsAddCloseButton = EditorGUI.ToggleLeft( new Rect( rect.xMin + k_SingleLineHeight / 2, y, rect.width - k_SingleLineHeight / 2, k_SingleLineHeight ), "是否添加Close Button", info.m_IsAddCloseButton );
                if ( new Rect( rect.xMin + k_SingleLineHeight / 2, y, rect.width - k_SingleLineHeight / 2, k_SingleLineHeight ).Contains( Event.current.mousePosition ) )
                {
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>( "Assets/Editor/UIPanelGenerator/Textures/close_2.png" );
                    GUI.DrawTexture( new Rect( rect.xMax + 10f, y - 2 * k_SingleLineHeight, 48f, 96f ), texture );
                    Handles.DrawLine( new Vector3( rect.xMax + 10f, y - 2 * k_SingleLineHeight, 0 ), new Vector3( rect.xMax + 58f, y - 2 * k_SingleLineHeight, 0 ) );
                    Handles.DrawLine( new Vector3( rect.xMax + 58f, y - 2 * k_SingleLineHeight, 0 ), new Vector3( rect.xMax + 58f, y - 2 * k_SingleLineHeight + 96f, 0 ) );
                    Handles.DrawLine( new Vector3( rect.xMax + 58f, y - 2 * k_SingleLineHeight + 96f, 0 ), new Vector3( rect.xMax + 10f, y - 2 * k_SingleLineHeight + 96f, 0 ) );
                    Handles.DrawLine( new Vector3( rect.xMax + 10f, y - 2 * k_SingleLineHeight + 96f, 0 ), new Vector3( rect.xMax + 10f, y - 2 * k_SingleLineHeight, 0 ) );
                }
                y += k_SingleLineHeight;

                info.m_IsAddCloseBackground = EditorGUI.ToggleLeft( new Rect( rect.xMin + k_SingleLineHeight / 2, y, rect.width - k_SingleLineHeight / 2, k_SingleLineHeight ), "是否添加Close BackGround", info.m_IsAddCloseBackground );
                y += k_SingleLineHeight;

                Handles.DrawLine( new Vector3( rect.xMin, y - 3 * k_SingleLineHeight, 0 ), new Vector3( rect.xMin, y, 0 ) );
                Handles.DrawLine( new Vector3( rect.xMin, y, 0 ), new Vector3( rect.xMax, y, 0 ) );
                Handles.DrawLine( new Vector3( rect.xMax, y, 0 ), new Vector3( rect.xMax, y - 3 * k_SingleLineHeight, 0 ) );
                Handles.DrawLine( new Vector3( rect.xMin, y - 3 * k_SingleLineHeight, 0 ), new Vector3( rect.xMax, y - 3 * k_SingleLineHeight, 0 ) );
            }

            info.m_IsCreatePanelScript = EditorGUI.ToggleLeft( new Rect( rect.xMin, y, rect.width, k_SingleLineHeight ), "是否创建Panel脚本", info.m_IsCreatePanelScript );
            y += k_SingleLineHeight;

            info.m_IsCreateManagerScript = EditorGUI.ToggleLeft( new Rect( rect.xMin, y, rect.width, k_SingleLineHeight ), "是否创建Manager脚本", info.m_IsCreateManagerScript );
            y += k_SingleLineHeight;


        }

        private void OnDrawHeader( Rect rect )
        {
            Rect labelRect = new Rect( rect.xMin, rect.yMin, rect.width - rect.height, rect.height );
            GUI.Label( labelRect, "UI Panels" );

            GUI.Label( new Rect( rect.xMax - rect.height, rect.yMin + 1f, rect.height + 10f, rect.height + 10f ), EditorGUIUtility.IconContent( "d__Popup" ) );
            Rect checkRect = new Rect( rect.xMax - rect.height, rect.yMin, rect.height, rect.height );
            if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 && checkRect.Contains( Event.current.mousePosition ) )
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem( new GUIContent( "Delete All" ), false, OnDeleteAll );
                menu.AddItem( new GUIContent( "Show Checkbox" ), isShowCheckbox, OnShowCheckbox );
                menu.ShowAsContext();
            }

            Rect toggleRect = new Rect( new Rect( rect.xMax - 2 * rect.height, rect.yMin - 1f, rect.height, rect.height ) );
            if ( isShowCheckbox )
            {
                isAllSelected = EditorGUI.Toggle( toggleRect, isAllSelected );
                if ( GUI.changed )
                {
                    ChangeAllCheckboxes( isAllSelected );
                }
                if ( isAllSelected )
                {
                    for ( int i = 0; i < m_PanelBase.m_PanelBaseInfos.Count; i++ )
                    {
                        if ( !m_PanelBase.m_PanelBaseInfos[ i ].m_IsCheckboxSelected )
                        {
                            isAllSelected = false;
                            break;
                        }
                    }
                }
            }
        }

        private void ChangeAllCheckboxes( bool value )
        {
            for ( int i = 0; i < m_PanelBase.m_PanelBaseInfos.Count; i++ )
            {
                m_PanelBase.m_PanelBaseInfos[ i ].m_IsCheckboxSelected = value;
            }
        }

        private void OnShowCheckbox()
        {
            isShowCheckbox = !isShowCheckbox;
            if ( !isShowCheckbox )
                ChangeAllCheckboxes( false );
        }

        private void OnDeleteAll()
        {
            if ( m_ReorderableList.count > 0 )
            {
                if ( EditorUtility.DisplayDialog( "警告", "是否清空列表", "确定", "取消" ) )
                {
                    m_ReorderableList.list.Clear();
                }
            }
            else
            {
                EditorUtility.DisplayDialog( "提示", "列表已经为空", "确定" );
            }

        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, false );
            GUILayout.BeginHorizontal();
            GUILayout.Space( 10 );
            GUILayout.BeginVertical();
            GUILayout.Space( 10 );

            if ( m_ReorderableList != null )
            {
                m_ReorderableList.DoLayoutList();
            }

            GUILayout.EndVertical();
            GUILayout.Space( 10 );
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.Space( 10 );
            GUILayout.BeginVertical();

            if ( isShowTips )
            {
                EditorGUILayout.HelpBox( "上方UIPanel列表为空", MessageType.Info );
            }

            if ( GUILayout.Button( "创 建 如 上 UIPanel", "LargeButton" ) )
            {
                isShowTips = m_ReorderableList.count == 0;

                if ( m_ReorderableList.count > 0 )
                {
                    CreateUIPanels();
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space( 10 );
            GUILayout.EndHorizontal();
            GUILayout.Space( 10 );
        }

        private void CreateUIPanels()
        {
            for ( int i = 0; i < m_ReorderableList.count; i++ )
            {
                UIPanelBase.PanelBaseInfo info = m_PanelBase.m_PanelBaseInfos[ i ];

                if ( info.m_Name != null && info.m_Name != "" )
                {
                    if ( IsOnlyLetterOrNumber( info.m_Name ) )
                    {
                        CreateSinglePanel( info );
                    }
                    else
                    {
                        Debug.LogErrorFormat( "面板 “ {0} ” 名称包含非法字符！", info.m_Name );
                        continue;
                    }
                }
                else
                {
                    Debug.LogErrorFormat( "面板 “ {0} ” 名称为空！", info.m_Name );
                    continue;
                }
            }
        }

        private void CreateSinglePanel( UIPanelBase.PanelBaseInfo info )
        {
            GameObject uiPanelBaseGameObject = null;

            switch ( info.m_Level )
            {
                case UIPanelBase.PanelBaseInfo.PanelLevel.一级面板:
                    uiPanelBaseGameObject = InstantiateAndAssign( info, m_Prefab1Path );
                    UIAnchor[] anchors = uiPanelBaseGameObject.GetComponentsInChildren<UIAnchor>();
                    for ( int i = 0; i < info.m_IsSelectedList.Count; i++ )
                    {
                        if ( !info.m_IsSelectedList[ i ] )
                        {
                            DestroyImmediate( anchors[ i ].gameObject );
                        }
                    }
                    if ( info.m_Side == 16 )
                    {
                        anchors[ 4 ].gameObject.name = "Anchor";
                    }
                    break;

                case UIPanelBase.PanelBaseInfo.PanelLevel.二级面板:
                    uiPanelBaseGameObject = InstantiateAndAssign( info, m_Prefab2Path );
                    UISprite[] sprites = uiPanelBaseGameObject.GetComponentsInChildren<UISprite>();
                    if ( !info.m_IsAddCloseButton )
                        DestroyImmediate( sprites[ 0 ].gameObject );
                    if ( !info.m_IsAddCloseBackground )
                        DestroyImmediate( sprites[ 1 ].gameObject );
                    break;
            }

            CreateUIPanelPrefabs( uiPanelBaseGameObject, info );
        }

        private GameObject InstantiateAndAssign( UIPanelBase.PanelBaseInfo info, string prefabPath )
        {
            GameObject uiPanelBaseGameObject = AssetDatabase.LoadAssetAtPath<GameObject>( prefabPath );
            uiPanelBaseGameObject = Instantiate( uiPanelBaseGameObject );
            uiPanelBaseGameObject.name = info.m_Name;
            uiPanelBaseGameObject.GetComponent<UIPanel>().depth = info.m_Depth;
            return uiPanelBaseGameObject;
        }

        private void CreateUIPanelPrefabs( GameObject go, UIPanelBase.PanelBaseInfo info )
        {
            string fatherFolderPath = "Assets/Resources/UI/NewPanel";
            string filePath = CheckFolder( fatherFolderPath, info, "Panel", "prefab" );

            if ( !isTheSameName )
            {
                GameObject createObj = PrefabUtility.SaveAsPrefabAsset( go, filePath );
                info.m_Prefab = createObj;
                info.m_PrefabPath = filePath;

                CreatePanelScript( info );
                CreateManagerScript( info );
                info.m_IsPrefabCreated = true;
            }
            else
            {
                Debug.LogErrorFormat( "面板 “ {0} ” 名称与已创面板同名！", info.m_Name );
            }

            isTheSameName = false;
            DestroyImmediate( go );
        }

        private void CreateManagerScript( UIPanelBase.PanelBaseInfo info )
        {
            if ( !info.m_IsCreateManagerScript )
                return;

            string fatherFolderPath = "Assets/Code/Modules/Logic";
            string filePath = CheckFolder( fatherFolderPath, info, "Manager" );
            info.m_ManagerScriptPath = filePath;

            string tempContent = File.ReadAllText( m_ManagerScriptPath );
            tempContent = tempContent.Replace( "ManagerScriptBase", info.m_Name + "Manager" );
            File.WriteAllText( filePath, tempContent, System.Text.Encoding.UTF8 );
        }

        private void CreatePanelScript( UIPanelBase.PanelBaseInfo info )
        {
            if ( !info.m_IsCreatePanelScript )
                return;

            string fatherFolderPath = "Assets/Code/Modules/Panel";
            string filePath = CheckFolder( fatherFolderPath, info );
            info.m_PanelScriptPath = filePath;

            string tempContent = File.ReadAllText( m_PanelScriptPath );
            tempContent = tempContent.Replace( "PanelScriptBase", info.m_Name + ( info.m_IsAddSuffix ? "Panel" : "" ) );
            string folderName = info.m_FolderName;
            tempContent = tempContent.Replace( "ScriptPath", string.Format( "{0}/{1}", folderName.Replace( "Assets/Resources/", "" ), info.m_Name + ( info.m_IsAddSuffix ? "Panel" : "" ) ) );
            tempContent = tempContent.Replace( "Undefine", !Enum.IsDefined( typeof( TPanelType ), ( TPanelType ) info.m_Id ) ? info.m_Name : (( TPanelType ) info.m_Id).ToString() );
            if ( info.m_Level == UIPanelBase.PanelBaseInfo.PanelLevel.二级面板 && info.m_IsAddCloseButton )
                tempContent = tempContent.Replace( "//CloseButton", "UIEventListener.Get( transform.Find( \"Anchor/Offset/CloseButton\" ).gameObject ).onClick = _go => \n\t\t{\n\t\t\tClose();\n\t\t};" );
            File.WriteAllText( filePath, tempContent, System.Text.Encoding.UTF8 );
        }

        private string CheckFolder( string fatherFolder, UIPanelBase.PanelBaseInfo info, string nameSuffix = "Panel", string fileSuffix = "cs" )
        {
            string folderName = info.m_FolderName;
            folderName = folderName.Replace( "Assets/Resources/UI/NewPanel/", "" );
            if ( nameSuffix == "Manager" )
                folderName = folderName.Replace( "Panel", "" ) + nameSuffix;

            if ( !AssetDatabase.IsValidFolder( fatherFolder + "/" + folderName ) )
                AssetDatabase.CreateFolder( fatherFolder, folderName );

            DirectoryInfo dir = new DirectoryInfo( fatherFolder + "/" + folderName + "/" );
            FileInfo[] files = dir.GetFiles( "*." + fileSuffix );
            string temp = info.m_Name;
            if ( nameSuffix == "Manager" )
                temp = temp.Replace( "Panel", "" ) + nameSuffix;
            else
                temp += info.m_IsAddSuffix ? nameSuffix : "";

            string fileName = string.Format( "{0}.{1}", temp, fileSuffix );
            for ( int i = 0; i < files.Length; i++ )
            {
                if ( files[ i ].Name == fileName )
                {
                    isTheSameName = true;
                    break;
                }
            }

            return string.Format( "{0}/{1}/{2}", fatherFolder, folderName, fileName );
        }

        private bool IsOnlyLetterOrNumber( string str )
        {
            string pattern = @"^[a-zA-Z0-9]*$";

            return System.Text.RegularExpressions.Regex.IsMatch( str, pattern );
        }

        public static Texture2D Base64ToTexture( string base64 )
        {
            Texture2D t = new Texture2D( 1, 1 );
            t.hideFlags = HideFlags.HideAndDontSave;
            t.LoadImage( Convert.FromBase64String( base64 ) );
            return t;
        }
    }
}