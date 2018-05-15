// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBENGINE_CLIENTS_REMOTE_ENTITY_METHOD_H
#define KBENGINE_CLIENTS_REMOTE_ENTITY_METHOD_H

#include "common/common.h"
#if KBE_PLATFORM == PLATFORM_WIN32
#pragma warning (disable : 4910)
#pragma warning (disable : 4251)
#endif
// common include	
#include "entitydef/datatype.h"
#include "entitydef/datatypes.h"
#include "helper/debug_helper.h"
#include "pyscript/scriptobject.h"	
//#define NDEBUG
// windows include	
#if KBE_PLATFORM == PLATFORM_WIN32
#else
// linux include
#endif

namespace KBEngine{

class ClientsRemoteEntityMethod : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(ClientsRemoteEntityMethod, script::ScriptObject)	
public:	
	ClientsRemoteEntityMethod(PropertyDescription* pComponentPropertyDescription, 
		const ScriptDefModule* pScriptModule, 
		MethodDescription* methodDescription,
		bool otherClients,
		ENTITY_ID id);
	
	virtual ~ClientsRemoteEntityMethod();

	const char* getName(void) const
	{ 
		return methodDescription_->getName(); 
	};

	MethodDescription* getDescription(void) const
	{ 
		return methodDescription_; 
	}

	static PyObject* tp_call(PyObject* self, 
			PyObject* args, PyObject* kwds);

	PyObject* callmethod(PyObject* args, PyObject* kwds);

protected:	
	PropertyDescription*	pComponentPropertyDescription_;

	const ScriptDefModule*	pScriptModule_;			// ��entity��ʹ�õĽű�ģ�����
	MethodDescription*		methodDescription_;		// �������������

	bool					otherClients_;			// �Ƿ�ֻ�������ͻ��ˣ� �������Լ�

	ENTITY_ID				id_;					// entityID
};

}

#endif // KBENGINE_CLIENTS_REMOTE_ENTITY_METHOD_H
