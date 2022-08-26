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
    private readonly CountdownTimer _bulletCooldownTimer = new CountdownTimer(0);
    private readonly CountdownTimer _laserCooldownTimer = new CountdownTimer(0);
    private readonly Stack<Bullet> _bullets = new Stack<Bullet>();
    private readonly Stack<LaserRay> _laserRays = new Stack<LaserRay>();

    public float EngageImpulse { get; set; }

    public float RotationSpeed { get; set; }

    public float BulletCooldown { get; set; }

    public float LaserCooldown { get; set; }

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
        if (_bulletCooldownTimer.State == TimerState.Started)
        {
            return;
        }

        if (_bullets.Count == 0)
        {
            return;
        }

        FireProjectile(_bullets.Pop());

        _bulletCooldownTimer.Interval = TimeSpan.FromSeconds(BulletCooldown);
        _bulletCooldownTimer.Restart();
    }

    public void FireLaser()
    {
        if (_laserCooldownTimer.State == TimerState.Started)
        {
            return;
        }

        if (_laserRays.Count == 0)
        {
            return;
        }

        FireProjectile(_laserRays.Pop());

        _laserCooldownTimer.Interval = TimeSpan.FromSeconds(LaserCooldown);
        _laserCooldownTimer.Restart();
    }

    private void FireProjectile(Projectile projectile)
    {
        Model.Add(projectile);
        projectile.Position = Position;
        projectile.Rotation = Rotation;
        projectile.Fire(LookDirection);
    }

    public void Add(Bullet bullet)
    {
        bullet.Removed += OnBulletRemoved;
        _bullets.Push(bullet);
    }

    public void Add(LaserRay ray)
    {
        _laserRays.Push(ray);
    }

    private void OnBulletRemoved(object sender, EventArgs args)
    {
        _bullets.Push((Bullet)sender);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _bulletCooldownTimer.Update(gameTime);
        _laserCooldownTimer.Update(gameTime);
    }
}