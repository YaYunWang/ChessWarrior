

public abstract class ManagerTemplateBase<T> : MonoBehaviourSingleton<T> where T : MonoBehaviourSingleton<T>
{
	protected sealed override void OnCreateInstance()
	{
		InitManager();
	}

	protected abstract void InitManager();
}



