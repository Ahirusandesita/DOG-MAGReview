using System;
using UnityEngine;

/// <summary>
/// Inspector�ŃV�[������ݒ肷��N���X
/// <br>- �K���ϐ����V���A���C�Y�����邱��</br>
/// </summary>
[Serializable]
public class RegisterSceneInInspector
{
    // Editor����̂݃A�N�Z�X������
    [SerializeField] private string name;
    [SerializeField] private int selectedIndex;

    /// <summary>
    /// �V�[����
    /// <br>- BuildSettings�ɓo�^����Ă���V�[�������v���_�E���œo�^</br>
    /// </summary>
    public string Name => name;
}
