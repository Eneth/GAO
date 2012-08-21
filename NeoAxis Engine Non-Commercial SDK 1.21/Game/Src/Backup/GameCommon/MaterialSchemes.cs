// Copyright (C) 2006-2012 NeoAxis Group Ltd.
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCommon
{
	public enum MaterialSchemes
	{
		//no normal mapping, no receiving shadows, no specular.
		//Game: HeightmapTerrain will use SimpleRendering mode for this scheme.
		//Game: used for generation WaterPlane reflection.
		Low,

		//High. Maximum quality.
		//Resource Editor and Map Editor uses "Default" scheme by default.
		//Note! Need save "Default" scheme in this enumeration. Without that scheme possible to get side effects.
		Default
	}
}
