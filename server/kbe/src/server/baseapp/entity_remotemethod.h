// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_BASE_REMOTE_METHOD_H
#define KBE_BASE_REMOTE_METHOD_H


#include "helper/debug_helper.h"
#include "common/common.h"	
#include "entitydef/remote_entity_method.h"

namespace KBEngine{

class EntityRemoteMethod : public RemoteEntityMethod
{
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(EntityRemoteMethod, RemoteEntityMethod)	
public:
	EntityRemoteMethod(MethodDescription* methodDescription, 
						EntityCallAbstract* entityCall);

	~EntityRemoteMethod();

	static PyObject* tp_call(PyObject* self, 
			PyObject* args, PyObject* kwds);
private:

};

}
#endif
