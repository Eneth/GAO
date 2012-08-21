// Copyright (C) 2006-2012 NeoAxis Group Ltd.
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Design;
using Engine.EntitySystem;
using Engine.PhysicsSystem;
using Engine.MapSystem;
using Engine.MathEx;
using Engine.Renderer;
using Engine.Utils;
using GameCommon;

namespace GameEntities
{
	/// <summary>
	/// Defines the <see cref="GameMap"/> entity type.
	/// </summary>
	[AllowToCreateTypeBasedOnThisClass( false )]
	public class GameMapType : MapType
	{
	}

	public class GameMap : Map
	{
		static GameMap instance;

		[FieldSerialize]
		[DefaultValue( GameMap.GameTypes.Action )]
		GameTypes gameType = GameTypes.Action;

		[FieldSerialize]
		string gameMusic = "Sounds\\Music\\Game.ogg";

		//Wind settings
		[FieldSerialize]
		Radian windDirection;
		[FieldSerialize]
		float windSpeed = 1;

		[FieldSerialize]
		UnitType playerUnitType;

		///////////////////////////////////////////

		public enum GameTypes
		{
			None,
			Action,
			RTS,
			TPSArcade,
			TurretDemo,
			JigsawPuzzleGame,
			BallGame,
			VillageDemo,

			//Put here your game type.
		}

		///////////////////////////////////////////

		enum NetworkMessages
		{
			GameTypeToClient
		}

		///////////////////////////////////////////

		GameMapType _type = null; public new GameMapType Type { get { return _type; } }

		public GameMap()
		{
			instance = this;
		}

		public static new GameMap Instance
		{
			get { return instance; }
		}

		/// <summary>Overridden from <see cref="Engine.EntitySystem.Entity.OnPostCreate2(bool)"/>.</summary>
		protected override void OnPostCreate2( bool loaded )
		{
			base.OnPostCreate2( loaded );

			GameWorld gameWorld = Parent as GameWorld;
			if( gameWorld != null )
				gameWorld.DoActionsAfterMapCreated();

			UpdateWindSpeedSettings();
		}

		/// <summary>Overridden from <see cref="Engine.EntitySystem.Entity.OnDestroy()"/>.</summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
			instance = null;
		}

		[DefaultValue( GameMap.GameTypes.Action )]
		public GameTypes GameType
		{
			get { return gameType; }
			set
			{
				gameType = value;

				//send to clients
				if( EntitySystemWorld.Instance.IsServer() )
					Server_SendGameTypeToClients( EntitySystemWorld.Instance.RemoteEntityWorlds );
			}
		}

		protected override void OnPreloadResources()
		{
			base.OnPreloadResources();

			//here you can to preload resources for your specific map.
			//
			//example:
			//EntityType entityType = EntityTypes.Instance.GetByName( "MyEntity" );
			//if( entityType != null )
			//entityType.PreloadResources();
		}

		[DefaultValue( "Sounds\\Music\\Game.ogg" )]
		[Editor( typeof( EditorSoundUITypeEditor ), typeof( UITypeEditor ) )]
		public string GameMusic
		{
			get { return gameMusic; }
			set { gameMusic = value; }
		}

		void Server_SendGameTypeToClients( IList<RemoteEntityWorld> remoteEntityWorlds )
		{
			SendDataWriter writer = BeginNetworkMessage( remoteEntityWorlds, typeof( GameMap ),
				(ushort)NetworkMessages.GameTypeToClient );
			writer.WriteVariableUInt32( (uint)gameType );
			EndNetworkMessage();
		}

		[NetworkReceive( NetworkDirections.ToClient, (ushort)NetworkMessages.GameTypeToClient )]
		void Client_ReceiveGameType( RemoteEntityWorld sender, ReceiveDataReader reader )
		{
			GameTypes value = (GameTypes)reader.ReadVariableUInt32();
			if( !reader.Complete() )
				return;
			gameType = value;
		}

		protected override void Server_OnClientConnectedBeforePostCreate(
			RemoteEntityWorld remoteEntityWorld )
		{
			base.Server_OnClientConnectedBeforePostCreate( remoteEntityWorld );

			//send gameType value to the connected world
			Server_SendGameTypeToClients( new RemoteEntityWorld[] { remoteEntityWorld } );
		}

		protected override bool OnIsBillboardVisibleByCamera( Camera camera, float cameraVisibleStartOffset,
			Vec3 billboardPosition, object billboardOwner )
		{
			//We can override behaviour of billboard visiblity check when CameraVisibleCheck property is True.
			//MapObjectAttachedBillboard, CameraAttachedObject classes are supported.
			//By default visibility is checking by mean frustum and by mean physics ray cast.

			return base.OnIsBillboardVisibleByCamera( camera, cameraVisibleStartOffset, billboardPosition,
				billboardOwner );
		}

		[DefaultValue( typeof( Radian ), "0" )]
		[TypeConverter( typeof( RadianAsDegreeConverter ) )]
		[Editor( typeof( SingleValueEditor ), typeof( UITypeEditor ) )]
		[EditorLimitsRange( 0, MathFunctions.PI * 2 )]
		public Radian WindDirection
		{
			get { return windDirection; }
			set
			{
				if( windDirection == value )
					return;
				windDirection = value;
				UpdateWindSpeedSettings();
			}
		}

		[DefaultValue( 1f )]
		[Editor( typeof( SingleValueEditor ), typeof( UITypeEditor ) )]
		[EditorLimitsRange( 0, 20 )]
		public float WindSpeed
		{
			get { return windSpeed; }
			set
			{
				if( windSpeed == value )
					return;
				windSpeed = value;
				UpdateWindSpeedSettings();
			}
		}

		void UpdateWindSpeedSettings()
		{
			Vec2 speed = new Vec2(
				MathFunctions.Cos( windDirection ) * windSpeed,
				MathFunctions.Sin( windDirection ) * windSpeed );

			//update Vegetation materials
			foreach( VegetationMaterial material in VegetationMaterial.AllVegetationMaterials )
				material.UpdateGlobalWindSettings( speed );
		}

		public UnitType PlayerUnitType
		{
			get { return playerUnitType; }
			set { playerUnitType = value; }
		}

	}

}
