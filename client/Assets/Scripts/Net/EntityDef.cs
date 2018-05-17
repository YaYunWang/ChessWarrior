/*
	Generated by KBEngine!
	Please do not modify this file!
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class EntityDef
	{
		public static Dictionary<string, UInt16> datatype2id = new Dictionary<string, UInt16>();
		public static Dictionary<string, DATATYPE_BASE> datatypes = new Dictionary<string, DATATYPE_BASE>();
		public static Dictionary<UInt16, DATATYPE_BASE> id2datatypes = new Dictionary<UInt16, DATATYPE_BASE>();
		public static Dictionary<string, Int32> entityclass = new Dictionary<string, Int32>();
		public static Dictionary<string, ScriptModule> moduledefs = new Dictionary<string, ScriptModule>();
		public static Dictionary<UInt16, ScriptModule> idmoduledefs = new Dictionary<UInt16, ScriptModule>();

		public static bool init()
		{
			initDataTypes();
			initDefTypes();
			initScriptModules();
			return true;
		}

		public static bool reset()
		{
			clear();
			return init();
		}

		public static void clear()
		{
			datatype2id.Clear();
			datatypes.Clear();
			id2datatypes.Clear();
			entityclass.Clear();
			moduledefs.Clear();
			idmoduledefs.Clear();
		}

		public static void initDataTypes()
		{
			datatypes["UINT8"] = new DATATYPE_UINT8();
			datatypes["UINT16"] = new DATATYPE_UINT16();
			datatypes["UINT32"] = new DATATYPE_UINT32();
			datatypes["UINT64"] = new DATATYPE_UINT64();

			datatypes["INT8"] = new DATATYPE_INT8();
			datatypes["INT16"] = new DATATYPE_INT16();
			datatypes["INT32"] = new DATATYPE_INT32();
			datatypes["INT64"] = new DATATYPE_INT64();

			datatypes["FLOAT"] = new DATATYPE_FLOAT();
			datatypes["DOUBLE"] = new DATATYPE_DOUBLE();

			datatypes["STRING"] = new DATATYPE_STRING();
			datatypes["VECTOR2"] = new DATATYPE_VECTOR2();

			datatypes["VECTOR3"] = new DATATYPE_VECTOR3();

			datatypes["VECTOR4"] = new DATATYPE_VECTOR4();
			datatypes["PYTHON"] = new DATATYPE_PYTHON();

			datatypes["UNICODE"] = new DATATYPE_UNICODE();
			datatypes["ENTITYCALL"] = new DATATYPE_ENTITYCALL();

			datatypes["BLOB"] = new DATATYPE_BLOB();
		}

		public static void initScriptModules()
		{
			ScriptModule pAccountModule = new ScriptModule("Account");
			EntityDef.moduledefs["Account"] = pAccountModule;
			EntityDef.idmoduledefs[1] = pAccountModule;

			Property pAccount_position = new Property();
			pAccount_position.name = "position";
			pAccount_position.properUtype = 40000;
			pAccount_position.properFlags = 4;
			pAccount_position.aliasID = 1;
			Vector3 Account_position_defval = new Vector3();
			pAccount_position.defaultVal = Account_position_defval;
			pAccountModule.propertys["position"] = pAccount_position; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_position.aliasID] = pAccount_position;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(position / 40000).");

			Property pAccount_direction = new Property();
			pAccount_direction.name = "direction";
			pAccount_direction.properUtype = 40001;
			pAccount_direction.properFlags = 4;
			pAccount_direction.aliasID = 2;
			Vector3 Account_direction_defval = new Vector3();
			pAccount_direction.defaultVal = Account_direction_defval;
			pAccountModule.propertys["direction"] = pAccount_direction; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_direction.aliasID] = pAccount_direction;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(direction / 40001).");

			Property pAccount_spaceID = new Property();
			pAccount_spaceID.name = "spaceID";
			pAccount_spaceID.properUtype = 40002;
			pAccount_spaceID.properFlags = 16;
			pAccount_spaceID.aliasID = 3;
			UInt32 Account_spaceID_defval;
			UInt32.TryParse("", out Account_spaceID_defval);
			pAccount_spaceID.defaultVal = Account_spaceID_defval;
			pAccountModule.propertys["spaceID"] = pAccount_spaceID; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_spaceID.aliasID] = pAccount_spaceID;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(spaceID / 40002).");

			Property pAccount_MineChess = new Property();
			pAccount_MineChess.name = "MineChess";
			pAccount_MineChess.properUtype = 3;
			pAccount_MineChess.properFlags = 32;
			pAccount_MineChess.aliasID = 4;
			pAccount_MineChess.defaultVal = EntityDef.id2datatypes[23].parseDefaultValStr("");
			pAccountModule.propertys["MineChess"] = pAccount_MineChess; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_MineChess.aliasID] = pAccount_MineChess;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(MineChess / 3).");

			Property pAccount_RoleName = new Property();
			pAccount_RoleName.name = "RoleName";
			pAccount_RoleName.properUtype = 2;
			pAccount_RoleName.properFlags = 32;
			pAccount_RoleName.aliasID = 5;
			string Account_RoleName_defval = "";
			pAccount_RoleName.defaultVal = Account_RoleName_defval;
			pAccountModule.propertys["RoleName"] = pAccount_RoleName; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_RoleName.aliasID] = pAccount_RoleName;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(RoleName / 2).");

			Property pAccount_RoleType = new Property();
			pAccount_RoleType.name = "RoleType";
			pAccount_RoleType.properUtype = 1;
			pAccount_RoleType.properFlags = 32;
			pAccount_RoleType.aliasID = 6;
			Int16 Account_RoleType_defval;
			Int16.TryParse("0", out Account_RoleType_defval);
			pAccount_RoleType.defaultVal = Account_RoleType_defval;
			pAccountModule.propertys["RoleType"] = pAccount_RoleType; 

			pAccountModule.usePropertyDescrAlias = true;
			pAccountModule.idpropertys[(UInt16)pAccount_RoleType.aliasID] = pAccount_RoleType;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), property(RoleType / 1).");

			List<DATATYPE_BASE> pAccount_ReNameResult_args = new List<DATATYPE_BASE>();
			pAccount_ReNameResult_args.Add(EntityDef.id2datatypes[7]);

			Method pAccount_ReNameResult = new Method();
			pAccount_ReNameResult.name = "ReNameResult";
			pAccount_ReNameResult.methodUtype = 3;
			pAccount_ReNameResult.aliasID = 1;
			pAccount_ReNameResult.args = pAccount_ReNameResult_args;

			pAccountModule.methods["ReNameResult"] = pAccount_ReNameResult; 
			pAccountModule.useMethodDescrAlias = true;
			pAccountModule.idmethods[(UInt16)pAccount_ReNameResult.aliasID] = pAccount_ReNameResult;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), method(ReNameResult / 3).");

			List<DATATYPE_BASE> pAccount_ReCreateAccountRequest_args = new List<DATATYPE_BASE>();
			pAccount_ReCreateAccountRequest_args.Add(EntityDef.id2datatypes[7]);
			pAccount_ReCreateAccountRequest_args.Add(EntityDef.id2datatypes[12]);

			Method pAccount_ReCreateAccountRequest = new Method();
			pAccount_ReCreateAccountRequest.name = "ReCreateAccountRequest";
			pAccount_ReCreateAccountRequest.methodUtype = 1;
			pAccount_ReCreateAccountRequest.aliasID = -1;
			pAccount_ReCreateAccountRequest.args = pAccount_ReCreateAccountRequest_args;

			pAccountModule.methods["ReCreateAccountRequest"] = pAccount_ReCreateAccountRequest; 
			pAccountModule.base_methods["ReCreateAccountRequest"] = pAccount_ReCreateAccountRequest;

			pAccountModule.idbase_methods[pAccount_ReCreateAccountRequest.methodUtype] = pAccount_ReCreateAccountRequest;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), method(ReCreateAccountRequest / 1).");

			List<DATATYPE_BASE> pAccount_TestChessEntity_args = new List<DATATYPE_BASE>();
			pAccount_TestChessEntity_args.Add(EntityDef.id2datatypes[5]);

			Method pAccount_TestChessEntity = new Method();
			pAccount_TestChessEntity.name = "TestChessEntity";
			pAccount_TestChessEntity.methodUtype = 2;
			pAccount_TestChessEntity.aliasID = -1;
			pAccount_TestChessEntity.args = pAccount_TestChessEntity_args;

			pAccountModule.methods["TestChessEntity"] = pAccount_TestChessEntity; 
			pAccountModule.base_methods["TestChessEntity"] = pAccount_TestChessEntity;

			pAccountModule.idbase_methods[pAccount_TestChessEntity.methodUtype] = pAccount_TestChessEntity;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Account), method(TestChessEntity / 2).");

			ScriptModule pChessModule = new ScriptModule("Chess");
			EntityDef.moduledefs["Chess"] = pChessModule;
			EntityDef.idmoduledefs[2] = pChessModule;

			Property pChess_position = new Property();
			pChess_position.name = "position";
			pChess_position.properUtype = 40000;
			pChess_position.properFlags = 4;
			pChess_position.aliasID = 1;
			Vector3 Chess_position_defval = new Vector3();
			pChess_position.defaultVal = Chess_position_defval;
			pChessModule.propertys["position"] = pChess_position; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_position.aliasID] = pChess_position;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(position / 40000).");

			Property pChess_direction = new Property();
			pChess_direction.name = "direction";
			pChess_direction.properUtype = 40001;
			pChess_direction.properFlags = 4;
			pChess_direction.aliasID = 2;
			Vector3 Chess_direction_defval = new Vector3();
			pChess_direction.defaultVal = Chess_direction_defval;
			pChessModule.propertys["direction"] = pChess_direction; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_direction.aliasID] = pChess_direction;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(direction / 40001).");

			Property pChess_spaceID = new Property();
			pChess_spaceID.name = "spaceID";
			pChess_spaceID.properUtype = 40002;
			pChess_spaceID.properFlags = 16;
			pChess_spaceID.aliasID = 3;
			UInt32 Chess_spaceID_defval;
			UInt32.TryParse("", out Chess_spaceID_defval);
			pChess_spaceID.defaultVal = Chess_spaceID_defval;
			pChessModule.propertys["spaceID"] = pChess_spaceID; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_spaceID.aliasID] = pChess_spaceID;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(spaceID / 40002).");

			Property pChess_chess_id = new Property();
			pChess_chess_id.name = "chess_id";
			pChess_chess_id.properUtype = 4;
			pChess_chess_id.properFlags = 32;
			pChess_chess_id.aliasID = 4;
			UInt64 Chess_chess_id_defval;
			UInt64.TryParse("0", out Chess_chess_id_defval);
			pChess_chess_id.defaultVal = Chess_chess_id_defval;
			pChessModule.propertys["chess_id"] = pChess_chess_id; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_chess_id.aliasID] = pChess_chess_id;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(chess_id / 4).");

			Property pChess_chess_level = new Property();
			pChess_chess_level.name = "chess_level";
			pChess_chess_level.properUtype = 5;
			pChess_chess_level.properFlags = 32;
			pChess_chess_level.aliasID = 5;
			UInt64 Chess_chess_level_defval;
			UInt64.TryParse("0", out Chess_chess_level_defval);
			pChess_chess_level.defaultVal = Chess_chess_level_defval;
			pChessModule.propertys["chess_level"] = pChess_chess_level; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_chess_level.aliasID] = pChess_chess_level;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(chess_level / 5).");

			Property pChess_chess_name = new Property();
			pChess_chess_name.name = "chess_name";
			pChess_chess_name.properUtype = 6;
			pChess_chess_name.properFlags = 32;
			pChess_chess_name.aliasID = 6;
			string Chess_chess_name_defval = "";
			pChess_chess_name.defaultVal = Chess_chess_name_defval;
			pChessModule.propertys["chess_name"] = pChess_chess_name; 

			pChessModule.usePropertyDescrAlias = true;
			pChessModule.idpropertys[(UInt16)pChess_chess_name.aliasID] = pChess_chess_name;

			//Dbg.DEBUG_MSG("EntityDef::initScriptModules: add(Chess), property(chess_name / 6).");

		}

		public static void initDefTypes()
		{
			{
				UInt16 utype = 2;
				string typeName = "UINT8";
				string name = "UINT8";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 3;
				string typeName = "UINT16";
				string name = "UINT16";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 5;
				string typeName = "UINT64";
				string name = "UINT64";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 4;
				string typeName = "UINT32";
				string name = "UINT32";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 6;
				string typeName = "INT8";
				string name = "INT8";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 7;
				string typeName = "INT16";
				string name = "INT16";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 8;
				string typeName = "INT32";
				string name = "INT32";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 9;
				string typeName = "INT64";
				string name = "INT64";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 1;
				string typeName = "STRING";
				string name = "STRING";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 12;
				string typeName = "UNICODE";
				string name = "UNICODE";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 13;
				string typeName = "FLOAT";
				string name = "FLOAT";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 14;
				string typeName = "DOUBLE";
				string name = "DOUBLE";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 10;
				string typeName = "PYTHON";
				string name = "PYTHON";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 10;
				string typeName = "PY_DICT";
				string name = "PY_DICT";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 10;
				string typeName = "PY_TUPLE";
				string name = "PY_TUPLE";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 10;
				string typeName = "PY_LIST";
				string name = "PY_LIST";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 10;
				string typeName = "ENTITYCALL";
				string name = "ENTITYCALL";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 11;
				string typeName = "BLOB";
				string name = "BLOB";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 15;
				string typeName = "VECTOR2";
				string name = "VECTOR2";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 16;
				string typeName = "VECTOR3";
				string name = "VECTOR3";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 17;
				string typeName = "VECTOR4";
				string name = "VECTOR4";
				DATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[typeName] = val;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 22;
				string typeName = "CHESS_INFO";
				DATATYPE_CHESS_INFO datatype = new DATATYPE_CHESS_INFO();
				EntityDef.datatypes[typeName] = datatype;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			{
				UInt16 utype = 23;
				string typeName = "CHESS_INFO_LIST";
				DATATYPE_CHESS_INFO_LIST datatype = new DATATYPE_CHESS_INFO_LIST();
				EntityDef.datatypes[typeName] = datatype;
				EntityDef.id2datatypes[utype] = EntityDef.datatypes[typeName];
				EntityDef.datatype2id[typeName] = utype;
			}

			foreach(string datatypeStr in EntityDef.datatypes.Keys)
			{
				DATATYPE_BASE dataType = EntityDef.datatypes[datatypeStr];
				if(dataType != null)
				{
					dataType.bind();
				}
			}
		}

	}


}