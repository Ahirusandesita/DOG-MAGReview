using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// アニメーションクラス
/// </summary>
public class Tween : ITween, IDisposable
{
    private IEnumerator coroutine = default;
    private bool isStop = default;


    /// <summary>
    /// 点滅（停止されるまで繰り返し実行される）
    /// </summary>
    /// <returns>実行中のアニメーション</returns>
    public ITween Flash(Graphic targetUI, float flashInterval, float minAlfa, float maxAlfa)
    {
        coroutine = FlashAsync(targetUI, flashInterval, minAlfa, maxAlfa);
        CoroutineHandler.StartStaticCoroutine(coroutine);
        isStop = false;

        return this;
    }

    private IEnumerator FlashAsync(Graphic targetUI, float flashInterval, float minAlfa, float maxAlfa)
    {
        bool isIncrement = false;
        ReactiveProperty<float> alfaProperty = new ReactiveProperty<float>();
        alfaProperty.Where(alfa => !isIncrement && alfa <= minAlfa).Subscribe(alfa => ChangeIncrement());
        alfaProperty.Where(alfa => isIncrement && alfa >= maxAlfa).Subscribe(alfa => ChangeDecrement());

        while (true)
        {
            alfaProperty.Value = targetUI.color.a;
            if (isIncrement)
            {
                alfaProperty.Value += (maxAlfa - minAlfa) * (Time.deltaTime / flashInterval);
            }
            else
            {
                alfaProperty.Value -= (maxAlfa - minAlfa) * (Time.deltaTime / flashInterval);
            }
            targetUI.color = new Color(1f, 1f, 1f, alfaProperty.Value);

            yield return null;
        }


        void ChangeIncrement()
        {
            isIncrement = true;
            alfaProperty.Value = minAlfa;
        }
        void ChangeDecrement()
        {
            isIncrement = false;
            alfaProperty.Value = maxAlfa;
        }
    }

    public void Stop()
    {
        if (!isStop)
        {
            isStop = true;
            CoroutineHandler.instance.StopCoroutine(coroutine);
        }
        else
        {
            Debug.LogError("コルーチンは既に停止しています。");
        }
    }

    public void Restart()
    {
        if (isStop)
        {
            isStop = false;
            CoroutineHandler.StartStaticCoroutine(coroutine);
        }
        else
        {
            Debug.LogError("コルーチンは既に再開しています。");
        }
    }

    public void Dispose()
    {
        if (!isStop)
        {
            isStop = true;
            CoroutineHandler.instance.StopCoroutine(coroutine);
        }

        coroutine = null;
    }
}
