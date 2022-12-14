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

using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace MonoAsteroids;

/// <summary>
///
/// <para>A spaceship controlled by the player. The spaceship has two types of guns that the player can use to destroy
/// various game objects: machine gun and laser gun.</para>
///
/// <para>The spaceship explodes if it collides with another game object.</para>
///
/// </summary>
public class Starship : Spacecraft
{
    private readonly LaserGun _laserGun;
    private readonly MachineGun _machineGun;

    public Starship()
    {
        _laserGun  = new LaserGun(this);
        _machineGun = new MachineGun(this);
    }

    public LaserGun LaserGun => _laserGun;

    public MachineGun MachineGun => _machineGun;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _laserGun.Update(gameTime);
        _machineGun.Update(gameTime);
    }

    protected override bool OnCollisionValidating(Fixture sender, Fixture other, Contact contact)
    {
        Break((GameObject)other.Body);

        return base.OnCollisionValidating(sender, other, contact);
    }
}