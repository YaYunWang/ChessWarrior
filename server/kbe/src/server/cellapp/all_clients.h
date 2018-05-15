// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBE_ALL_CLIENTS_H
#define KBE_ALL_CLIENTS_H

#include "common/common.h"
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

class AllClients;
class ScriptDefModule;
class PropertyDescription;

class AllClientsComponent : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(AllClientsComponent, ScriptObject)
public:
	AllClientsComponent(PropertyDescription* pComponentPropertyDescription, AllClients* pAllClients);

	~AllClientsComponent();

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

	ScriptDefModule* pComponentScriptDefModule();

protected:
	AllClients* pAllClients_;
	PropertyDescription* pComponentPropertyDescription_;
};

class AllClients : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(AllClients, ScriptObject)
public:
	AllClients(const ScriptDefModule* pScriptModule, 
		ENTITY_ID eid, 
		bool otherClients);
	
	~AllClients();
	
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
	
	/** 
		��ȡentityID 
	*/
	ENTITY_ID id() const{ return id_; }
	void setID(int id){ id_ = id; }
	DECLARE_PY_GET_MOTHOD(pyGetID);

	void setScriptModule(const ScriptDefModule*	pScriptModule){ 
		pScriptModule_ = pScriptModule; 
	}

	bool isOtherClients() const {
		return otherClients_;
	}

protected:
	const ScriptDefModule*					pScriptModule_;			// ��entity��ʹ�õĽű�ģ�����

	ENTITY_ID								id_;					// entityID

	bool									otherClients_;			// �Ƿ�ֻ�������ͻ��ˣ� �������Լ�
};

}

#endif // KBE_ALL_CLIENTS_H
