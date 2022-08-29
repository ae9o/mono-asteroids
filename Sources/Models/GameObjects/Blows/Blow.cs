/*
 * Copyright (C) 2022 Alexei Evdokimenko
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace MonoAsteroids;

public class Blow : PoolableGameObject
{
    private readonly CountdownTimer _removeTimer;

    public TimeSpan RemoveInterval
    {
        get => _removeTimer.Interval;

        set
        {
            _removeTimer.Interval = value;
        }
    }

    public Blow()
    {
        _removeTimer = new CountdownTimer(0);
        _removeTimer.Completed += OnRemoveTimerCompleted;
        _removeTimer.Stop();
    }

    protected override void OnAdded()
    {
        base.OnAdded();

        _removeTimer.Restart();
    }

    private void OnRemoveTimerCompleted(object sender, EventArgs args)
    {
        Remove();
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();

        ReturnToPool();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _removeTimer.Update(gameTime);
    }

    public override void Reset() {}
}