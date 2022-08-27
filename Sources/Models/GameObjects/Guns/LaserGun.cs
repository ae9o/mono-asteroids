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

public class LaserGun : Gun<LaserRay>
{
    private readonly ContinuousClock _chargingClock;
    private int _maxCharge = 3;
    private int _currentCharge;

    public LaserGun(GameObject parent)
        : base(parent)
    {
        _chargingClock = new ContinuousClock(ChargingInterval);
        _chargingClock.Tick += OnChargingTick;
    }

    public int MaxCharge
    {
        get => _maxCharge;

        set
        {
            _currentCharge += value - _maxCharge;
            _maxCharge = value;
        }
    }

    public int CurrentCharge => _currentCharge;

    public float ChargingInterval { get; set; } = 3f;

    public override void Fire()
    {
        if (_currentCharge > 0)
        {
            base.Fire();
        }
    }

    protected override void DoFire(LaserRay ray)
    {
        base.DoFire(ray);

        --_currentCharge;
    }

    private void OnChargingTick(object sender, EventArgs args)
    {
        if (_currentCharge < _maxCharge)
        {
            ++_currentCharge;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _chargingClock.Update(gameTime);
    }
}