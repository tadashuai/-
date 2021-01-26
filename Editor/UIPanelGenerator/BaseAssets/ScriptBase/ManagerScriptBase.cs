using Common;
using Goods;
using LocaleX;
using Network;
using Network.Messages;
using System;
using System.Collections.Generic;

public class ManagerScriptBase : IManagerInit
{
	private NetMessageReg m_msgReg = null;

	public void Initialize()
	{
		m_msgReg = new NetMessageReg(

			);
	}

	public void Uninitialize()
	{
		if ( m_msgReg != null )
		{
			m_msgReg.Dispose();
			m_msgReg = null;
		}

		UninitializeData();
	}

	public void UninitializeData()
	{

	}


	#region SendMessage

	#endregion

	#region OnNetMessage

	#endregion
}
