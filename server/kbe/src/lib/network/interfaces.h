// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_NETWORK_INTERFACES_H
#define KBE_NETWORK_INTERFACES_H

namespace KBEngine { 
namespace Network
{
class Channel;
class MessageHandler;

/** ����ӿ����ڽ�����ͨ��Network������Ϣ
*/
class InputNotificationHandler
{
public:
	virtual ~InputNotificationHandler() {};
	virtual int handleInputNotification(int fd) = 0;
};

/** ����ӿ����ڽ�����ͨ��Network�����Ϣ
*/
class OutputNotificationHandler
{
public:
	virtual ~OutputNotificationHandler() {};
	virtual int handleOutputNotification(int fd) = 0;
};

/** ����ӿ����ڽ���һ������ͨ����ʱ��Ϣ
*/
class ChannelTimeOutHandler
{
public:
	virtual void onChannelTimeOut(Channel * pChannel) = 0;
};

/** ����ӿ����ڽ���һ���ڲ�����ͨ��ȡ��ע��
*/
class ChannelDeregisterHandler
{
public:
	virtual void onChannelDeregister(Channel * pChannel) = 0;
};

/** ����ӿ����ڼ���NetworkStats�¼�
*/
class NetworkStatsHandler
{
public:
	virtual void onSendMessage(const MessageHandler& msgHandler, int size) = 0;
	virtual void onRecvMessage(const MessageHandler& msgHandler, int size) = 0;
};


}
}

#endif // KBE_NETWORK_INTERFACES_H
