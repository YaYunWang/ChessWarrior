using System;

public interface IPooledObjSupporter : IDisposable
{
    void Reset();
}
