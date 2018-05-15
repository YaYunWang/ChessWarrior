// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_FORWARD_MESSAGEBUFFER_H
#define KBE_FORWARD_MESSAGEBUFFER_H

#include "common/common.h"
#include "common/tasks.h"
#include "common/singleton.h"
#include "helper/debug_helper.h"
#include "server/components.h"
#include "network/bundle.h"

namespace KBEngine { 
namespace Network
{
class Bundle;
class NetworkInterface;
class EventDispatcher;
}

/*
	�����app��û���ҵ��κ�cellapp����baseapp���ģ�齫һЩ��Ϣ���������� 
	�ȴ����µ�cellapp����baseapp������ʼ��ָ��ת����
*/


/*
	��һ����Ϣ���ɹ�ת����������handler����ʣ�������
	��Ҫ��дprocess
*/
class ForwardMessageOverHandler
{
public:
	virtual void process() = 0;
	virtual ~ForwardMessageOverHandler(){}
};

class ForwardItem
{
public:
	ForwardItem():
		pBundle(NULL),
		pHandler(NULL)
	{
	}

	virtual ~ForwardItem()
	{
	}

	Network::Bundle* pBundle;
	ForwardMessageOverHandler* pHandler;

	virtual bool isOK(){
		return true;
	}
};

/*
	ת��������Ϣ��ָ�������
*/
class ForwardComponent_MessageBuffer : public Task, 
						public Singleton<ForwardComponent_MessageBuffer>
{
public:
	ForwardComponent_MessageBuffer(Network::NetworkInterface & networkInterface);
	virtual ~ForwardComponent_MessageBuffer();

	Network::EventDispatcher & dispatcher();

	void push(COMPONENT_ID componentID, ForwardItem* pHandler);
	
	bool process();

	virtual void clear();
	
private:
	Network::NetworkInterface & networkInterface_;
	bool start_;
	
	typedef std::vector<ForwardItem*> MSGMAP_ITEM;
	typedef std::map<COMPONENT_ID, MSGMAP_ITEM> MSGMAP;
	MSGMAP pMap_;

};

/*
	ת��������Ϣ��ͬ�������������
*/
class ForwardAnywhere_MessageBuffer : public Task, 
						public Singleton<ForwardAnywhere_MessageBuffer>
{
public:
	ForwardAnywhere_MessageBuffer(Network::NetworkInterface & networkInterface, COMPONENT_TYPE forwardComponentType);
	virtual ~ForwardAnywhere_MessageBuffer();

	Network::EventDispatcher & dispatcher();

	void push(ForwardItem* pHandler);
	
	bool process();

	virtual void clear();
	
private:
	Network::NetworkInterface & networkInterface_;
	COMPONENT_TYPE forwardComponentType_;
	bool start_;
	
	std::vector<ForwardItem*> pBundles_;

};

}

#endif // KBE_FORWARD_MESSAGEBUFFER_H
