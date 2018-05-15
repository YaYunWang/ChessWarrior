// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com
#ifndef KBE_GLOBAL_DATA_SERVER_H
#define KBE_GLOBAL_DATA_SERVER_H

#include "common/common.h"
#include "helper/debug_helper.h"
	
namespace KBEngine{
namespace Network
{
	class Channel;
}

class GlobalDataServer
{
public:	
	enum DATA_TYPE
	{
		GLOBAL_DATA,
		BASEAPP_DATA,
		CELLAPP_DATA
	};

public:	
	GlobalDataServer(DATA_TYPE dataType);
	~GlobalDataServer();
			
	/** д���� */
	bool write(Network::Channel* pChannel, COMPONENT_TYPE componentType, const std::string& key, const std::string& value);
	
	/** ɾ������ */
	bool del(Network::Channel* pChannel, COMPONENT_TYPE componentType, const std::string& key);	
	
	/** ��Ӹ÷���������Ҫ���ĵ������� */
	void addConcernComponentType(COMPONENT_TYPE ct){ concernComponentTypes_.push_back(ct); }
	
	/** �㲥һ�����ݵĸı�������ĵ���� */
	void broadcastDataChanged(Network::Channel* pChannel, COMPONENT_TYPE componentType, const std::string& key, 
							const std::string& value, bool isDelete = false);
	
	/** һ���µĿͻ��˵�½ */
	void onGlobalDataClientLogon(Network::Channel* client, COMPONENT_TYPE componentType);

private:
	DATA_TYPE dataType_;

	std::vector<COMPONENT_TYPE> concernComponentTypes_;						// ��GlobalDataServer����Ҫ���ĵ�������
	typedef std::map<std::string, std::string> DATA_MAP;
	typedef DATA_MAP::iterator DATA_MAP_KEY;
	DATA_MAP dict_;
} ;

}

#endif // KBE_GLOBAL_DATA_SERVER_H
