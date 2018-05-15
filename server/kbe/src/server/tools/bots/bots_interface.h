// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#if defined(DEFINE_IN_INTERFACE)
	#undef KBE_BOTS_INTERFACE_H
#endif


#ifndef KBE_BOTS_INTERFACE_H
#define KBE_BOTS_INTERFACE_H

// common include	
#if defined(BOTS)
#include "bots.h"
#endif
#include "bots_interface_macros.h"
#include "network/interface_defs.h"
#include "client_lib/common.h"

//#define NDEBUG
// windows include	
#if KBE_PLATFORM == PLATFORM_WIN32
#else
// linux include
#endif
	
namespace KBEngine{

/**
	Bots������Ϣ�ӿ��ڴ˶���
*/
NETWORK_INTERFACE_DECLARE_BEGIN(BotsInterface)

	// ĳapp��������look��
	BOTS_MESSAGE_DECLARE_ARGS0(lookApp,									NETWORK_FIXED_MESSAGE)

	// ����رշ�����
	BOTS_MESSAGE_DECLARE_STREAM(reqCloseServer,							NETWORK_VARIABLE_MESSAGE)

	// consoleԶ��ִ��python��䡣
	BOTS_MESSAGE_DECLARE_STREAM(onExecScriptCommand,					NETWORK_VARIABLE_MESSAGE)

	// ĳ��app��app��֪���ڻ״̬��
	BOTS_MESSAGE_DECLARE_ARGS2(onAppActiveTick,							NETWORK_FIXED_MESSAGE,
								COMPONENT_TYPE,							componentType, 
								COMPONENT_ID,							componentID)

	// ���bots��
	BOTS_MESSAGE_DECLARE_STREAM(addBots,								NETWORK_VARIABLE_MESSAGE)

	// �����ѯwatcher����
	BOTS_MESSAGE_DECLARE_STREAM(queryWatcher,							NETWORK_VARIABLE_MESSAGE)

	// ��ʼprofile
	BOTS_MESSAGE_DECLARE_STREAM(startProfile,							NETWORK_VARIABLE_MESSAGE)

	// ����ǿ��ɱ����ǰapp
	BOTS_MESSAGE_DECLARE_STREAM(reqKillServer,							NETWORK_VARIABLE_MESSAGE)

NETWORK_INTERFACE_DECLARE_END()

#ifdef DEFINE_IN_INTERFACE
	#undef DEFINE_IN_INTERFACE
#endif

}
#endif
