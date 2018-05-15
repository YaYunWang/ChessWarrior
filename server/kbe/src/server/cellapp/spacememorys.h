// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_SPACEMEMORYS_H
#define KBE_SPACEMEMORYS_H

#include "helper/debug_helper.h"
#include "common/common.h"
#include "common/singleton.h"
#include "updatable.h"
#include "spacememory.h"

namespace KBEngine{

class SpaceMemorys
{
public:
	SpaceMemorys();
	~SpaceMemorys();
	
	static void finalise();

	typedef std::map<SPACE_ID, KBEShared_ptr<SpaceMemory> > SPACEMEMORYS;

	/** 
		����һ���µ�space 
	*/
	static SpaceMemory* createNewSpace(SPACE_ID spaceID, const std::string& scriptModuleName);
	
	/**
		����һ��space
	*/
	static bool destroySpace(SPACE_ID spaceID, ENTITY_ID entityID);

	/** 
		Ѱ��һ��ָ��space 
	*/
	static SpaceMemory* findSpace(SPACE_ID spaceID);
	
	/** 
		�������е�space 
	*/
	static void update();

	static size_t size(){ return spaces_.size(); }

protected:
	static SPACEMEMORYS spaces_;
};

}
#endif
