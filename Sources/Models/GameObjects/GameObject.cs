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
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace MonoAsteroids;

/// <summary>
/// The base class for all game objects.
/// </summary>
public abstract class GameObject : Body
{
    public GameObject()
    {
        BodyType = BodyType.Dynamic;
    }

    public GameObject Parent { get; set; }

    public Vector2 Size { get; set; }

    public Vector2 LookDirection => GetWorldVector(Stage.UpDirection);

    public void Remove()
    {
        Model.Instance.Stage.Remove(this);
    }

    public virtual void Update(GameTime gameTime)
    {
        ClipToStage();
    }

    /// <summary>
    /// Adjusts the position of the object so that it stays within the bounds of the stage.
    /// </summary>
    private void ClipToStage()
    {
        Vector2 halfSize = Size * 0.5f;
        Vector2 pos = Position;
        Vector2 tmp = pos;

        if (pos.X < -halfSize.X)
        {
            tmp.X = Stage.Width + halfSize.X;
        }
        else if (pos.X > Stage.Width + halfSize.X)
        {
            tmp.X = -halfSize.X;
        }

        if (pos.Y < -halfSize.Y)
        {
            tmp.Y = Stage.Height + halfSize.Y;
        }
        else if (pos.Y > Stage.Height + halfSize.Y)
        {
            tmp.Y = -halfSize.Y;
        }

        if (tmp != pos)
        {
            OnFlewOutOfStage();
            Position = tmp;
        }
    }

    /// <summary>
    /// Called when the object leaves the stage boundaries.
    /// </summary>
    protected virtual void OnFlewOutOfStage() {}

    /// <summary>
    /// Called when the object is added to the stage.
    /// </summary>
    public virtual void OnAdded()
    {
        OnCollision += OnCollisionValidating;
    }

    /// <summary>
    /// Called when the object is removed from the scene.
    /// </summary>
    public virtual void OnRemoved() {}

    /// <summary>
    /// Called when the physics engine detects a collision with another game object.
    /// </summary>
    /// <param name="sender">The colliding fixture of this object.</param>
    /// <param name="other">The colliding fixture of other object.</param>
    /// <param name="contact">The collision details.</param>
    /// <returns>True if the collision is approved.</returns>
    protected virtual bool OnCollisionValidating(Fixture sender, Fixture other, Contact contact)
    {
        return true;
    }
}