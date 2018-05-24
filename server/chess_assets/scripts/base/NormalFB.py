# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import d_chess

class NormalFB(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)

		# 创建cell
		self.createCellEntityInNewSpace(None)

		INFO_MSG("NormalFB is create.")

		self.create_chess_index = 0

	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		INFO_MSG("time %d " % (userArg))
		# 开始创建棋子
		if userArg == 1:
			temp_chess_len = len(d_chess.test_chess)
			INFO_MSG("========= %d   %d" % (self.create_chess_index, temp_chess_len))
			if self.create_chess_index >= temp_chess_len:
				INFO_MSG("creat chess finish...")
				self.startRound(1)
				self.delTimer(1)
			else:
				INFO_MSG("creat chess ")
				param = d_chess.test_chess[self.create_chess_index]

				temp = KBEngine.createEntityLocally("Chess", param)
				temp.CreateCell(self, self.cell)

				self.create_chess_index = self.create_chess_index + 1
		elif userArg == 2:
			# 强制走一步，这里暂时不做处理
			self.nextRound()

	def nextRound(self):
		self.delTimer(2)
		if self.current_round_type == 1:
			self.startRound(0)
		else:
			self.startRound(1)
	"""
		开启回合
		1 自己回合
		0 电脑回合
	"""
	def startRound(self, type):
		if self.player is None:
			INFO_MSG("start round , but player is none.")
			return

		INFO_MSG("round begin ....")
		self.current_round_type = type
		self.player.StartRound(type)

		self.addTimer(30, 30, 2)

	def onClientEnabled(self):
		"""
		KBEngine method.
		该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
		cell部分。
		"""
		INFO_MSG("NormalFB[%i] entities enable. entityCall:%s" % (self.id, self.client))
			
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
		DEBUG_MSG("NormalFB[%i].onClientDeath:" % self.id)
		self.destroy()

	def onGetCell(self):
		if self.player is not None:
			INFO_MSG("normal fb cell create. player is not none.")
		else:
			INFO_MSG("normal fb cell create. player is none.")

		# account 创建cell

		self.player.CreateCell(self, self.cell)

	def onLoseCell(self):
		self.destroy()

	def ClientDeath(self):
		self.destroyCellEntity()

	def ClientReady(self):
		INFO_MSG("client ready.")
		self.create_chess_index = 0
		self.addTimer(0, 0.5, 1)

	def ChessMove(self, chess_id, index_x, index_z):
		INFO_MSG("=== chess move index 2")

		chess = KBEngine.entities[chess_id]
		if chess is None:
			INFO_MSG("not find this chess %d" % (chess_id))
			return

		chess.Move(index_x, index_z)

		self.player.Move(chess_id, index_x, index_z)

		self.nextRound()