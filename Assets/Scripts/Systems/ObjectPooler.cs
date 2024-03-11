using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


/// <summary>
/// �I�u�W�F�N�g�v�[���𐶐�����N���X
/// </summary>
public class ObjectPooler : IReturnablePool, IGettablePool, IDisposable
{
    private PoolObject prefab = default;
    private GameObject parent = default;
    private Queue<PoolObject> objectPool = default;

    public IReadOnlyCollection<PoolObject> ObjectPool => objectPool;


    #region �R���X�g���N�^
    /// <summary>
    /// �I�u�W�F�N�g�v�[���𐶐�����R���X�g���N�^
    /// <br>- new�����i�K��Instantiate�����邽�ߒ���</br>
    /// <br>- new�̖߂�l��IGettablePool�C���^�[�t�F�[�X�Ŏ󂯎�邱��</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // �e�I�u�W�F�N�g�𐶐�
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // ��������
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPooler�N���X�̃C���X�^���X��Set����
            obj.IsInitialCreate = true;
            obj.Initialize();           // ���������I�u�W�F�N�g�̏������������Ăяo��
            objectPool.Enqueue(obj);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[���𐶐�����R���X�g���N�^
    /// <br>- new�����i�K��Instantiate�����邽�ߒ���</br>
    /// <br>- new�̖߂�l��IGettablePool�C���^�[�t�F�[�X�Ŏ󂯎�邱��</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, Action initializeAction, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // �e�I�u�W�F�N�g�𐶐�
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // ��������
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPooler�N���X�̃C���X�^���X��Set����
            obj.IsInitialCreate = true;
            obj.Initialize(initializeAction);           // ���������I�u�W�F�N�g�̏������������Ăяo��
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
        // �L���[�̒��g����ł���΁A�V���ɐ�������
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
        // �L���[�̒��g����ł���΁A�V���ɐ�������
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
/// �I�u�W�F�N�g�v�[���𐶐�����N���X
/// </summary>
public class ObjectPooler<T> : IReturnablePool, IGettablePool, IDisposable
{
    private PoolObject prefab = default;
    private GameObject parent = default;
    private Queue<PoolObject> objectPool = default;

    public IReadOnlyCollection<PoolObject> ObjectPool => objectPool;


    /// <summary>
    /// �I�u�W�F�N�g�v�[���𐶐�����R���X�g���N�^
    /// <br>- new�����i�K��Instantiate�����邽�ߒ���</br>
    /// <br>- new�̖߂�l��IGettablePool�C���^�[�t�F�[�X�Ŏ󂯎�邱��</br>
    /// </summary>
    public ObjectPooler(PoolObjectAsset createObjectData, Action<T> initializeAction, T t, string parentName = null)
    {
        prefab = createObjectData.Prefab;
        objectPool = new();

        // �e�I�u�W�F�N�g�𐶐�
        if (parentName is null)
        {
            parentName = "PooledObjects";
        }
        parent = new GameObject(parentName);

        // ��������
        for (int i = 0; i < createObjectData.MaxCreateCount; i++)
        {
            PoolObject obj = Object.Instantiate(prefab, parent.transform);
            obj.Pool = this;            // ObjectPooler�N���X�̃C���X�^���X��Set����
            obj.IsInitialCreate = true;
            obj.Initialize(initializeAction, t);           // ���������I�u�W�F�N�g�̏������������Ăяo��
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
        // �L���[�̒��g����ł���΁A�V���ɐ�������
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
        // �L���[�̒��g����ł���΁A�V���ɐ�������
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
