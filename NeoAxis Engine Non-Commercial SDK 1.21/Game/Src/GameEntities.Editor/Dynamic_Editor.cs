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
	public class DynamicType_AutomaticInfluencesCollectionEditor : GameEntitiesGeneralListCollectionEditor
	{
		public DynamicType_AutomaticInfluencesCollectionEditor()
			: base( typeof( List<DynamicType.AutomaticInfluenceItem> ) )
		{ }
	}
}
