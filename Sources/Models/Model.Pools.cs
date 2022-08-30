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

namespace MonoAsteroids;

/// <summary>
/// This part of the model controls the creation of large amounts of game objects. Since the same kinds of objects are
/// constantly created and destroyed during the game, pooling must be applied.
/// </summary>
public partial class Model : GameComponent, IEnumerable<GameObject>
{
    public const int InitialAsteroidCount = 2;

    private readonly Pool<Asteroid> _largeAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewLargeAsteroid);
    private readonly Pool<Asteroid> _mediumAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewMediumAsteroid);
    private readonly Pool<Asteroid> _smallAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewSmallAsteroid);
    private readonly Pool<Ufo> _ufoPool = new Pool<Ufo>(GameObjectFactory.NewDefaultUfo);
    private readonly Pool<Blast> _blastPool = new Pool<Blast>(GameObjectFactory.NewDefaultBlast);

    private Asteroid ObtainLargeAsteroid()
    {
        var asteroid = _largeAsteroidPool.Obtain();
        asteroid.Position = RandomUtils.Random.NextPositionOutsideWorld(WorldWidth, WorldHeight, asteroid.Size);
        asteroid.ShardSupplier = ObtainMediumAsteroid;
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Asteroid ObtainMediumAsteroid()
    {
        var asteroid = _mediumAsteroidPool.Obtain();
        asteroid.ShardSupplier = ObtainSmallAsteroid;
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Asteroid ObtainSmallAsteroid()
    {
        var asteroid = _smallAsteroidPool.Obtain();
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Ufo ObtainUfo()
    {
        var ufo = _ufoPool.Obtain();
        ufo.Target = _starship;
        ufo.Position = RandomUtils.Random.NextPositionOutsideWorld(WorldWidth, WorldHeight, ufo.Size);
        ufo.BlastSupplier = ObtainBlast;
        ufo.Broken += OnScorableObjectBroken;
        return ufo;
    }

    private Blast ObtainBlast()
    {
        return _blastPool.Obtain();
    }

    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < InitialAsteroidCount; ++i)
        {
            SpawnLargeAsteroid();
        }
    }

    private void SpawnLargeAsteroid()
    {
        Add(ObtainLargeAsteroid());
    }

    private void SpawnUfo()
    {
        Add(ObtainUfo());
    }
}