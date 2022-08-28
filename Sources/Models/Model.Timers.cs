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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace MonoAsteroids;

public partial class Model : GameComponent, IEnumerable<GameObject>
{
    public const float AsteroidSpawnInterval = 10f;
    public const float UfoSpawnInterval = 10f;

    private ContinuousClock _asteroidSpawnClock;
    private ContinuousClock _ufoSpawnClock;

    private void InitializeTimers()
    {
        _asteroidSpawnClock = new ContinuousClock(AsteroidSpawnInterval);
        _asteroidSpawnClock.Tick += OnAsteroidSpawnClockTick;
        _asteroidSpawnClock.Stop();
    }

    private void UpdateTimers(GameTime gameTime)
    {
        _asteroidSpawnClock.Update(gameTime);
    }

    private void StopTimers()
    {
        _asteroidSpawnClock.Stop();
    }

    private void RestartTimers()
    {
        _asteroidSpawnClock.Restart();
    }
}