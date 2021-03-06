﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpaceAim3D.WebService
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Db")]
	public partial class DbDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertPlayer(Player instance);
    partial void UpdatePlayer(Player instance);
    partial void DeletePlayer(Player instance);
    partial void InsertResult(Result instance);
    partial void UpdateResult(Result instance);
    partial void DeleteResult(Result instance);
    #endregion
		
		public DbDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DbDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Player> Players
		{
			get
			{
				return this.GetTable<Player>();
			}
		}
		
		public System.Data.Linq.Table<Result> Results
		{
			get
			{
				return this.GetTable<Result>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Players")]
	public partial class Player : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PlayerId;
		
		private string _Key;
		
		private string _Name;
		
		private System.Nullable<double> _LocationLatitude;
		
		private System.Nullable<double> _LocationLongitude;
		
		private System.DateTime _LastUpdate;
		
		private EntitySet<Result> _Results;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPlayerIdChanging(int value);
    partial void OnPlayerIdChanged();
    partial void OnKeyChanging(string value);
    partial void OnKeyChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnLocationLatitudeChanging(System.Nullable<double> value);
    partial void OnLocationLatitudeChanged();
    partial void OnLocationLongitudeChanging(System.Nullable<double> value);
    partial void OnLocationLongitudeChanged();
    partial void OnLastUpdateChanging(System.DateTime value);
    partial void OnLastUpdateChanged();
    #endregion
		
		public Player()
		{
			this._Results = new EntitySet<Result>(new Action<Result>(this.attach_Results), new Action<Result>(this.detach_Results));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayerId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PlayerId
		{
			get
			{
				return this._PlayerId;
			}
			set
			{
				if ((this._PlayerId != value))
				{
					this.OnPlayerIdChanging(value);
					this.SendPropertyChanging();
					this._PlayerId = value;
					this.SendPropertyChanged("PlayerId");
					this.OnPlayerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Key]", Storage="_Key", DbType="NVarChar(32) NOT NULL", CanBeNull=false)]
		public string Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				if ((this._Key != value))
				{
					this.OnKeyChanging(value);
					this.SendPropertyChanging();
					this._Key = value;
					this.SendPropertyChanged("Key");
					this.OnKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationLatitude", DbType="Float")]
		public System.Nullable<double> LocationLatitude
		{
			get
			{
				return this._LocationLatitude;
			}
			set
			{
				if ((this._LocationLatitude != value))
				{
					this.OnLocationLatitudeChanging(value);
					this.SendPropertyChanging();
					this._LocationLatitude = value;
					this.SendPropertyChanged("LocationLatitude");
					this.OnLocationLatitudeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationLongitude", DbType="Float")]
		public System.Nullable<double> LocationLongitude
		{
			get
			{
				return this._LocationLongitude;
			}
			set
			{
				if ((this._LocationLongitude != value))
				{
					this.OnLocationLongitudeChanging(value);
					this.SendPropertyChanging();
					this._LocationLongitude = value;
					this.SendPropertyChanged("LocationLongitude");
					this.OnLocationLongitudeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastUpdate", DbType="DateTime NOT NULL")]
		public System.DateTime LastUpdate
		{
			get
			{
				return this._LastUpdate;
			}
			set
			{
				if ((this._LastUpdate != value))
				{
					this.OnLastUpdateChanging(value);
					this.SendPropertyChanging();
					this._LastUpdate = value;
					this.SendPropertyChanged("LastUpdate");
					this.OnLastUpdateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Player_Result", Storage="_Results", ThisKey="PlayerId", OtherKey="PlayerId")]
		public EntitySet<Result> Results
		{
			get
			{
				return this._Results;
			}
			set
			{
				this._Results.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Results(Result entity)
		{
			this.SendPropertyChanging();
			entity.Player = this;
		}
		
		private void detach_Results(Result entity)
		{
			this.SendPropertyChanging();
			entity.Player = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Results")]
	public partial class Result : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ResultId;
		
		private int _Score;
		
		private int _PlayerId;
		
		private System.DateTime _Date;
		
		private EntityRef<Player> _Player;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnResultIdChanging(int value);
    partial void OnResultIdChanged();
    partial void OnScoreChanging(int value);
    partial void OnScoreChanged();
    partial void OnPlayerIdChanging(int value);
    partial void OnPlayerIdChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    #endregion
		
		public Result()
		{
			this._Player = default(EntityRef<Player>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ResultId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ResultId
		{
			get
			{
				return this._ResultId;
			}
			set
			{
				if ((this._ResultId != value))
				{
					this.OnResultIdChanging(value);
					this.SendPropertyChanging();
					this._ResultId = value;
					this.SendPropertyChanged("ResultId");
					this.OnResultIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Score", DbType="Int NOT NULL")]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				if ((this._Score != value))
				{
					this.OnScoreChanging(value);
					this.SendPropertyChanging();
					this._Score = value;
					this.SendPropertyChanged("Score");
					this.OnScoreChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayerId", DbType="Int NOT NULL")]
		public int PlayerId
		{
			get
			{
				return this._PlayerId;
			}
			set
			{
				if ((this._PlayerId != value))
				{
					if (this._Player.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnPlayerIdChanging(value);
					this.SendPropertyChanging();
					this._PlayerId = value;
					this.SendPropertyChanged("PlayerId");
					this.OnPlayerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Player_Result", Storage="_Player", ThisKey="PlayerId", OtherKey="PlayerId", IsForeignKey=true)]
		public Player Player
		{
			get
			{
				return this._Player.Entity;
			}
			set
			{
				Player previousValue = this._Player.Entity;
				if (((previousValue != value) 
							|| (this._Player.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Player.Entity = null;
						previousValue.Results.Remove(this);
					}
					this._Player.Entity = value;
					if ((value != null))
					{
						value.Results.Add(this);
						this._PlayerId = value.PlayerId;
					}
					else
					{
						this._PlayerId = default(int);
					}
					this.SendPropertyChanged("Player");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
