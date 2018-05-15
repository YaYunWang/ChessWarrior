// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com
#ifndef KBE_GLOBAL_DATA_CLIENT_H
#define KBE_GLOBAL_DATA_CLIENT_H

#include "globaldata_server.h"
#include "common/common.h"
#include "helper/debug_helper.h"
#include "pyscript/map.h"

namespace KBEngine{

class GlobalDataClient : public script::Map
{	
	/** ���໯ ��һЩpy�������������� */
	INSTANCE_SCRIPT_HREADER(GlobalDataClient, script::Map)
		
public:	
	GlobalDataClient(COMPONENT_TYPE componentType, GlobalDataServer::DATA_TYPE dataType);
	~GlobalDataClient();
	
	/** д���� */
	bool write(PyObject* pyKey, PyObject* pyValue);
	
	/** ɾ������ */
	bool del(PyObject* pyKey);
	
	/** ���ݸı�֪ͨ */
	void onDataChanged(PyObject* key, PyObject* value, bool isDelete = false);
	
	/** ���ø�ȫ�����ݿͻ��˵ķ������������ */
	void setServerComponentType(COMPONENT_TYPE ct){ serverComponentType_ = ct; }
	
private:
	COMPONENT_TYPE					serverComponentType_;				// GlobalDataServer���ڷ��������������
	GlobalDataServer::DATA_TYPE 	dataType_;
} ;

}

#endif // KBE_GLOBAL_DATA_CLIENT_H
