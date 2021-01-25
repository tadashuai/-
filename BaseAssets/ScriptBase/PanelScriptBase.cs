using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using LocaleX;
using System;

public class PanelScriptBase : UIScene
{

    #region member
    const string m_panelPath = "ScriptPath";
    static TPanelType m_page = TPanelType.Undefine;
    #endregion

    #region open
    public PanelScriptBase() : base( TPanelType.Undefine)
    {

    }

    public static PanelScriptBase TryGet()
    {
        return GameCenter.uiManger.FindUISceneByType<PanelScriptBase>();
    }

    public static PanelScriptBase Open(int subPage = 0)
    {
        if ( subPage == 0 )
        {
            m_page = TPanelType.Undefine;
        }
        else
        {
            m_page = ( TPanelType ) subPage;
        }

        PanelScriptBase panel = TryGet();
        if ( panel == null )
        {
            panel = GameCenter.uiManger.OpenScene<PanelScriptBase>( m_panelPath, true );
        }
        return panel;
    }

    public static void Close()
    {
        PanelScriptBase panel = TryGet();
        if ( panel != null )
        {
            GameCenter.uiManger.CloseScene( panel );
        }
    }
    #endregion

    #region inital
    protected override void OnStart()
    {
        //CloseButton
        AddGameMsg();
    }

    protected override void OnPreDestroy()
    {
        DeleteGameMsg();
    }

    private void AddGameMsg()
    {

    }

    private void DeleteGameMsg()
    {
        
    }

    private void InitalizeData()
    {

    }
    #endregion
}
