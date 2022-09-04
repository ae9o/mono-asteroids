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
using MonoGame.Extended.Collections;

namespace MonoAsteroids;

/// <summary>
/// Manages all game timers.
/// </summary>
public class Timers
{
    private readonly Bag<GameTimer> _timers = new Bag<GameTimer>();

    public void Add(EventHandler OnTick, float interval)
    {
        var timer = new ContinuousClock(interval);
        timer.Tick += OnTick;
        timer.Stop();
        _timers.Add(timer);
    }

    public void Stop()
    {
        foreach (var timer in _timers)
        {
            timer.Stop();
        }
    }

    public void Restart()
    {
        foreach (var timer in _timers)
        {
            timer.Restart();
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var timer in _timers)
        {
            timer.Update(gameTime);
        }
    }
}