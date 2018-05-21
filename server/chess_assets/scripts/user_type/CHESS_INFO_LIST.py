# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import * 

class TChessInfoList(dict):
	"""
	"""
	def __init__(self):
		"""
		"""
		dict.__init__(self)
		
	def asDict(self):
		datas = []
		dct = {"values" : datas}

		for key, val in self.items():
			datas.append(val)
			
		return dct

	def createFromDict(self, dictData):
		for data in dictData["values"]:
			self[data[0]] = data
		return self
		
class CHESS_INFO_LIST_PICKLER:
	def __init__(self):
		pass

	def createObjFromDict(self, dct):
		return TChessInfoList().createFromDict(dct)

	def getDictFromObj(self, obj):
		return obj.asDict()

	def isSameType(self, obj):
		return isinstance(obj, TChessInfoList)

chess_list_inst = CHESS_INFO_LIST_PICKLER()