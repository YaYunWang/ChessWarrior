// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBENGINE_CLIENT_ENTITY_METHOD_H
#define KBENGINE_CLIENT_ENTITY_METHOD_H

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

class ClientEntityMethod : public script::ScriptObject
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(ClientEntityMethod, script::ScriptObject)	
public:	
	ClientEntityMethod(PropertyDescription* pComponentPropertyDescription,
		const ScriptDefModule* pScriptModule, MethodDescription* methodDescription,
		ENTITY_ID srcEntityID, ENTITY_ID clientEntityID);
	
	virtual ~ClientEntityMethod();

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
	PropertyDescription*					pComponentPropertyDescription_;

	const ScriptDefModule*					pScriptModule_;						// ��entity��ʹ�õĽű�ģ�����

	MethodDescription*						methodDescription_;					// �������������

	ENTITY_ID								srcEntityID_;						// srcEntityID_

	ENTITY_ID								clientEntityID_;					// clientEntityID_
};

}

#endif // KBENGINE_CLIENT_ENTITY_METHOD_H
