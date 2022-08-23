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
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoAsteroids;

public class Core : GameComponent
{
    public const float VIRTUAL_WORLD_WIDTH = 1280f;
    public const float VIRTUAL_WORLD_HEIGHT = 720f;

    private readonly List<GameObject> _gameObjects = new List<GameObject>();

    private World _world;
    private Starship _starship;
    private Asteroid _asteroid;
    private Ufo _ufo;

    public Starship Starship => _starship;

    public Core(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
        _world = new World();

        _starship = new Starship(_world);
        _asteroid = new Asteroid();
        _ufo = new Ufo();

        _gameObjects.Add(_starship);
        _gameObjects.Add(_asteroid);
        _gameObjects.Add(_ufo);

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var o in _gameObjects)
        {
            o.Update(gameTime);
            ClipToWorld(o);            
        }

        _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void ClipToWorld(GameObject gameObject)
    {
        Vector2 size = gameObject.Size;
        Vector2 pos = gameObject.Position;

        if (pos.X < -size.X)
        {
            pos.X = VIRTUAL_WORLD_WIDTH;
        }
        else if (pos.X > VIRTUAL_WORLD_WIDTH)
        {
            pos.X = -size.X;
        }

        if (pos.Y < -size.Y)
        {
            pos.Y = VIRTUAL_WORLD_HEIGHT;
        }
        else if (pos.Y > VIRTUAL_WORLD_HEIGHT)
        {
            pos.Y = -size.Y;
        }

        gameObject.Position = pos;
    }

    public void Visit(IGameObjectsVisitor visitor)
    {
        foreach (var o in _gameObjects)
        {
            o.Visit(visitor);
        }
    }
}