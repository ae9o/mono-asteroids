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
///
/// <para>The base class for a game weapon. Allows to shoot the specified type of ammo. This is a customizable property.
/// </para>
///
/// <para>Allows to set a cooldown that limits the intensity of shooting.</para>
///
/// <para>It is not a standalone game object. It binds to the parent game object and uses its characteristics to place
/// ammo in the world and set the direction of their flight.<para>
///
/// </summary>
/// <typeparam name="T">Type of the ammo.</typeparam>
public class Gun<T>
    where T : Projectile
{
    private readonly CountdownTimer _cooldownTimer = new CountdownTimer(0);
    private readonly GameObject _parent;
    private Func<T> _projectileSupplier;
    private Pool<T> _projectilePool;

    public Gun(GameObject parent)
    {
        _parent = parent;
    }

    public float Cooldown { get; set; } = 0.2f;

    public Func<T> ProjectileSupplier
    {
        get => _projectileSupplier;

        set
        {
            _projectileSupplier = value;
            _projectilePool = new Pool<T>(value);
        }
    }

    public virtual void Fire()
    {
        if (_cooldownTimer.State == TimerState.Started)
        {
            return;
        }

        if (_projectilePool == null)
        {
            return;
        }

        DoFire(_projectilePool.Obtain());

        _cooldownTimer.Interval = TimeSpan.FromSeconds(Cooldown);
        _cooldownTimer.Restart();
    }

    protected virtual void DoFire(T projectile)
    {
        projectile.Position = _parent.Position;
        projectile.Rotation = _parent.Rotation;
        projectile.Fire(_parent.LookDirection);
        _parent.Model.Add(projectile);
    }

    public virtual void Update(GameTime gameTime)
    {
        _cooldownTimer.Update(gameTime);
    }
}