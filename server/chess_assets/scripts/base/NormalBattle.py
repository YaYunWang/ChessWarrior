# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import d_chess

class NormalBattle(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)
		# 创建cell
		self.createCellEntityInNewSpace(None)
		self.readyIndex = 0
		self.create_chess_index = 0

	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		# 开始创建棋子
		if userArg == 1:
			temp_chess_len = len(d_chess.test_chess2)
			if self.create_chess_index >= temp_chess_len:
				self.startRound(1)
				self.delTimer(1)
			else:
				param = d_chess.test_chess2[self.create_chess_index]

				if self.create_chess_index <= 15:
					param["chess_owner_player"] = 1
				else:
					param["chess_owner_player"] = 2

				temp = KBEngine.createEntityLocally("Chess", param)
				temp.CreateCell(self, self.cell)

				self.create_chess_index = self.create_chess_index + 1
	
	def nextRound(self):
		self.delTimer(self.state_id)
		if self.current_round_type == 1:
			self.startRound(2)
		else:
			self.startRound(1)

	def startRound(self, type):
		if self.player1 is None:
			INFO_MSG("start round , but player is none.")
			return
		if self.player2 is None:
			INFO_MSG("start round , but player is none.")
			return

		self.current_round_type = type
		self.player1.StartRound(type)
		self.player2.StartRound(type)

		self.state_id = self.addTimer(30, 30, 2)

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

	def onGetCell(self):
		if self.player1 is not None:
			self.player1.CreateCell(self, self.cell)
		if self.player2 is not None:
			self.player2.CreateCell(self, self.cell)

	def ClientReady(self, player):
		INFO_MSG("client ready.")
		self.readyIndex = self.readyIndex + 1
		if self.readyIndex >= 2:
			self.player1.CampTypeSet(1)
			self.player2.CampTypeSet(2)

			self.addTimer(0, 0.5, 1)

	def ChessMove(self, chess_id, index_x, index_z):
		chess = KBEngine.entities[chess_id]
		if chess is None:
			INFO_MSG("not find this chess %d" % (chess_id))
			return

		chess.Move(index_x, index_z)

		self.player1.Move(chess_id, index_x, index_z)
		self.player2.Move(chess_id, index_x, index_z)

		self.nextRound()

	def AttackChess(self, chess_id, be_chess_id, index_x, index_z):
		chess = KBEngine.entities[chess_id]
		if chess is None:
			INFO_MSG("not find this chess %d" % (chess_id))
			return

		be_chess = KBEngine.entities[be_chess_id]
		if chess is None:
			INFO_MSG("not find this be attack chess %d" % (be_chess_id))
			return

		chess.Move(index_x, index_z)
		self.player1.Attack(chess_id, be_chess_id)
		self.player2.Attack(chess_id, be_chess_id)

	def KillChess(self, chess_id):
		# 干掉这个棋子
		chess = KBEngine.entities[chess_id]
		if chess is None:
			INFO_MSG("not find this chess %d" % (chess_id))
			return

		self.nextRound()

	def ClientDeath(self):
		if self.cell is not None:
			self.destroyCellEntity()
