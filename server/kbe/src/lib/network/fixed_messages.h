// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_FIXED_NETWORK_MESSAGES_H
#define KBE_FIXED_NETWORK_MESSAGES_H

#include "common/common.h"
#include "common/singleton.h"
#include "helper/debug_helper.h"
#include "network/common.h"

namespace KBEngine { 
namespace Network
{
/*
	�������м�Э��(ǰ������֮��)����ǿ��Լ����
	û��ʹ�õ�kbe����Э���Զ��󶨻��Ƶ�ǰ�˿���ʹ�ô˴���ǿ��Լ��Э�顣
*/
class FixedMessages : public Singleton<FixedMessages>
{
public:

	// �̶���Э�����ݽṹ
	struct MSGInfo
	{
		MessageID msgid;
		std::string msgname;
		//std::wstring descr;
	};

public:
	FixedMessages();
	~FixedMessages();

	bool loadConfig(std::string fileName, bool notFoundError = true);

	FixedMessages::MSGInfo* isFixed(const char* msgName);
	bool isFixed(MessageID msgid);

public:
	typedef KBEUnordered_map<std::string, MSGInfo> MSGINFO_MAP;

private:
	MSGINFO_MAP _infomap;
	bool _loaded;
};

}
}
#endif // KBE_FIXED_NETWORK_MESSAGES_H
