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
	public class WaterPlaneType_SplashesCollectionEditor : GameEntitiesGeneralListCollectionEditor 
	{
		public WaterPlaneType_SplashesCollectionEditor()
			: base( typeof( List<WaterPlaneType.SplashItem> ) )
		{ }
	}

	public class WaterPlaneTypeSplashItem_ParticlesCollectionEditor: GameEntitiesGeneralListCollectionEditor
	{
		public WaterPlaneTypeSplashItem_ParticlesCollectionEditor()
			: base( typeof( List<WaterPlaneType.SplashItem.ParticleItem> ) )
		{ }
	}
}
