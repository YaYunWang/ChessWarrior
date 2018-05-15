// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_RANGE_TRIGGER_H
#define KBE_RANGE_TRIGGER_H

#include "coordinate_node.h"
#include "helper/debug_helper.h"
#include "common/common.h"	

namespace KBEngine{

class RangeTriggerNode;
class RangeTrigger
{
public:
	RangeTrigger(CoordinateNode* origin, float xz, float y);
	virtual ~RangeTrigger();

	bool install();
	bool uninstall();
	bool reinstall(CoordinateNode* pCoordinateNode);
	INLINE bool isInstalled() const;

	INLINE void range(float xz, float y);
	INLINE float range_xz() const;
	INLINE float range_y() const;

	INLINE CoordinateNode* origin() const;
	INLINE void origin(CoordinateNode* pCoordinateNode);

	/**
		���·�Χ����
	*/
	virtual void update(float xz, float y);

	/**
		ĳ���ڵ��������뿪��rangeTrigger
	*/
	virtual void onEnter(CoordinateNode * pNode) = 0;
	virtual void onLeave(CoordinateNode * pNode) = 0;

	/**
		ĳ���ڵ�䶯�����˱��ڵ�
		@isfront: ��ǰ�ƶ���������ƶ�
	*/
	virtual void onNodePassX(RangeTriggerNode* pRangeTriggerNode, CoordinateNode* pNode, bool isfront);
	virtual void onNodePassY(RangeTriggerNode* pRangeTriggerNode, CoordinateNode* pNode, bool isfront);
	virtual void onNodePassZ(RangeTriggerNode* pRangeTriggerNode, CoordinateNode* pNode, bool isfront);

protected:
	float range_xz_, range_y_;

	CoordinateNode* origin_;

	RangeTriggerNode* positiveBoundary_;
	RangeTriggerNode* negativeBoundary_;

	bool removing_;
};

}

#ifdef CODE_INLINE
#include "range_trigger.inl"
#endif
#endif
