# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import * 

class TChessInfo(list):
	"""
	"""
	def __init__(self):
		"""
		"""
		list.__init__(self)
		
	def asDict(self):
		data = {
			"id" : self[0],
			"name" : self[1],
			"level": self[2]
		}

		return data

	def createFromDict(self, dictData):
		self.extend([dictData["id"], dictData["name"], dictData["level"]])
		return self
		

class CHESS_INFO_PICKLER:
	def __init__(self):
		pass

	def createObjFromDict(self, dct):
		return TChessInfo().createFromDict(dct)

	def getDictFromObj(self, obj):
		return obj.asDict()

	def isSameType(self, obj):
		return isinstance(obj, TChessInfo)

inst = CHESS_INFO_PICKLER()