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
    private SpriteFont _defaultFont;
    private SpriteFont _largeFont;
    private Vector2 _viewportSize;

    private readonly Vector2 _scorePosition = new Vector2(5f, 5f);
    private readonly Vector2 _laserRayCountPosition = new Vector2(5f, 25f);
    private readonly Vector2 _title2Offset = new Vector2(0f, 100f);

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
        _viewportSize.X = viewport.Width;
        _viewportSize.Y = viewport.Height;

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

        _defaultFont = content.Load<SpriteFont>("DefaultFont");
        _largeFont = content.Load<SpriteFont>("LargeFont");
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
        DrawGameObjects();
        DrawUi();
    }

    private void DrawGameObjects()
    {
        _spriteBatch.Begin(transformMatrix: _viewportScaleMatrix);
        foreach (var obj in _model)
        {
            _drawables[obj.GetType()].Draw(_spriteBatch, obj);
        }
        _spriteBatch.End();
    }

    private void DrawUi()
    {
        _spriteBatch.Begin();

        DrawScore(String.Format("Score: {0}", _model.Score));

        switch (_model.State)
        {
            case ModelState.Fresh:
                DrawTitle1("Press  SPACE  to  start");
                break;

            case ModelState.RoundFinished:
                DrawTitle1("Game  over");
                DrawTitle2("Press  SPACE  to  restart");
                break;

            default:
                // Ignored
                break;
        }

        _spriteBatch.End();
    }

    private void DrawScore(string s)
    {
        _spriteBatch.DrawString(_defaultFont, s, _scorePosition, Color.Black);
    }

    private void DrawLaserRayCount(string s)
    {
        _spriteBatch.DrawString(_defaultFont, s, _laserRayCountPosition, Color.Black);
    }

    private void DrawTitle1(string s)
    {
        _spriteBatch.DrawString(_largeFont, s, (_viewportSize - _largeFont.MeasureString(s)) * 0.5f, Color.Black);
    }

    private void DrawTitle2(string s)
    {
        _spriteBatch.DrawString(_defaultFont, s, (_viewportSize - _defaultFont.MeasureString(s)) * 0.5f + _title2Offset,
                Color.Black);
    }
}