# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *

class Chess(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)

		INFO_MSG("chess cell is create: %d" % (self.chess_id))