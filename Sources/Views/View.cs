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
using Microsoft.Xna.Framework.Graphics;

namespace MonoAsteroids;

public partial class View : DrawableGameComponent
{
    private readonly Dictionary<Type, IDrawable> _drawables = new Dictionary<Type, IDrawable>();
    private readonly Model _model;
    private Matrix _viewportScaleMatrix;
    private SpriteBatch _spriteBatch;

    public View(Game game, Model Model) : base(game)
    {
        _model = Model;
    }

    public override void Initialize()
    {
        base.Initialize();
        ScaleViewport();
    }

    private void ScaleViewport()
    {
        var viewport = Game.GraphicsDevice.Viewport;
        var scaleX = viewport.Width / Model.WorldWidth;
        var scaleY = viewport.Height / Model.WorldHeight;
        _viewportScaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        LoadDrawables();
    }

    protected override void UnloadContent()
    {
        UnloadDrawables();
        _spriteBatch.Dispose();
    }

    private void LoadDrawables()
    {
        var content = Game.Content;
        _drawables.Add(typeof(Starship), new TextureDrawable(content, "StarshipSprite"));
        _drawables.Add(typeof(Asteroid), new TextureDrawable(content, "AsteroidSprite"));
        _drawables.Add(typeof(Ufo), new TextureDrawable(content, "UfoSprite"));
        _drawables.Add(typeof(Bullet), new TextureDrawable(content, "BulletSprite"));
        _drawables.Add(typeof(LaserRay), new TextureDrawable(content, "LaserRaySprite"));
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

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _viewportScaleMatrix);
        foreach (var obj in _model)
        {
            if (_drawables.TryGetValue(obj.GetType(), out var drawable))
            {
                drawable.Draw(_spriteBatch, obj);
            }
        }
        _spriteBatch.End();
    }
}