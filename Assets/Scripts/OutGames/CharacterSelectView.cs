using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// �L�����N�^�[�I���V�X�e���̃r���[���v���C���[���ƂɊǗ�����N���X
/// </summary>
public class CharacterSelectView : MonoBehaviour
{
    [SerializeField] private Image frameImage = default;
    [SerializeField] private Image rightArrowImage = default;
    [SerializeField] private Image leftArrowImage = default;

    private Transform myTransform = default;

    // �r���[����Ŏg�p���邽�߁A�C���X�^���X������Image��ۑ�����z��
    private Image[] characterImages = default;
    private TeamColorAsset teamColorAsset;
    private CancellationToken token = default;

    // �I�𒆂̃L�����N�^�[
    private int selectedCharacterIndex = 0;
    // �I���\�ȃL�����N�^�[��
    private int selectableCharacterCount = default;

    // UI�p��Vector�l�i2�l�v���C�p�j
    private readonly Vector2 UIPosLeft = new(-480f, 0f);
    private readonly Vector2 UIPosRight = new(480f, 0f);


    private void Awake()
    {
        myTransform = this.transform;
        token = this.GetCancellationTokenOnDestroy();
    }


    /// <summary>
    /// �r���[���C���X�^���X������
    /// </summary>
    /// <param name="viewData">�r���[�̐����ɕK�v�ȃf�[�^</param>
    public void CreateView(OutGameData[] viewData, int playerIndex, TeamColorAsset[] teamColorAssets)
    {
        // �����\�b�h�Ŏg�p���������߁A�L�����N�^�[����ۑ�
        selectableCharacterCount = viewData.Length;
        // �C���X�^���X�������I�u�W�F�N�g��ۑ����邽�߂̔z���������
        characterImages = new Image[selectableCharacterCount];

        // �I���\�ȃL�����N�^�[��UI�iImage�j���C���X�^���X������
        for (int i = 0; i < selectableCharacterCount; i++)
        {
            characterImages[i] = Instantiate(viewData[i].CharacterUI, myTransform);
            characterImages[i].gameObject.SetActive(false);

            // PlayerIndex����1P or 2P�𔻒f���A�I�u�W�F�N�g����ύX
            switch (playerIndex)
            {
                case 0:
                    teamColorAsset = teamColorAssets[0];
                    frameImage.color = teamColorAsset.UIColor;

                    characterImages[i].color = teamColorAssets[0].CharacterColor;
                    myTransform.localPosition = UIPosLeft;
                    break;

                case 1:
                    teamColorAsset = teamColorAssets[1];
                    frameImage.color = teamColorAsset.UIColor;

                    characterImages[i].color = teamColorAssets[1].CharacterColor;
                    myTransform.localPosition  = UIPosRight;
                    break;

                // Debug
                case 2:
                    teamColorAsset = teamColorAssets[0];
                    frameImage.color = teamColorAsset.UIColor;

                    characterImages[i].color = teamColorAssets[0].CharacterColor;
                    myTransform.localPosition = UIPosLeft + Vector2.up * -240f;
                    break;

                case 3:
                    teamColorAsset = teamColorAssets[1];
                    frameImage.color = teamColorAsset.UIColor;

                    characterImages[i].color = teamColorAssets[1].CharacterColor;
                    myTransform.localPosition = UIPosRight + Vector2.up * -240f;
                    break;
            }
        }

        // ������Ԃł͐擪�̃L�����N�^�[��\��
        characterImages[0].gameObject.SetActive(true);
    }

    /// <summary>
    /// �I�𒆂̃L�����N�^�[��ύX����
    /// </summary>
    /// <param name="increaseOrDecrease">�ύX�����i�C���N�������g or �f�N�������g�j</param>
    /// <returns>�ύX��̃L�����N�^�[�C���f�b�N�X</returns>
    public void ChangeCharacterSelected(int increaseOrDecrease)
    {
        ChangeColorAsync(increaseOrDecrease).Forget();

        // �I���\�L�����N�^�[�̍ő�l�i�C���f�b�N�X�j���擾
        int selectableCharacterMaxIndex = selectableCharacterCount - 1;

        // ���ݕ\������Ă���L�����N�^�[�C���[�W���\��
        characterImages[selectedCharacterIndex].gameObject.SetActive(false);

        selectedCharacterIndex += increaseOrDecrease;

        // �C���f�b�N�X����̌��ʁA�ő�l�𒴂����ꍇ0�ɂ���
        if (selectedCharacterIndex > selectableCharacterMaxIndex)
        {
            selectedCharacterIndex = 0;
        }
        // �C���f�b�N�X����̌��ʁA0����������ꍇ�ő�l�ɂ���
        else if (selectedCharacterIndex < 0)
        {
            selectedCharacterIndex = selectableCharacterMaxIndex;
        }

        // �C���f�b�N�X�����̃L�����N�^�[��\��
        characterImages[selectedCharacterIndex].gameObject.SetActive(true);
    }

    public int SubmitCharacter()
    {
        rightArrowImage.gameObject.SetActive(false);
        leftArrowImage.gameObject.SetActive(false);

        return selectedCharacterIndex;
    }

    public void CanceledCharacter()
    {
        rightArrowImage.gameObject.SetActive(true);
        leftArrowImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// �񓯊��Ŗ��̐F��ς���
    /// </summary>
    private async UniTaskVoid ChangeColorAsync(int increaseOrDecrease)
    {
        switch (increaseOrDecrease)
        {
            case 1:
                rightArrowImage.color = teamColorAsset.UIColor;
                break;

            case -1:
                leftArrowImage.color = teamColorAsset.UIColor;
                break;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.13), cancellationToken: token);

        switch (increaseOrDecrease)
        {
            case 1:
                rightArrowImage.color = Color.white;
                break;

            case -1:
                leftArrowImage.color = Color.white;
                break;
        }
    }
}
