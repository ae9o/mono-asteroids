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
using MonoGame.Extended;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoAsteroids;

public abstract class GameObject : Body, IUpdate
{
    public event EventHandler Added;
    public event EventHandler Removed;
    public event EventHandler FlewOutOfWorld;

    private Model _model;

    public GameObject()
    {
        BodyType = BodyType.Dynamic;
    }

    public Model Model
    {
        get => _model;

        set
        {
            if (_model == value)
            {
                return;
            }

            _model = value;

            if (value != null)
            {
                OnAdded();
            }
            else
            {
                OnRemoved();
            }
        }
    }

    public Vector2 Size { get; set; }

    public Vector2 LookDirection => GetWorldVector(Model.WorldUpDirection);

    public virtual void Update(GameTime gameTime)
    {
        ClipToWorld();
    }

    private void ClipToWorld()
    {
        Vector2 halfSize = Size * 0.5f;
        Vector2 pos = Position;
        Vector2 tmp = pos;

        if (pos.X < -halfSize.X)
        {
            tmp.X = Model.WorldWidth + halfSize.X;
        }
        else if (pos.X > Model.WorldWidth + halfSize.X)
        {
            tmp.X = -halfSize.X;
        }

        if (pos.Y < -halfSize.Y)
        {
            tmp.Y = Model.WorldHeight + halfSize.Y;
        }
        else if (pos.Y > Model.WorldHeight + halfSize.Y)
        {
            tmp.Y = -halfSize.Y;
        }

        if (tmp != pos)
        {
            OnFlewOutOfWorld();
            Position = tmp;
        }
    }

    public virtual void OnFlewOutOfWorld()
    {
        FlewOutOfWorld?.Invoke(this, EventArgs.Empty);
    }

    public void Remove()
    {
        Model?.Remove(this);
    }

    public virtual void OnAdded()
    {
        Added?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnRemoved()
    {
        Removed?.Invoke(this, EventArgs.Empty);
    }
}