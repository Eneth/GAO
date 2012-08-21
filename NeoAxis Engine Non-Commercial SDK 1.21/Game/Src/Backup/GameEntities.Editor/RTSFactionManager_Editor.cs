// Copyright (C) 2006-2012 NeoAxis Group Ltd.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using Engine;
using Engine.EntitySystem;
using Engine.Utils;

namespace GameEntities.Editor
{
	public class RTSFactionManager_FactionsCollectionEditor : GameEntitiesGeneralListCollectionEditor 
	{
		public RTSFactionManager_FactionsCollectionEditor ()
			: base( typeof( List<RTSFactionManager.FactionItem> ) )
		{ }
	}
}
