using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventTypes
{
        //====================场景切换事件=====================//
        /// <summary>
        /// The exit scene.
        /// </summary>
        ExitScene = 1,          
        /// <summary>
        /// The enter scene.
        /// </summary>
        EnterScene = 2,         
        /// <summary>
        /// The main UIReady.
        /// </summary>
        MainUIReady = 3,      

        EntityCreated,      // 有对象被创建 参数 BaseEntity

        EntityDestroy,      // 对象即将被销毁， 参数 BaseEntity

		KillChess,			// 杀死对象

		CustomEventEnd,
}
