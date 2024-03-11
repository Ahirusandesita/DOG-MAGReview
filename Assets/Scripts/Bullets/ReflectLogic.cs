using UnityEngine;
using VContainer;

/// <summary>
/// ���˂̋������v�Z����N���X
/// </summary>
public class ReflectLogic : IReflectable
{
    private readonly IPositionAdapter positionAdapter;
    private readonly IRotationAdapter rotationAdapter;

    private readonly (int x, int y) topRight = (1, 1);
    private readonly (int x, int y) topLeft = (-1, 1);
    private readonly (int x, int y) bottomRight = (1, -1);
    private readonly (int x, int y) bottomLeft = (-1, -1);


    [Inject]
    public ReflectLogic(IPositionAdapter positionAdapter, IRotationAdapter rotationAdapter)
    {
        this.positionAdapter = positionAdapter;
        this.rotationAdapter = rotationAdapter;
    }

    public bool Reflect(Vector2 inDirection, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // target��x���Ay�����ꂼ��̕ӂ̔����̃x�N�g�����Z�o
        // ��]���l�����邽�߁A�@���x�N�g�����|����
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // target�̉E��̍��W
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // target�̍����̍��W
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // �utarget�̉E��̍��W�v���猩���AHit�������W�i������щ���-1�A�E����я��1�ƕ\���j
        (int x, int y) signOfHitPosFromTopRight;
        // �utarget�̍����̍��W�v���猩���AHit�������W
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hit�������W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(positionAdapter.Position);
        // target�̉E��̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // target�̍����̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1���������A1���E�����Ƃ���int�ŕ\���������[�J�����W���Z�o
        // �܂�E��܂��͍������猩�āAHit���W�͂ǂ̕�����
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // �@�@�@�@�@�@�@�@�@�@�@�����������������������������������@��target�̉E��̍��W
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@Square�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // target�̍����̍��W���@����������������������������������


        // �Z�o����target�̖@���x�N�g��
        Vector2 targetNormal = default;

        // ��ʁiHit���W�́Atarget�̉E����W���猩��[����̕���] AND target�̍������W���猩��[�E��̕���]�j
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // �E��
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // �E��p
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = (targetTransform.up + targetTransform.right).normalized;
        }
        // �E���p
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = (-targetTransform.up + targetTransform.right).normalized;
        }
        // �����p
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            targetNormal = (-targetTransform.up + -targetTransform.right).normalized;
        }
        // ����p
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = (targetTransform.up + -targetTransform.right).normalized;
        }


        // ���ˊp�Ɩ@���x�N�g���̊p�̑傫�����Z�o
        float inDirAngleSize = Mathf.Abs(Vector2.Angle(inDirection, targetNormal));

        // 2�̃x�N�g���̊p�x�̑傫���̐�Βl���A0���ȏ�90���ȉ��̂Ƃ��A�d�����˂Ɣ��肵�������I��
        if (inDirAngleSize >= 0 && inDirAngleSize <= 90)
        {
            return false;
        }


        // ���ˊp�Ɩ@���x�N�g������A���ˊp���Z�o
        Vector2 reflectDir = Vector2.Reflect(inDirection, targetNormal);

        // ���ˊp�̕���������
        rotationAdapter.RotateInVectorDirection(reflectDir);
        return true;
    }
}

/// <summary>
/// �ȈՓI�Ȕ��ˌv�Z�N���X
/// </summary>
public class SimpleReflectLogic
{
    private readonly (int x, int y) topRight = (1, 1);
    private readonly (int x, int y) topLeft = (-1, 1);
    private readonly (int x, int y) bottomRight = (1, -1);
    private readonly (int x, int y) bottomLeft = (-1, -1);


