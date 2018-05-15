// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_INTERFACES_TASKS_H
#define KBE_INTERFACES_TASKS_H

// common include	
#include "common/common.h"
#include "common/memorystream.h"
#include "thread/threadtask.h"
#include "helper/debug_helper.h"
#include "network/address.h"
#include "server/server_errors.h"

namespace KBEngine{ 

class Orders;

#define INTERFACES_TASK_UNKNOWN 0
#define INTERFACES_TASK_CREATEACCOUNT 1
#define INTERFACES_TASK_LOGIN 2
#define INTERFACES_TASK_CHARGE 3

class InterfacesTask
{
public:
	InterfacesTask();
	virtual ~InterfacesTask();
	
	std::string commitName;			// �ύʱ�õ�����
	std::string accountName;		// ����Ϸ���������ݿ�����account�󶨵�����
	std::string password;			// ����
	std::string postDatas;			// �ύ�ĸ�������
	std::string getDatas;			// ���ظ��ͻ��˵ĸ�������
	COMPONENT_ID baseappID;
	COMPONENT_ID dbmgrID;
	SERVER_ERROR_CODE retcode;

	Network::Address address;

	bool enable;
};

class CreateAccountTask : public InterfacesTask
{
public:
	CreateAccountTask();
	virtual ~CreateAccountTask();
	
	virtual uint8 type(){ return INTERFACES_TASK_CREATEACCOUNT; }

protected:
};

class LoginAccountTask : public CreateAccountTask
{
public:
	LoginAccountTask();
	virtual ~LoginAccountTask();
	
	virtual uint8 type(){ return INTERFACES_TASK_LOGIN; }

protected:
};

class ChargeTask : public InterfacesTask
{
public:
	ChargeTask();
	virtual ~ChargeTask();
	
	virtual uint8 type(){ return INTERFACES_TASK_CHARGE; }

	OrdersCharge* pOrders;
	OrdersCharge orders;
	SERVER_ERROR_CODE retcode;
};


}

#endif // KBE_INTERFACES_TASKS_H
