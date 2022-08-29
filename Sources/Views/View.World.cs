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

namespace MonoAsteroids;

public partial class View : DrawableGameComponent
{
    private readonly Dictionary<Type, IDrawable> _drawables = new Dictionary<Type, IDrawable>();

    private void LoadDrawables()
    {
        var content = Game.Content;

        _drawables.Add(typeof(Starship), new TextureDrawable(content, "Sprites/StarshipSprite"));
        _drawables.Add(typeof(Asteroid), new TextureDrawable(content, "Sprites/AsteroidSprite"));
        _drawables.Add(typeof(Ufo), new TextureDrawable(content, "Sprites/UfoSprite"));
        _drawables.Add(typeof(Bullet), new TextureDrawable(content, "Sprites/BulletSprite"));
        _drawables.Add(typeof(LaserRay), new TextureDrawable(content, "Sprites/LaserRaySprite"));
    }

    private void UnloadDrawables()
    {
        foreach (var drawable in _drawables)
        {
            if (drawable.Value is IDisposable)
            {
                ((IDisposable)drawable.Value).Dispose();
            }
        }
        _drawables.Clear();
    }

    private void DrawWorld(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: _viewportScaleMatrix);
        foreach (var obj in _model)
        {
            _drawables[obj.GetType()].Draw(_spriteBatch, obj);
        }
        _spriteBatch.End();
    }
}