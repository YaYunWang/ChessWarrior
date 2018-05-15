// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_UPDATABLE_H
#define KBE_UPDATABLE_H

// common include
#include "helper/debug_helper.h"
#include "common/common.h"

// #define NDEBUG
// windows include	
#if KBE_PLATFORM == PLATFORM_WIN32	
#else
// linux include
#endif

namespace KBEngine{

/*
	��������һ�����ǻᱻ���µĶ��� appÿ��tick����������е�
	Updatable������״̬�� ��Ҫʵ�ֲ�ͬ��Updatable����ɲ�ͬ�ĸ������ԡ�
*/
class Updatable
{
public:
	Updatable();
	~Updatable();

	virtual bool update() = 0;

	virtual uint8 updatePriority() const {
		return 0;
	}

	std::string c_str() { return updatableName; }

	// ������Updatables�����е�λ��
	int removeIdx;

	std::string updatableName;
};

}
#endif
