// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_ENTITY_AUTOLOADER_H
#define KBE_ENTITY_AUTOLOADER_H

#include "common/common.h"

namespace KBEngine{

class InitProgressHandler;
class EntityAutoLoader
{
public:
	EntityAutoLoader(Network::NetworkInterface & networkInterface, InitProgressHandler* pInitProgressHandler);
	~EntityAutoLoader();
	
	bool process();

	void pInitProgressHandler(InitProgressHandler* p)
		{ pInitProgressHandler_ = p; }

	/** ����ӿ�
		���ݿ��в�ѯ���Զ�entity������Ϣ����
	*/
	void onEntityAutoLoadCBFromDBMgr(Network::Channel* pChannel, MemoryStream& s);

private:
	Network::NetworkInterface & networkInterface_;
	InitProgressHandler* pInitProgressHandler_;

	std::vector< std::vector<ENTITY_SCRIPT_UID> > entityTypes_;

	// ÿ��ȡ��ѯ�����������
	ENTITY_ID start_;
	ENTITY_ID end_;

	bool querying_;
};


}

#endif // KBE_ENTITY_AUTOLOADER_H
