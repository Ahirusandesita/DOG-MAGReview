using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


/// <summary>
/// オブジェクトプールを生成するクラス
/// </summary>
public class ObjectPooler : IReturnablePool, IGettablePool, IDisposable
{
    private PoolObject prefab = default;
    private GameObject parent = default;
    private Queue<PoolObject> objectPool = default;

    public IReadOnlyCollection<PoolObject> ObjectPool => objectPool;


    #region コンストラクタ
    /// <summary>
    /// オブジェクトプールを生成するコンストラクタ
    /// <br>- newした段階でInstantiateが走るため注意</br>
    /// <br>- newの戻り値はIGettablePoolインターフェースで受け取ること</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // 親オブジェクトを生成
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // 初期生成
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPoolerクラスのインスタンスをSetする
            obj.IsInitialCreate = true;
            obj.Initialize();           // 生成したオブジェクトの初期化処理を呼び出す
            objectPool.Enqueue(obj);
        }
    }

    /// <summary>
    /// オブジェクトプールを生成するコンストラクタ
    /// <br>- newした段階でInstantiateが走るため注意</br>
    /// <br>- newの戻り値はIGettablePoolインターフェースで受け取ること</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, Action initializeAction, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // 親オブジェクトを生成
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // 初期生成
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPoolerクラスのインスタンスをSetする
            obj.IsInitialCreate = true;
            obj.Initialize(initializeAction);           // 生成したオブジェクトの初期化処理を呼び出す
            objectPool.Enqueue(obj);
        }
    }
    #endregion


    public PoolObject Get(Vector2 initialPos, Quaternion initialDir)
    {
        PoolObject obj;

        if (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        // キューの中身が空であれば、新たに生成する
        else
        {
            obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;
        }

        obj.Enable(initialPos, initialDir);
        return obj;
    }

    public PoolObject Get()
    {
        PoolObject obj;

        if (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        // キューの中身が空であれば、新たに生成する
        else
        {
            obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;
        }

        return obj;
    }

    public void Return(PoolObject thisObj)
    {
        thisObj.Disable();
        objectPool.Enqueue(thisObj);
    }

    public void Dispose()
    {
        Object.Destroy(parent);

        objectPool.Clear();
        objectPool.TrimExcess();

        prefab = null;
        parent = null;
        objectPool = null;
    }
}


/// <summary>
/// オブジェクトプールを生成するクラス
/// </summary>
public class ObjectPooler<T> : IReturnablePool, IGettablePool, IDisposable
{
    private PoolObject prefab = default;
    private GameObject parent = default;
    private Queue<PoolObject> objectPool = default;

    public IReadOnlyCollection<PoolObject> ObjectPool => objectPool;


    /// <summary>
    /// オブジェクトプールを生成するコンストラクタ
    /// <br>- newした段階でInstantiateが走るため注意</br>
    /// <br>- newの戻り値はIGettablePoolインターフェースで受け取ること</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, Action<T> initializeAction, T t, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // 親オブジェクトを生成
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // 初期生成
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPoolerクラスのインスタンスをSetする
            obj.IsInitialCreate = true;
            obj.Initialize(initializeAction, t);           // 生成したオブジェクトの初期化処理を呼び出す
            objectPool.Enqueue(obj);
        }
    }


    public PoolObject Get(Vector2 initialPos, Quaternion initialDir)
    {
        PoolObject obj;

        if (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        // キューの中身が空であれば、新たに生成する
        else
        {
            obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;
        }

        obj.Enable(initialPos, initialDir);
        return obj;
    }

    public PoolObject Get()
    {
        PoolObject obj;

        if (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        // キューの中身が空であれば、新たに生成する
        else
        {
            obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;
        }

        return obj;
    }

    public void Return(PoolObject thisObj)
    {
        thisObj.Disable();
        objectPool.Enqueue(thisObj);
    }

    public void Dispose()
    {
        Object.Destroy(parent);

        objectPool.Clear();
        objectPool.TrimExcess();

        prefab = null;
        parent = null;
        objectPool = null;
    }
}