    /// <summary>
    /// ���������I�u�W�F�N�g�̖@�����擾����i���������I�u�W�F�N�g�������̂Ƃ��j
    /// </summary>
    public Vector2 GetNormal(Vector2 origin, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // target��x���Ay�����ꂼ��̕ӂ̔����̃x�N�g�����Z�o
        // ��]���l�����邽�߁A�@���x�N�g�����|����
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // target�̉E��̍��W
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // target�̍����̍��W
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // �utarget�̉E��̍��W�v���猩���AHit�������W�i������щ���-1�A�E����я��1�ƕ\���j
        (int x, int y) signOfHitPosFromTopRight;
        // �utarget�̍����̍��W�v���猩���AHit�������W
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hit�������W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(origin);
        // target�̉E��̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // target�̍����̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1���������A1���E�����Ƃ���int�ŕ\���������[�J�����W���Z�o
        // �܂�E��܂��͍������猩�āAHit���W�͂ǂ̕�����
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // �@�@�@�@�@�@�@�@�@�@�@�����������������������������������@��target�̉E��̍��W
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@Square�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // target�̍����̍��W���@����������������������������������


        // �Z�o����target�̖@���x�N�g��
        Vector2 targetNormal = default;

        // ��ʁiHit���W�́Atarget�̉E����W���猩��[����̕���] AND target�̍������W���猩��[�E��̕���]�j
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // �E��
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // �E��p
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = (targetTransform.up + targetTransform.right).normalized;
        }
        // �E���p
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = (-targetTransform.up + targetTransform.right).normalized;
        }
        // �����p
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            targetNormal = (-targetTransform.up + -targetTransform.right).normalized;
        }
        // ����p
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = (targetTransform.up + -targetTransform.right).normalized;
        }

        return targetNormal;
    }


    /// <summary>
    /// ���������I�u�W�F�N�g�̖@�����擾����i���������I�u�W�F�N�g���P�̂̂Ƃ��j
    /// </summary>
    public Vector2 GetNormal(Vector2 origin, Vector2 inDirection, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // target��x���Ay�����ꂼ��̕ӂ̔����̃x�N�g�����Z�o
        // ��]���l�����邽�߁A�@���x�N�g�����|����
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // target�̉E��̍��W
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // target�̍����̍��W
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // �utarget�̉E��̍��W�v���猩���AHit�������W�i������щ���-1�A�E����я��1�ƕ\���j
        (int x, int y) signOfHitPosFromTopRight;
        // �utarget�̍����̍��W�v���猩���AHit�������W
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hit�������W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(origin);
        // target�̉E��̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // target�̍����̍��W���Atarget���猩�����[�J�����W�ɕϊ�
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1���������A1���E�����Ƃ���int�ŕ\���������[�J�����W���Z�o
        // �܂�E��܂��͍������猩�āAHit���W�͂ǂ̕�����
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // target�̉E��̍��W���猩��Hit���W�̋����i�p�v�Z�Ŏg�p�j
        Vector2 distancePosFromTopRight;

        distancePosFromTopRight.x = Mathf.Abs(hitPosLocal.x - targetTopRightPosLocal.x);
        distancePosFromTopRight.y = Mathf.Abs(hitPosLocal.y - targetTopRightPosLocal.y);

        // target�̍����̍��W���猩��Hit���W�̋����i�p�v�Z�Ŏg�p�j
        Vector2 distancePosFromBottomLeft;

        distancePosFromBottomLeft.x = Mathf.Abs(hitPosLocal.x - targetBottomLeftPosLocal.x);
        distancePosFromBottomLeft.y = Mathf.Abs(hitPosLocal.y - targetBottomLeftPosLocal.y);



        // �@�@�@�@�@�@�@�@�@�@�@�����������������������������������@��target�̉E��̍��W
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@Square�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // �@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b
        // target�̍����̍��W���@����������������������������������


        // �Z�o����target�̖@���x�N�g��
        Vector2 targetNormal = default;

        // ��ʁiHit���W�́Atarget�̉E����W���猩��[����̕���] AND target�̍������W���猩��[�E��̕���]�j
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // �E��
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // ����
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // �E��p
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            // �p�ɓ������Ă���Ƃ��A2�ʂ̂����������߂����̖ʂ̖@����Ԃ�
            if (distancePosFromTopRight.x < distancePosFromTopRight.y)
            {
                targetNormal = targetTransform.up;
            }
            else
            {
                targetNormal = targetTransform.right;
            }
        }
        // �E���p
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            if (distancePosFromTopRight.x < distancePosFromBottomLeft.y)
            {
                targetNormal = -targetTransform.up;
            }
            else
            {
                targetNormal = targetTransform.right;
            }
        }
        // �����p
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            if (distancePosFromBottomLeft.x < distancePosFromBottomLeft.y)
            {
                targetNormal = -targetTransform.up;
            }
            else
            {
                targetNormal = -targetTransform.right;
            }
        }
        // ����p
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            if (distancePosFromBottomLeft.x < distancePosFromTopRight.y)
            {
                targetNormal = targetTransform.up;
            }
            else
            {
                targetNormal = -targetTransform.right;
            }
        }

        // ���ˊp�Ɩ@���x�N�g���̊p�̑傫�����Z�o
        float inDirAngleSize = Mathf.Abs(Vector2.Angle(inDirection, targetNormal));

        // 2�̃x�N�g���̊p�x�̑傫���̐�Βl���A0���ȏ�90���ȉ��̂Ƃ��A�d���Ăяo���Ɣ��肵�������I��
        if (inDirAngleSize >= 0 && inDirAngleSize <= 90)
        {
            // ���˃x�N�g�������̂܂ܕԂ�
            return inDirection;
        }

        return targetNormal;
    }
}