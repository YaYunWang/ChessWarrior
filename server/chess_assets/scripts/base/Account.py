# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from CHESS_INFO import TChessInfo
from CHESS_INFO_LIST import TChessInfoList
import d_chess

class Account(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)

		#dataDict = {"id":1, "name":"test", "level":1}
		#self.MineChess = TChessInfo().createFromDict(dataDict)

	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		DEBUG_MSG(id, userArg)
		
	def onClientEnabled(self):
		"""
		KBEngine method.
		该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
		cell部分。
		"""
		INFO_MSG("account[%i] entities enable. entityCall:%s" % (self.id, self.client))
			
	def onLogOnAttempt(self, ip, port, password):
		"""
		KBEngine method.
		客户端登陆失败时会回调到这里
		"""
		INFO_MSG(ip, port, password)
		return KBEngine.LOG_ON_ACCEPT
		
	def onClientDeath(self):
		"""
		KBEngine method.
		客户端对应实体已经销毁
		"""
		DEBUG_MSG("Account[%i].onClientDeath:" % self.id)
		self.destroy()

	def ReCreateAccountRequest(self, role_type, role_name):
		self.RoleType = role_type
		self.RoleName = role_name
		
		for data in d_chess.born_chess:
			dataDict = {"id":data, "name":d_chess.data[data]["name"], "level":1}
			chess = TChessInfo().createFromDict(dataDict)

			self.MineChess[data] = chess
			
		self.client.ReNameResult(1)

		DEBUG_MSG("re name %s %s" % (self.RoleName, role_name))