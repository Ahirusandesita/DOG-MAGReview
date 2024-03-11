using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// キャラクター選択システムのビューをプレイヤーごとに管理するクラス
/// </summary>
public class CharacterSelectView : MonoBehaviour
{
    [SerializeField] private Image frameImage = default;
    [SerializeField] private Image rightArrowImage = default;
    [SerializeField] private Image leftArrowImage = default;

    private Transform myTransform = default;

    // ビュー操作で使用するため、インスタンス化したImageを保存する配列
    private Image[] characterImages = default;
    private TeamColorAsset teamColorAsset;
    private CancellationToken token = default;

    // 選択中のキャラクター
    private int selectedCharacterIndex = 0;
    // 選択可能なキャラクター数
    private int selectableCharacterCount = default;

    // UI用のVector値（2人プレイ用）
    private readonly Vector2 UIPosLeft = new(-480f, 0f);
    private readonly Vector2 UIPosRight = new(480f, 0f);


    private void Awake()
    {
        myTransform = this.transform;
        token = this.GetCancellationTokenOnDestroy();
    }


    /// <summary>
    /// ビューをインスタンス化する
    /// </summary>
    /// <param name="viewData">ビューの生成に必要なデータ</param>
    public void CreateView(OutGameData[] viewData, int playerIndex, TeamColorAsset[] teamColorAssets)
    {
        // 他メソッドで使用したいため、キャラクター数を保存
        selectableCharacterCount = viewData.Length;
        // インスタンス化したオブジェクトを保存するための配列を初期化
        characterImages = new Image[selectableCharacterCount];

        // 選択可能なキャラクターのUI（Image）をインスタンス化する
        for (int i = 0; i < selectableCharacterCount; i++)
        {
            characterImages[i] = Instantiate(viewData[i].CharacterUI, myTransform);
            characterImages[i].gameObject.SetActive(false);

            // PlayerIndexから1P or 2Pを判断し、オブジェクト情報を変更
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

        // 初期状態では先頭のキャラクターを表示
        characterImages[0].gameObject.SetActive(true);
    }

    /// <summary>
    /// 選択中のキャラクターを変更する
    /// </summary>
    /// <param name="increaseOrDecrease">変更方向（インクリメント or デクリメント）</param>
    /// <returns>変更後のキャラクターインデックス</returns>
    public void ChangeCharacterSelected(int increaseOrDecrease)
    {
        ChangeColorAsync(increaseOrDecrease).Forget();

        // 選択可能キャラクターの最大値（インデックス）を取得
        int selectableCharacterMaxIndex = selectableCharacterCount - 1;

        // 現在表示されているキャラクターイメージを非表示
        characterImages[selectedCharacterIndex].gameObject.SetActive(false);

        selectedCharacterIndex += increaseOrDecrease;

        // インデックス操作の結果、最大値を超えた場合0にする
        if (selectedCharacterIndex > selectableCharacterMaxIndex)
        {
            selectedCharacterIndex = 0;
        }
        // インデックス操作の結果、0を下回った場合最大値にする
        else if (selectedCharacterIndex < 0)
        {
            selectedCharacterIndex = selectableCharacterMaxIndex;
        }

        // インデックス操作後のキャラクターを表示
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
    /// 非同期で矢印の色を変える
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
