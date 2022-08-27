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

using MonoGame.Extended;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoAsteroids;

public static class GameObjectFactory
{
    public static Starship NewDefaultStarship()
    {
        Starship starship = new Starship();
        starship.Size = new Vector2(0.1f, 0.1f);
        starship.LinearDamping = 5f;
        starship.AngularDamping = 10f;
        starship.MachineGun.ProjectileSupplier = NewDefaultBullet;
        starship.LaserGun.ProjectileSupplier = NewDefaultLaserRay;

        var fixture = starship.CreateCircle(0.05f, 1f);
        fixture.CollisionCategories = Category.Cat1;
        fixture.IsSensor = true;

        return starship;
    }

    public static Starship NewDemoStarship()
    {
        Starship starship = new Starship();
        starship.Size = new Vector2(0.1f, 0.1f);
        starship.Position = Utils.Random.NextPositionOutsideWorld(Model.WorldWidth, Model.WorldHeight);
        starship.LinearVelocity = new Vector2(0.2f, -0.2f);
        starship.Rotation = 0.75f;
        return starship;
    }

    public static Asteroid NewLargeAsteroid()
    {
        Asteroid asteroid = new Asteroid();
        asteroid.Size = new Vector2(0.1f, 0.1f);
        asteroid.LinearVelocity = Utils.Random.NextVector(0.1f, 0.2f);
        asteroid.AngularVelocity = Utils.Random.NextSingle(-0.7f, 0.7f);

        var fixture = asteroid.CreateCircle(0.05f, 1f);
        fixture.CollisionCategories = Category.Cat2;
        fixture.CollidesWith = Category.Cat1 | Category.Cat3;

        return asteroid;
    }

    public static Asteroid NewMediumAsteroid()
    {
        Asteroid asteroid = new Asteroid();
        asteroid.Size = new Vector2(0.05f, 0.05f);

        var fixture = asteroid.CreateCircle(0.025f, 1f);
        fixture.CollisionCategories = Category.Cat2;
        fixture.CollidesWith = Category.Cat1 | Category.Cat3;

        return asteroid;
    }

    public static Asteroid NewSmallAsteroid()
    {
        Asteroid asteroid = new Asteroid();
        asteroid.Size = new Vector2(0.025f, 0.025f);

        var fixture = asteroid.CreateCircle(0.0125f, 1f);
        fixture.CollisionCategories = Category.Cat2;
        fixture.CollidesWith = Category.Cat1 | Category.Cat3;

        return asteroid;
    }

    public static Bullet NewDefaultBullet()
    {
        Bullet bullet = new Bullet();
        bullet.Size = new Vector2(0.01f, 0.01f);
        bullet.Speed = 1f;
        bullet.RemoveSelfOnCollision = true;

        var fixture = bullet.CreateRectangle(0.01f, 0.01f, 1f, Vector2.Zero);
        fixture.CollisionCategories = Category.Cat3;
        fixture.CollidesWith = Category.Cat2;
        fixture.IsSensor = true;

        return bullet;
    }

    public static LaserRay NewDefaultLaserRay()
    {
        LaserRay ray = new LaserRay();
        ray.Size = new Vector2(0.01f, 0.05f);
        ray.Speed = 2f;

        var fixture = ray.CreateRectangle(0.01f, 0.05f, 1f, Vector2.Zero);
        fixture.CollisionCategories = Category.Cat3;
        fixture.CollidesWith = Category.Cat2;
        fixture.IsSensor = true;

        return ray;
    }
}