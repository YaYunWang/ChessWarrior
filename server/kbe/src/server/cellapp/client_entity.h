// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBE_CLIENT_ENTITY_H
#define KBE_CLIENT_ENTITY_H
	
// common include	
#include "common/common.h"
//#include "network/channel.h"
#include "pyscript/scriptobject.h"
#include "entitydef/common.h"
#include "network/address.h"

//#define NDEBUG
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <iostream>	
#include <map>	
#include <vector>	
// windows include	
#if KBE_PLATFORM == PLATFORM_WIN32
#include <time.h> 
#else
// linux include
#include <errno.h>
#endif
	
namespace KBEngine{

namespace Network
{
class Channel;
class Bundle;
}

class ClientEntity;
class ScriptDefModule;
class PropertyDescription;

class ClientEntityComponent : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(ClientEntityComponent, ScriptObject)
public:
	ClientEntityComponent(PropertyDescription* pComponentPropertyDescription, ClientEntity* pClientEntity);

	~ClientEntityComponent();

	ScriptDefModule* pComponentScriptDefModule();

	/**
		�ű������ȡ���Ի��߷���
	*/
	PyObject* onScriptGetAttribute(PyObject* attr);

	/**
		��ö��������
	*/
	PyObject* tp_repr();
	PyObject* tp_str();

	void c_str(char* s, size_t size);

protected:
	ClientEntity*							pClientEntity_;
	PropertyDescription*					pComponentPropertyDescription_;

};

class ClientEntity : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(ClientEntity, ScriptObject)
public:
	ClientEntity(ENTITY_ID srcEntityID, ENTITY_ID clientEntityID);
	
	~ClientEntity();
	
	/** 
		�ű������ȡ���Ի��߷��� 
	*/
	PyObject* onScriptGetAttribute(PyObject* attr);						
			
	/** 
		��ö�������� 
	*/
	PyObject* tp_repr();
	PyObject* tp_str();
	
	void c_str(char* s, size_t size);

	ENTITY_ID srcEntityID() const {
		return srcEntityID_;
	}

	ENTITY_ID clientEntityID() const {
		return clientEntityID_;
	}

protected:
	ENTITY_ID								srcEntityID_;						// srcEntityID_

	ENTITY_ID								clientEntityID_;					// clientEntityID_
};

}

#endif // KBE_CLIENT_ENTITY_H
