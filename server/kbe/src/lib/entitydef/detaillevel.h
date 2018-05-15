// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com


#ifndef KBE_DETAILLEVEL_H
#define KBE_DETAILLEVEL_H

#include "common/common.h"
#include "helper/debug_helper.h"
#include "pyscript/scriptobject.h"	

namespace KBEngine{

/** entity ���鼶�����Ͷ��� 
	Ĭ����3������ֱ�Ϊ:
	 ���� �У� Զ
*/
struct DetailLevel
{
	struct Level
	{
		Level():radius(FLT_MAX), hyst(1.0f){};
		float radius;
		float hyst;

		bool inLevel(float dist)
		{
			if(radius >= dist)
				return true;

			return false;
		}
	};
	
	DetailLevel()
	{
	}

	~DetailLevel()
	{
	}

	Level level[3]; // ���� �У� Զ
};

}


#endif // KBE_DETAILLEVEL_H

