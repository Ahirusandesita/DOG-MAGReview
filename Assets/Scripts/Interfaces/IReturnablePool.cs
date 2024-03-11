using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプールに返却可能なインターフェース
/// </summary>
public interface IReturnablePool
{
    /// <summary>
    /// オブジェクトをプールに返却する
    /// </summary>
    /// <param name="thisObj">自分自身</param>
    void Return(PoolObject thisObj);
}

/// <summary>
/// オブジェクトプールから取得可能なインターフェース
/// </summary>
public interface IGettablePool
{
    public IReadOnlyCollection<PoolObject> ObjectPool { get; }

    /// <summary>
    ///  プールからオブジェクトを取得する
    ///  <br>- 初期化：Auto</br>
    /// </summary>
    /// <param name="initialPos">初期位置</param>
    /// <param name="initialDir">初期角度</param>
    /// <returns>取得したオブジェクト</returns>
    PoolObject Get(Vector2 initialPos, Quaternion initialDir);

    /// <summary>
    ///  プールからオブジェクトを取得する
    ///  <br>- 初期化：Manual（Enableを手動で実行する必要がある）</br>
    /// </summary>
    /// <returns>取得したオブジェクト</returns>
    PoolObject Get();

    /// <summary>
    /// プールを削除する
    /// </summary>
    void Dispose();
}