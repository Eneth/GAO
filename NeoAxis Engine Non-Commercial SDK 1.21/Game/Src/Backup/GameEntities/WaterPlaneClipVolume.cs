// Copyright (C) 2006-2012 NeoAxis Group Ltd.
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Engine;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.PhysicsSystem;
using Engine.Renderer;
using Engine.MathEx;
using Engine.Utils;


namespace GameEntities
{
	/// <summary>
	/// Defines the <see cref="WaterPlaneClipVolume"/> entity type.
	/// </summary>
	public class WaterPlaneClipVolumeType : MapObjectType
	{
	}

	/// <summary>
	/// Addition class for WaterPlane.
	/// By this class is possible to disable reflections for objects inside specified volume.
	/// </summary>
	public class WaterPlaneClipVolume : MapObject
	{
		static List<WaterPlaneClipVolume> instances = new List<WaterPlaneClipVolume>();

		///////////////////////////////////////////

		WaterPlaneClipVolumeType _type = null; public new WaterPlaneClipVolumeType Type { get { return _type; } }

		[Browsable( false )]
		public static List<WaterPlaneClipVolume> Instances
		{
			get { return instances; }
		}

		protected override void OnPostCreate( bool loaded )
		{
			if( !instances.Contains( this ) )
				instances.Add( this );
			base.OnPostCreate( loaded );
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			instances.Remove( this );
		}

		protected override void OnCalculateMapBounds( ref Bounds bounds )
		{
			base.OnCalculateMapBounds( ref bounds );
			bounds = GetBox().ToBounds();
		}

		/// <summary>Overridden from <see cref="Engine.MapSystem.MapObject.OnRender(Camera)"/>.</summary>
		protected override void OnRender( Camera camera )
		{
			base.OnRender( camera );

			if( EntitySystemWorld.Instance.IsEditor() && EditorLayer.Visible ||
				EngineDebugSettings.DrawGameSpecificDebugGeometry )
			{
				if( camera.Purpose == Camera.Purposes.MainCamera )
				{
					camera.DebugGeometry.Color = new ColorValue( 1, 0, 0 );
					camera.DebugGeometry.AddBox( GetBox() );
				}
			}
		}

		protected override bool OnGetEditorSelectionByRay( Ray ray, out Vec3 pos, ref float priority )
		{
			if( GetBox().IsContainsPoint( ray.Origin ) )
			{
				pos = Vec3.Zero;
				return false;
			}

			float scale1, scale2;
			bool ret = GetBox().RayIntersection( ray, out scale1, out scale2 );
			if( ret )
				pos = ray.GetPointOnRay( Math.Min( scale1, scale2 ) );
			else
				pos = Vec3.Zero;
			return ret;
		}

		protected override void OnEditorSelectionDebugRender( Camera camera, bool bigBorder,
			bool simpleGeometry )
		{
			Box box = GetBox();
			box.Expand( bigBorder ? .2f : .1f );
			camera.DebugGeometry.AddBox( box );
		}

	}
}
