using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class DeviceManager
{
    private static Gamepad[] s_gamepads = default;

    private InputDevice[] playDevices = default;

    private const float VIBRATE_POWER = 0.1f;


    public DeviceManager(InputDevice[] devices)
    {
        playDevices = devices;
        var gamepads = new List<Gamepad>();

        foreach (var device in playDevices)
        {
            if (device is Gamepad gamepad)
            {
                gamepads.Add(gamepad);
            }
        }

        s_gamepads = gamepads.ToArray();
    }

    /// <summary>
    /// �Q�[���ɐڑ����Ă���Gamepad��U��������
    /// </summary>
    /// <param name="vibrateTime">�U������</param>
    /// <param name="vibratePower">�U���̋��� (0.0f ~ 1.0f)</param>
    public static async UniTaskVoid VibrateGamepads(float vibrateTime, float vibratePower = VIBRATE_POWER)
    {
        var cts = new CancellationTokenSource();

        foreach (var gamepad in s_gamepads)
        {
            gamepad.SetMotorSpeeds(vibratePower, vibratePower);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(vibrateTime), cancellationToken: cts.Token);

        foreach (var gamepad in s_gamepads)
        {
            gamepad.ResetHaptics();
        }
    }

    /// <summary>
    /// �w�肵��Gamepad��U��������
    /// </summary>
    /// <param name="vibrateTime">�U������</param>
    /// <param name="vibratePower">�U���̋��� (0.0f ~ 1.0f)</param>
    public static async UniTaskVoid VibrateGamepad(PlayerInput playerInput, float vibrateTime, float vibratePower = VIBRATE_POWER)
    {
        Gamepad haptics = null;

        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad gamepad)
            {
                haptics = gamepad;
                break;
            }
        }

        if (haptics is null)
        {
            Debug.LogError("Gamepad�ł͂Ȃ����ߐU���ł��܂���B");
            return;
        }

        var cts = new CancellationTokenSource();

        haptics.SetMotorSpeeds(vibratePower, vibratePower);

        await UniTask.Delay(TimeSpan.FromSeconds(vibrateTime), cancellationToken: cts.Token);

        haptics.ResetHaptics();
    }

    /// <summary>
    /// �w�肵��Gamepad��U��������
    /// </summary>
    /// <param name="vibrateTime">�U������</param>
    /// <param name="vibratePower">�U���̋��� (0.0f ~ 1.0f)</param>
    public static async UniTaskVoid VibrateGamepad(InputDevice device, float vibrateTime, float vibratePower = VIBRATE_POWER)
    {
        if (device is Gamepad gamepad)
        {
            var cts = new CancellationTokenSource();

            gamepad.SetMotorSpeeds(vibratePower, vibratePower);

            await UniTask.Delay(TimeSpan.FromSeconds(vibrateTime), cancellationToken: cts.Token);

            gamepad.ResetHaptics();
        }
        else
        {
            Debug.LogError("Gamepad�ł͂Ȃ����ߐU���ł��܂���B");
            return;
        }
    }

    /// <summary>
    /// �w�肵��Gamepad��U��������
    /// </summary>
    /// <param name="vibrateTime">�U������</param>
    /// <param name="vibratePower">�U���̋��� (0.0f ~ 1.0f)</param>
    public static async UniTaskVoid VibrateGamepad(Gamepad gamepad, float vibrateTime, float vibratePower = VIBRATE_POWER)
    {
        var cts = new CancellationTokenSource();

        gamepad.SetMotorSpeeds(vibratePower, vibratePower);

        await UniTask.Delay(TimeSpan.FromSeconds(vibrateTime), cancellationToken: cts.Token);

        gamepad.ResetHaptics();
    }
}
