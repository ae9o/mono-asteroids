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

/// <summary>
/// Draws the content of the game stage.
/// </summary>
public class StageView : DrawableGameComponent
{
    private readonly Dictionary<Type, IDrawable> _drawables = new Dictionary<Type, IDrawable>();
    private Texture2D _backgroundTexture;
    private Matrix _viewportScaleMatrix;
    private SpriteBatch _spriteBatch;
    private Vector2 _viewportSize;
    private float _scaleX;
    private float _scaleY;

    public StageView(Game game)
        : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        ScaleViewport();
    }

    private void ScaleViewport()
    {
        var viewport = Game.GraphicsDevice.Viewport;
        _viewportSize.X = viewport.Width;
        _viewportSize.Y = viewport.Height;

        _scaleX = viewport.Width / Stage.Width;
        _scaleY = viewport.Height / Stage.Height;
        _viewportScaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1.0f);
    }

    protected override void LoadContent()
    {
        var content = Game.Content;

        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        _backgroundTexture = content.Load<Texture2D>("Backgrounds/Space");

        _drawables.Add(typeof(Starship), new TextureDrawable(content, "Sprites/StarshipSprite"));
        _drawables.Add(typeof(Asteroid), new TextureDrawable(content, "Sprites/AsteroidSprite"));
        _drawables.Add(typeof(Ufo), new TextureDrawable(content, "Sprites/UfoSprite"));
        _drawables.Add(typeof(Bullet), new TextureDrawable(content, "Sprites/BulletSprite"));
        _drawables.Add(typeof(LaserRay), new TextureDrawable(content, "Sprites/LaserRaySprite"));
        _drawables.Add(typeof(Blast), new TextureDrawable(content, "Sprites/BlastSprite"));
    }

    protected override void UnloadContent()
    {
        foreach (var drawable in _drawables)
        {
            if (drawable.Value is IDisposable)
            {
                ((IDisposable)drawable.Value).Dispose();
            }
        }
        _drawables.Clear();

        _backgroundTexture.Dispose();
        _spriteBatch.Dispose();
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Black);
        DrawBackground();
        DrawGameObjects();
    }

    private void DrawBackground()
    {
        _spriteBatch.Begin();
        var v = new Vector2();
        var dx = _backgroundTexture.Width;
        var dy = _backgroundTexture.Height;
        do
        {
            do
            {
                _spriteBatch.Draw(_backgroundTexture, v, Color.White);
                v.X += dx;
            }
            while (v.X < _viewportSize.X);
            v.Y += dy;
            v.X = 0;
        }
        while (v.Y < _viewportSize.Y);
        _spriteBatch.End();
    }

    private void DrawGameObjects()
    {
        _spriteBatch.Begin(transformMatrix: _viewportScaleMatrix);
        foreach (var obj in Model.Instance.Stage)
        {
            _drawables[obj.GetType()].Draw(_spriteBatch, obj);
        }
        _spriteBatch.End();
    }
}