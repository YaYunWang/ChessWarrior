// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#if defined(DEFINE_IN_INTERFACE)
	#undef KBE_LOGGER_INTERFACE_H
#endif


#ifndef KBE_LOGGER_INTERFACE_H
#define KBE_LOGGER_INTERFACE_H

// common include	
#if defined(LOGGER)
#include "logger.h"
#endif
#include "logger_interface_macros.h"
#include "network/interface_defs.h"
//#define NDEBUG
// windows include	
#if KBE_PLATFORM == PLATFORM_WIN32
#else
// linux include
#endif
	
namespace KBEngine{

/**
	Logger������Ϣ�ӿ��ڴ˶���
*/
NETWORK_INTERFACE_DECLARE_BEGIN(LoggerInterface)
	// ĳappע���Լ��Ľӿڵ�ַ����app
	LOGGER_MESSAGE_DECLARE_ARGS11(onRegisterNewApp,							NETWORK_VARIABLE_MESSAGE,
									int32,									uid, 
									std::string,							username,
									COMPONENT_TYPE,							componentType, 
									COMPONENT_ID,							componentID, 
									COMPONENT_ORDER,						globalorderID,
									COMPONENT_ORDER,						grouporderID,
									uint32,									intaddr, 
									uint16,									intport,
									uint32,									extaddr, 
									uint16,									extport,
									std::string,							extAddrEx)

	// ĳapp��������look��
	LOGGER_MESSAGE_DECLARE_ARGS0(lookApp,									NETWORK_FIXED_MESSAGE)

	// ĳ��app����鿴��app����״̬��
	LOGGER_MESSAGE_DECLARE_ARGS0(queryLoad,									NETWORK_FIXED_MESSAGE)

	// ĳ��app��app��֪���ڻ״̬��
	LOGGER_MESSAGE_DECLARE_ARGS2(onAppActiveTick,							NETWORK_FIXED_MESSAGE,
									COMPONENT_TYPE,							componentType, 
									COMPONENT_ID,							componentID)

	// Զ��д��־
	LOGGER_MESSAGE_DECLARE_STREAM(writeLog,									NETWORK_VARIABLE_MESSAGE)

	// ע��log������
	LOGGER_MESSAGE_DECLARE_STREAM(registerLogWatcher,						NETWORK_VARIABLE_MESSAGE)

	// ע��log������
	LOGGER_MESSAGE_DECLARE_STREAM(deregisterLogWatcher,						NETWORK_VARIABLE_MESSAGE)

	// log�����߸����Լ�������
	LOGGER_MESSAGE_DECLARE_STREAM(updateLogWatcherSetting,					NETWORK_VARIABLE_MESSAGE)

	// ����رշ�����
	LOGGER_MESSAGE_DECLARE_STREAM(reqCloseServer,							NETWORK_VARIABLE_MESSAGE)

	// �����ѯwatcher����
	LOGGER_MESSAGE_DECLARE_STREAM(queryWatcher,								NETWORK_VARIABLE_MESSAGE)

	// ��ʼprofile
	LOGGER_MESSAGE_DECLARE_STREAM(startProfile,								NETWORK_VARIABLE_MESSAGE)

	// ����ǿ��ɱ����ǰapp
	LOGGER_MESSAGE_DECLARE_STREAM(reqKillServer,							NETWORK_VARIABLE_MESSAGE)

NETWORK_INTERFACE_DECLARE_END()

#ifdef DEFINE_IN_INTERFACE
	#undef DEFINE_IN_INTERFACE
#endif

}
#endif
