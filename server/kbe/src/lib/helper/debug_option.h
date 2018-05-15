// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBE_DEBUG_OPTION_H
#define KBE_DEBUG_OPTION_H
#include "common/common.h"

namespace KBEngine{

namespace Network
{

/** 
	��������������ݰ��Ƿ�����Я��������Ϣ�� ������ĳЩǰ�˽������ʱ�ṩһЩ����
	 ���Ϊfalse��һЩ�̶����ȵ����ݰ���Я��������Ϣ�� �ɶԶ����н���
*/
extern bool g_packetAlwaysContainLength;

/**
�Ƿ���Ҫ���κν��պͷ��͵İ����ı������log���ṩ����
		g_trace_packet:
			0: �����
			1: 16�������
			2: �ַ������
			3: 10�������
		use_logfile:
			�Ƿ����һ��log�ļ�����¼�����ݣ��ļ���ͨ��Ϊ
			appname_packetlogs.log
		g_trace_packet_disables:
			�ر�ĳЩ�������
*/
extern uint8 g_trace_packet;
extern bool g_trace_encrypted_packet;
extern std::vector<std::string> g_trace_packet_disables;
extern bool g_trace_packet_use_logfile;

}

/**
	�Ƿ����entity�Ĵ����� �ű���ȡ���ԣ� ��ʼ�����Եȵ�����Ϣ��
*/
extern bool g_debugEntity;

/**
	apps����״̬, ���ڽű��л�ȡ��ֵ
		0 : debug
		1 : release
*/
extern int8 g_appPublish;

}

#endif // KBE_DEBUG_OPTION_H
