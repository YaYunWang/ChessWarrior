# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *

class RoomMsg(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)

		KBEngine.globalData["RoomMsg"] = self

		self.players = []

		self.addTimer(5, 5, 1)

	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		if userArg == 1:
			while len(self.players) >= 2:
				player1 = self.players.pop()
				player2 = self.players.pop()

				param = {
					"player1" : player1,
					"player2" : player2,
				}

				KBEngine.createEntityLocally("NormalBattle", param)
		
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

	def StartMatch(self, account):
		self.players.append(account)

	def UnStartMatch(self, account):
		self.players.remove(account)
