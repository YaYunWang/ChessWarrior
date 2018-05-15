// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_COORDINATE_SYSTEM_H
#define KBE_COORDINATE_SYSTEM_H

#include "helper/debug_helper.h"
#include "common/common.h"	

//#define DEBUG_COORDINATE_SYSTEM

namespace KBEngine{

class CoordinateNode;

class CoordinateSystem
{
public:
	CoordinateSystem();
	~CoordinateSystem();

	/**
		��list�в���ڵ�
	*/
	bool insert(CoordinateNode* pNode);

	/**
		���ڵ��list���Ƴ�
	*/
	bool remove(CoordinateNode* pNode);
	bool removeReal(CoordinateNode* pNode);
	void removeDelNodes();
	void releaseNodes();

	/**
		��ĳ���ڵ��б䶯ʱ����Ҫ��������list�е�
		���λ�õ���Ϣ
	*/
	void update(CoordinateNode* pNode);

	/**
		�ƶ��ڵ�
	*/
	void moveNodeX(CoordinateNode* pNode, float px, CoordinateNode* pCurrNode);
	void moveNodeY(CoordinateNode* pNode, float py, CoordinateNode* pCurrNode);
	void moveNodeZ(CoordinateNode* pNode, float pz, CoordinateNode* pCurrNode);

	INLINE CoordinateNode * pFirstXNode() const;
	INLINE CoordinateNode * pFirstYNode() const;
	INLINE CoordinateNode * pFirstZNode() const;

	INLINE bool isEmpty() const;

	INLINE uint32 size() const;

	static bool hasY;

	INLINE void incUpdating();
	INLINE void decUpdating();

private:
	uint32 size_;

	// �������βָ��
	CoordinateNode* first_x_coordinateNode_;
	CoordinateNode* first_y_coordinateNode_;
	CoordinateNode* first_z_coordinateNode_;

	std::list<CoordinateNode*> dels_;
	size_t dels_count_;

	int updating_;

	std::list<CoordinateNode*> releases_;
};

}

#ifdef CODE_INLINE
#include "coordinate_system.inl"
#endif
#endif
