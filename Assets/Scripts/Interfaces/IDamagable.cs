using UnityEngine;

/// <summary>
/// ��e�\�ȃC���^�[�t�F�[�X
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="teamInfo">�Ăяo�����̃`�[�����</param>
    /// <param name="damage">�󂯂�_���[�W��</param>
    /// <param name="impactDir">�ڐG�����I�u�W�F�N�g�̐i�s�x�N�g��
    /// <br>- Normalize���邱�Ƃ��]�܂���</br></param>
    void TakeDamage(TeamInfo teamInfo, int damage = 1, Vector2 impactDir = default);

    TeamInfo TeamInfo { get; }

    bool IsInvincible { get; }
}
