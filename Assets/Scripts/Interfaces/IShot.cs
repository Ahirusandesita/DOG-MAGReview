using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public interface IShot
{
    IReadOnlyReactiveProperty<int> BulletCountProperty { get; }
    void Life(int hp);
}
