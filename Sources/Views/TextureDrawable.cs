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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoAsteroids;

/// <summary>
/// This class draws a simple texture at the position of the game object.
/// </summary>
public class TextureDrawable : IDrawable, IDisposable
{
    private const float CenterOrigin = 0.5f;

    private readonly Texture2D _texture;
    private readonly Vector2 _textureSize;
    private readonly Vector2 _textureOrigin;

    public TextureDrawable(ContentManager contentManager, string contentName)
    {
        _texture = contentManager.Load<Texture2D>(contentName);
        _textureSize = new Vector2(_texture.Width, _texture.Height);
        _textureOrigin = _textureSize * CenterOrigin;
    }

    public void Dispose()
    {
        _texture.Dispose();
    }

    public void Draw(SpriteBatch spriteBatch, GameObject obj)
    {
        spriteBatch.Draw(_texture, obj.Position, null, Color.White, obj.Rotation, _textureOrigin,
                obj.Size / _textureSize, SpriteEffects.None, 0);
    }
}