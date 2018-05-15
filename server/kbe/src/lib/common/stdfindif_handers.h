// Copyright 2008-2018 Yolo Technologies, Inc. All Rights Reserved. https://www.comblockengine.com

#ifndef KBE_STDFINDIF_HANDERS_H
#define KBE_STDFINDIF_HANDERS_H

#include "common/platform.h"

namespace KBEngine{

// vector<string>֮�������ʹ�� std::find_if �������Ƿ����ĳ���ַ���
template<typename T>
class find_vec_string_exist_handle
{
public:
	find_vec_string_exist_handle(const std::basic_string< T >& str)
	: str_(str) {}

	bool operator()(const std::basic_string< T > &strSrc)
	{
		return strSrc == str_;
	}

	bool operator()(const T* strSrc)
	{
		return strSrc == str_;
	}

private:
	std::basic_string< T > str_;
};


// vector<obj*>֮�������ʹ�� std::find_if �������Ƿ����ĳ������
template<typename T>
class findif_vector_obj_exist_handler
{
public:
	findif_vector_obj_exist_handler(T obj)
	: obj_(obj) {}

	bool operator()(const T &obj)
	{
		return obj == obj_;
	}

	bool operator()(const T* obj)
	{
		return obj == obj_;
	}

private:
	T obj_;
};

}

#endif // KBE_STDFINDIF_HANDERS_H
