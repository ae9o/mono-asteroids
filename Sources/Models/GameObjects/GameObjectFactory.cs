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

namespace MonoAsteroids;

public static class GameObjectFactory
{
    public static Starship NewStarship()
    {
        Starship starship = new Starship();
        starship.Size = new Vector2(0.1f, 0.1f);
        starship.LinearDamping = 0.5f;
        starship.AngularDamping = 10f;
        starship.EngageImpulse = 0.001f;
        starship.RotationSpeed = 10;

        var fixture = starship.CreateCircle(0.05f, 1f);
        fixture.CollisionCategories = Category.Cat1;

        return starship;
    }

    public static Asteroid NewLargeAsteroid()
    {
        Asteroid asteroid = new Asteroid();
        asteroid.Size = new Vector2(0.1f, 0.1f);

        var fixture = asteroid.CreateCircle(0.05f, 1f);
        fixture.CollisionCategories = Category.Cat2;
        fixture.CollidesWith = Category.Cat1;

        return asteroid;
    }
}