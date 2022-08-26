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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace MonoAsteroids;

public class Starship : GameObject
{
    private readonly CountdownTimer _fireBulletCooldown = new CountdownTimer(0);
    private readonly CountdownTimer _fireLaserCooldown = new CountdownTimer(0);
    private readonly List<CountdownTimer> _chargeLaserCooldowns = new List<CountdownTimer>();

    public Func<Bullet> BulletSupplier { get; set; }

    public Func<LaserRay> LaserRaySupplier { get; set; }

    public int LaserCapacity { get; set; } = 5;

    public float EngageImpulse { get; set; } = 0.001f;

    public float RotationSpeed { get; set; } = 5f;

    public float BulletCooldown { get; set; } = 0.2f;

    public float LaserCooldown { get; set; } = 0.2f;

    public void Engage()
    {
        ApplyLinearImpulse(LookDirection * EngageImpulse);
    }

    public void RotateLeft(float delta)
    {
        Rotation -= RotationSpeed * delta;
    }

    public void RotateRight(float delta)
    {
        Rotation += RotationSpeed * delta;
    }

    public void FireBullet()
    {
        if (_fireBulletCooldown.State == TimerState.Started)
        {
            return;
        }

        if (BulletSupplier == null)
        {
            return;
        }

        FireProjectile(BulletSupplier());

        _fireBulletCooldown.Interval = TimeSpan.FromSeconds(BulletCooldown);
        _fireBulletCooldown.Restart();
    }

    public void FireLaser()
    {
        if (_fireLaserCooldown.State == TimerState.Started)
        {
            return;
        }

        if (LaserRaySupplier == null)
        {
            return;
        }

        FireProjectile(LaserRaySupplier());

        _fireLaserCooldown.Interval = TimeSpan.FromSeconds(LaserCooldown);
        _fireLaserCooldown.Restart();
    }

    private void FireProjectile(Projectile projectile)
    {
        projectile.Position = Position;
        projectile.Rotation = Rotation;
        projectile.Fire(LookDirection);
        Model.Add(projectile);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _fireBulletCooldown.Update(gameTime);
        _fireLaserCooldown.Update(gameTime);
    }
}