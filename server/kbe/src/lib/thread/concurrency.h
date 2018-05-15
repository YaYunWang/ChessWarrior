// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com



#ifndef KBE_CONCURENCY_H
#define KBE_CONCURENCY_H

#include "common/platform.h"
#include "helper/debug_helper.h"
namespace KBEngine{

extern void (*pMainThreadIdleStartCallback)();
extern void (*pMainThreadIdleEndCallback)();

namespace KBEConcurrency
{

/**
	���̴߳��ڿ���ʱ����
*/
inline void onStartMainThreadIdling()
{
	if(pMainThreadIdleStartCallback)
		(*pMainThreadIdleStartCallback)();
}

/**
	���߳̽������п�ʼ��æʱ����
*/
inline void onEndMainThreadIdling()
{
	if(pMainThreadIdleEndCallback)
		(*pMainThreadIdleEndCallback)();
}

/**
	���ûص�����
	���ص�����ʱ֪ͨ����
*/
inline void setMainThreadIdleCallbacks(void (*pStartCallback)(), void (*pEndCallback)())
{
	pMainThreadIdleStartCallback = pStartCallback;
	pMainThreadIdleEndCallback = pEndCallback;
}

}

}

#endif // KBE_CONCURENCY_H
