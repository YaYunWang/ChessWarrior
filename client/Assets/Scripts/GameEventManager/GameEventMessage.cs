using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventTypes
{
        // 连接到服务器
        EngineConnectedToServer = 1,
        // 从服务器断开
        EngineDisconnectedFromServer = 2,
        // 连接服务器失败
        EngineConnectServerFailed = 3,

        //=======================引擎网络消息====================//
        /// <summary>
        /// The engine login success.
        /// </summary>
        EngineLoginSuccess = 10,                     // LoginSuccessMsg msg
        /// <summary>
        /// The engine declare property table.
        /// </summary>
        EngineDeclarePropertyTable = 11,             // DeclareProperyTableMsg
        /// <summary>
        /// The engine declare record table.
        /// </summary>
        EngineDeclareRecordTable = 12,               // DeclareRecordTableMsg
        /// <summary>
        /// The engine enter scene.
        /// </summary>
        EngineEnterScene = 13,                       // EnterSceneMsg
        /// <summary>
        /// The engine add scene object.
        /// </summary>
        EngineAddSceneObject = 14,                   // AddSceneObjectMsg
        /// <summary>
        /// The engine remove scene object.
        /// </summary>
        EngineRemoveSceneObject = 15,                // RemoveSceneObjectMsg
        /// <summary>
        /// The engine set scene property.
        /// </summary>
        EngineSetSceneProperty = 16,                 // SetScenePropertyMsg
        /// <summary>
        /// The engine set object property.
        /// </summary>
        EngineSetObjectProperty = 17,                
        /// <summary>
        /// The engine create view.
        /// </summary>
        EngineCreateView = 18,
        /// <summary>
        /// The engine delete view.
        /// </summary>
        EngineDeleteView = 19,
        /// <summary>
        /// The engine set view propery.
        /// </summary>
        EngineSetViewPropery = 20,
        /// <summary>
        /// The engine view add.
        /// </summary>
        EngineViewAdd = 21,
        /// <summary>
        /// The engine view remove.
        /// </summary>
        EngineViewRemove = 22,
        /// <summary>
        /// The engine view change.
        /// </summary>
        EngineViewChange = 23,
        /// <summary>
        /// The engine record add row.
        /// </summary>
        EngineRecordAddRow = 24,
        /// <summary>
        /// The engine record remove row.
        /// </summary>
        EngineRecordRemoveRow = 25,
        /// <summary>
        /// The engine record set value.
        /// </summary>
        EngineRecordSetValue = 26,
        /// <summary>
        /// The engine record clear.
        /// </summary>
        EngineRecordClear = 27,
        /// <summary>
        /// The engine server custom.
        /// </summary>
        EngineServerCustom = 28,       
        /// <summary>
        /// The engine relocate.
        /// </summary>
        EngineRelocate = 29,
        /// <summary>
        /// The engine move.
        /// </summary>
        EngineMove = 30,
        /// <summary>
        /// The engine multi move.
        /// </summary>
        EngineMultiMove = 31,
        /// <summary>
        /// The engine set multi object property.
        /// </summary>
        EngineSetMultiObjectProp = 32,
        /// <summary>
        /// The engine add multi scene object.
        /// </summary>
        EngineAddMultiSceneObject = 33,
        /// <summary>
        /// The engine remove multi scene object.
        /// </summary>
        EngineRemoveMultiSceneObject = 34,
        /// <summary>
        /// The engine server error code.
        /// </summary>
        EngineServerErrorCode = 35,                  // ServerErrorCodeMsg
        /// <summary>
        /// The engine idle.
        /// </summary>
        EngineIdle = 36,
        /// <summary>
        /// The engine login queue.
        /// </summary>
        EngineLoginQueue = 37,
        /// <summary>
        /// The engine set login string.
        /// </summary>
        EngineSetLoginString = 38,                   // LoginStringMsg

        //================引擎压缩消息事件====================//

        /// <summary>
        /// The engine cp add scene object.
        /// </summary>
        EngineCpAddSceneObject = 39,
        /// <summary>
        /// The engine cp record add row.
        /// </summary>
        EngineCpRecordAddRow = 40,
        /// <summary>
        /// The engine cp view add.
        /// </summary>
        EngineCpViewAdd = 41,
        /// <summary>
        /// The engine cp server custom.
        /// </summary>
        EngineCpServerCustom = 42,
        /// <summary>
        /// The engine cp multi move.
        /// </summary>
        EngineCpMultiMove = 43,
        /// <summary>
        /// The engine cp set multi object property.
        /// </summary>
        EngineCpSetMultiObjectProp = 44,
        /// <summary>
        /// The engine cp add multi scene object.
        /// </summary>
        EngineCpAddMultiSceneObject = 45,

        ///////////  //////////


        //====================场景切换事件=====================//
        /// <summary>
        /// The exit scene.
        /// </summary>
        ExitScene = 51,          
        /// <summary>
        /// The enter scene.
        /// </summary>
        EnterScene = 52,         
        /// <summary>
        /// The main UIReady.
        /// </summary>
        MainUIReady = 53,      

        //=======================客户端自定义事件====================//

        CustomEventStart = 100,

        EntityCreated,      // 有对象被创建 参数 BaseEntity

        EntityDestroy,      // 对象即将被销毁， 参数 BaseEntity

	CustomEventEnd,
        //========================================================//
}
