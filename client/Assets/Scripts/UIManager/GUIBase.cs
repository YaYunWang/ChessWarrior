using FairyGUI;
using System.Collections.Generic;

public enum EventListenerType
{
    onClick,
    onRightClick,
    onTouchBegin,
    onTouchMove,
    onTouchEnd,
    onRollOver,
    onRollOut,
    onAddedToStage,
    onRemovedFromStage,
    onKeyDown,
    onClickLink,
    onPositionChanged,
    onSizeChanged,
    onDragStart,
    onDragMove,
    onDragEnd,
    OnGearStop,
}

public class GUIBase : Window
{
    public string uiName = string.Empty;
    public string packageName = string.Empty;

    private GComponent viewComponent;
    private Dictionary<string, GObject> cacheObjs = new Dictionary<string, GObject>();
    
    protected virtual void OnCreate() { }

    protected virtual void OnStart() { }
    
    protected virtual void OnRegister() { }

    protected virtual void OnUnRegister() { }
    
    protected virtual void OnClose() { }

    protected virtual void OnDestroy() { }
    
    public void Start()
    {
        Show();
        OnStart();
        OnRegister();
    }

    public void Update()
    {
        OnUpdate();
    }
    
    public void Close()
    {
        OnUnRegister();
        OnClose();
        Hide();
    }

    public void Destory()
    {
        foreach (var item in cacheObjs)
        {
            if (item.Value is GButton)
                item.Value.RemoveEventListeners();
        }
        cacheObjs.Clear();
        cacheObjs = null;
        Close();
        OnDestroy();
        this.Dispose();
    }
    
    protected override void OnInit()
    {
        viewComponent = UIPackage.CreateObject(packageName, uiName).asCom;
        if (viewComponent == null)
            return;

        viewComponent.fairyBatching = true;
        
        this.contentPane = viewComponent;
        
        OnCreate();
    }

    protected GObject GetChildren(string name)
    {
        if (viewComponent == null)
            return null;

        return viewComponent.GetChild(name);
    }

    protected void SetEnable(string url, bool enable)
    {
        GObject child = GetChildren(url);
        if (child == null)
            return;

        child.enabled = enable;
    }
    
    protected void SetEventListener(string objname, EventListenerType listenerType, EventCallback1 callback)
    {
        GButton button = FindChildren<GButton>(objname);
        if (button == null)
            return;

        switch (listenerType)
        {
            case EventListenerType.onClick:
                button.onClick.Add(callback);
                break;
            case EventListenerType.onRightClick:
                button.onRightClick.Add(callback);
                break;
            case EventListenerType.onTouchBegin:
                button.onTouchBegin.Add(callback);
                break;
            case EventListenerType.onTouchMove:
                button.onTouchMove.Add(callback);
                break;
            case EventListenerType.onTouchEnd:
                button.onTouchEnd.Add(callback);
                break;
            case EventListenerType.onRollOver:
                button.onRollOver.Add(callback);
                break;
            case EventListenerType.onRollOut:
                button.onRollOut.Add(callback);
                break;
            case EventListenerType.onAddedToStage:
                button.onAddedToStage.Add(callback);
                break;
            case EventListenerType.onRemovedFromStage:
                button.onRemovedFromStage.Add(callback);
                break;
            case EventListenerType.onKeyDown:
                button.onKeyDown.Add(callback);
                break;
            case EventListenerType.onClickLink:
                button.onClickLink.Add(callback);
                break;
            case EventListenerType.onPositionChanged:
                button.onPositionChanged.Add(callback);
                break;
            case EventListenerType.onSizeChanged:
                button.onSizeChanged.Add(callback);
                break;
            case EventListenerType.onDragStart:
                button.onDragStart.Add(callback);
                break;
            case EventListenerType.onDragMove:
                button.onDragMove.Add(callback);
                break;
            case EventListenerType.onDragEnd:
                button.onDragEnd.Add(callback);
                break;
            case EventListenerType.OnGearStop:
                button.OnGearStop.Add(callback);
                break;
            default:
                button.onClick.Add(callback);
                break;
        }
    }

    protected GTextField SetText(string objname,string value)
    {
        GTextField textfield = FindChildren<GTextField>(objname);
        if (textfield != null)
            textfield.text = value;
        return textfield;
    }

    protected void SetVisible(string objname, bool visible)
    {
        GObject obj = GetChildren(objname);
        if (obj != null && obj.visible != visible)
            obj.visible = visible;
    }

    protected string GetTextInput(string objname)
    {
        GTextInput textinput = FindChildren<GTextInput>(objname);
        if (textinput != null)
            return textinput.text;
        return string.Empty;
    }

    protected void SetTextInput(string objname, string value)
    {
        GTextInput textinput = FindChildren<GTextInput>(objname);
        if (textinput != null)
            textinput.text = value;
    }

    protected T FindChildren<T>(string objname) where T : GObject
    {
        GObject obj;
        if (!cacheObjs.TryGetValue(objname, out obj))
        {
            obj = GetChildren(objname);
            cacheObjs.Add(objname, obj);
        }
        if (obj == null)
            return null;

        return obj as T;
    }
}