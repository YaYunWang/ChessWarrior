/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Account : AccountBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Account.def
	// Please inherit and implement "class Account : AccountBase"
	public abstract class AccountBase : Entity
	{
		public EntityBaseEntityCall_AccountBase baseEntityCall = null;
		public EntityCellEntityCall_AccountBase cellEntityCall = null;

		public CHESS_INFO_LIST MineChess = new CHESS_INFO_LIST();
		public virtual void onMineChessChanged(CHESS_INFO_LIST oldValue) {}
		public string RoleName = "";
		public virtual void onRoleNameChanged(string oldValue) {}
		public Int16 RoleType = 0;
		public virtual void onRoleTypeChanged(Int16 oldValue) {}

		public abstract void ReNameResult(Int16 arg1); 

		public AccountBase()
		{
		}

		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_AccountBase(id, className);
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_AccountBase(id, className);
		}

		public override void onLoseCell()
		{
			cellEntityCall = null;
		}

		public override EntityCall getBaseEntityCall()
		{
			return baseEntityCall;
		}

		public override EntityCall getCellEntityCall()
		{
			return cellEntityCall;
		}

		public override void onRemoteMethodCall(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Account"];

			UInt16 methodUtype = 0;
			UInt16 componentPropertyUType = 0;

			if(sm.useMethodDescrAlias)
			{
				componentPropertyUType = stream.readUint8();
				methodUtype = stream.readUint8();
			}
			else
			{
				componentPropertyUType = stream.readUint16();
				methodUtype = stream.readUint16();
			}

			Method method = null;

			if(componentPropertyUType == 0)
			{
				method = sm.idmethods[methodUtype];
			}
			else
			{
				Property pComponentPropertyDescription = sm.idpropertys[componentPropertyUType];
				switch(pComponentPropertyDescription.properUtype)
				{
					default:
						break;
				}

				return;
			}

			switch(method.methodUtype)
			{
				case 2:
					Int16 ReNameResult_arg1 = stream.readInt16();
					ReNameResult(ReNameResult_arg1);
					break;
				default:
					break;
			};
		}

		public override void onUpdatePropertys(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Account"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			while(stream.length() > 0)
			{
				UInt16 _t_utype = 0;
				UInt16 _t_child_utype = 0;

				{
					if(sm.usePropertyDescrAlias)
					{
						_t_utype = stream.readUint8();
						_t_child_utype = stream.readUint8();
					}
					else
					{
						_t_utype = stream.readUint16();
						_t_child_utype = stream.readUint16();
					}
				}

				Property prop = null;

				if(_t_utype == 0)
				{
					prop = pdatas[_t_child_utype];
				}
				else
				{
					Property pComponentPropertyDescription = pdatas[_t_utype];
					switch(pComponentPropertyDescription.properUtype)
					{
						default:
							break;
					}

					return;
				}

				switch(prop.properUtype)
				{
					case 3:
						CHESS_INFO_LIST oldval_MineChess = MineChess;
						MineChess = ((DATATYPE_CHESS_INFO_LIST)EntityDef.id2datatypes[23]).createFromStreamEx(stream);

						if(prop.isBase())
						{
							if(inited)
								onMineChessChanged(oldval_MineChess);
						}
						else
						{
							if(inWorld)
								onMineChessChanged(oldval_MineChess);
						}

						break;
					case 2:
						string oldval_RoleName = RoleName;
						RoleName = stream.readUnicode();

						if(prop.isBase())
						{
							if(inited)
								onRoleNameChanged(oldval_RoleName);
						}
						else
						{
							if(inWorld)
								onRoleNameChanged(oldval_RoleName);
						}

						break;
					case 1:
						Int16 oldval_RoleType = RoleType;
						RoleType = stream.readInt16();

						if(prop.isBase())
						{
							if(inited)
								onRoleTypeChanged(oldval_RoleType);
						}
						else
						{
							if(inWorld)
								onRoleTypeChanged(oldval_RoleType);
						}

						break;
					case 40001:
						Vector3 oldval_direction = direction;
						direction = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onDirectionChanged(oldval_direction);
						}
						else
						{
							if(inWorld)
								onDirectionChanged(oldval_direction);
						}

						break;
					case 40000:
						Vector3 oldval_position = position;
						position = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onPositionChanged(oldval_position);
						}
						else
						{
							if(inWorld)
								onPositionChanged(oldval_position);
						}

						break;
					case 40002:
						stream.readUint32();
						break;
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Account"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			CHESS_INFO_LIST oldval_MineChess = MineChess;
			Property prop_MineChess = pdatas[4];
			if(prop_MineChess.isBase())
			{
				if(inited && !inWorld)
					onMineChessChanged(oldval_MineChess);
			}
			else
			{
				if(inWorld)
				{
					if(prop_MineChess.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onMineChessChanged(oldval_MineChess);
					}
				}
			}

			string oldval_RoleName = RoleName;
			Property prop_RoleName = pdatas[5];
			if(prop_RoleName.isBase())
			{
				if(inited && !inWorld)
					onRoleNameChanged(oldval_RoleName);
			}
			else
			{
				if(inWorld)
				{
					if(prop_RoleName.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onRoleNameChanged(oldval_RoleName);
					}
				}
			}

			Int16 oldval_RoleType = RoleType;
			Property prop_RoleType = pdatas[6];
			if(prop_RoleType.isBase())
			{
				if(inited && !inWorld)
					onRoleTypeChanged(oldval_RoleType);
			}
			else
			{
				if(inWorld)
				{
					if(prop_RoleType.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onRoleTypeChanged(oldval_RoleType);
					}
				}
			}

			Vector3 oldval_direction = direction;
			Property prop_direction = pdatas[2];
			if(prop_direction.isBase())
			{
				if(inited && !inWorld)
					onDirectionChanged(oldval_direction);
			}
			else
			{
				if(inWorld)
				{
					if(prop_direction.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onDirectionChanged(oldval_direction);
					}
				}
			}

			Vector3 oldval_position = position;
			Property prop_position = pdatas[1];
			if(prop_position.isBase())
			{
				if(inited && !inWorld)
					onPositionChanged(oldval_position);
			}
			else
			{
				if(inWorld)
				{
					if(prop_position.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onPositionChanged(oldval_position);
					}
				}
			}

		}
	}
}