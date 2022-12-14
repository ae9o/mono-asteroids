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

/// <summary>
/// A laser gun with limited ammo. Its ammo has the ability to regenerate over time.
/// </summary>
public class LaserGun : Gun<LaserRay>
{
    private readonly CountdownTimer _chargingTimer;
    private int _maxCharge = 3;
    private int _currentCharge;

    public LaserGun(GameObject parent)
        : base(parent)
    {
        _currentCharge = _maxCharge;

        _chargingTimer = new CountdownTimer(ChargingInterval);
        _chargingTimer.Completed += OnChargingCompleted;
        _chargingTimer.Stop();
    }

    public int MaxCharge
    {
        get => _maxCharge;

        set
        {
            _currentCharge += value - _maxCharge;
            _maxCharge = value;
            StartCharging();
        }
    }

    public int CurrentCharge => _currentCharge;

    public float ChargingInterval { get; set; } = 3f;

    public double ChargingPercent => _chargingTimer.CurrentTime.TotalSeconds / ChargingInterval;

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
        StartCharging();
    }

    private void StartCharging()
    {
        if ((_currentCharge < _maxCharge) && (_chargingTimer.State != TimerState.Started))
        {
            _chargingTimer.Restart();
        }
    }

    private void OnChargingCompleted(object sender, EventArgs args)
    {
        if (++_currentCharge < _maxCharge)
        {
            _chargingTimer.Restart();
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _chargingTimer.Update(gameTime);
    }
}