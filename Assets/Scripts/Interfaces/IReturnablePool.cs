using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�v�[���ɕԋp�\�ȃC���^�[�t�F�[�X
/// </summary>
public interface IReturnablePool
{
    /// <summary>
    /// �I�u�W�F�N�g���v�[���ɕԋp����
    /// </summary>
    /// <param name="thisObj">�������g</param>
    void Return(PoolObject thisObj);
}

/// <summary>
/// �I�u�W�F�N�g�v�[������擾�\�ȃC���^�[�t�F�[�X
/// </summary>
public interface IGettablePool
{
    public IReadOnlyCollection<PoolObject> ObjectPool { get; }

    /// <summary>
    ///  �v�[������I�u�W�F�N�g���擾����
    ///  <br>- �������FAuto</br>
    /// </summary>
    /// <param name="initialPos">�����ʒu</param>
    /// <param name="initialDir">�����p�x</param>
    /// <returns>�擾�����I�u�W�F�N�g</returns>
    PoolObject Get(Vector2 initialPos, Quaternion initialDir);

    /// <summary>
    ///  �v�[������I�u�W�F�N�g���擾����
    ///  <br>- �������FManual�iEnable���蓮�Ŏ��s����K�v������j</br>
    /// </summary>
    /// <returns>�擾�����I�u�W�F�N�g</returns>
    PoolObject Get();

    /// <summary>
    /// �v�[�����폜����
    /// </summary>
    void Dispose();
}