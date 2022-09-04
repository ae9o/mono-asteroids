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
using MonoGame.Extended.Collections;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoAsteroids;

/// <summary>
/// Contains all objects placed on the stage.
/// </summary>
public class Stage : IEnumerable<GameObject>
{
    public const float Width = 1.66f;
    public const float Height = 1f;

    public static readonly Vector2 Center = new Vector2(Width * 0.5f, Height * 0.5f);
    public static readonly Vector2 UpDirection = -Vector2.UnitY;

    private readonly World _world;
    private readonly Bag<GameObject> _addedGameObjects = new Bag<GameObject>();
    private readonly Bag<GameObject> _removedGameObjects = new Bag<GameObject>();
    private bool _locked;

    // Use a HashSet to store objects instead of a Bag. Bag has O(N) complexity to remove objects, while HashSet is O(1)
    // to both add and remove.
    private readonly HashSet<GameObject> _gameObjects = new HashSet<GameObject>();

    public Stage()
    {
        _world = new World();
        _world.Gravity = Vector2.Zero;
    }

    /// <summary>
    /// Temporarily delays the ability to remove and add game objects to the stage. During locking, objects are added to
    /// a temporary container, from which they will be transferred to the stage when the lock is released. Likewise for
    /// removal. This is necessary for the correct operation of the physics engine, since it does not allow
    /// modifications during the operation of the Step method.
    /// </summary>
    private void Lock()
    {
        _locked = true;
    }

    /// <summary>
    /// Releases the lock and transfers the game objects added during the lock from the temporary container to the
    /// stage. Likewise for removal.
    /// </summary>
    private void Unlock()
    {
        _locked = false;

        foreach (var obj in _removedGameObjects)
        {
            Remove(obj);
        }
        _removedGameObjects.Clear();

        foreach (var obj in _addedGameObjects)
        {
            Add(obj);
        }
        _addedGameObjects.Clear();
    }

    public void Add(GameObject obj)
    {
        if (_locked)
        {
            _addedGameObjects.Add(obj);
        }
        else
        {
            _gameObjects.Add(obj);
            _world.Add(obj);
            obj.OnAdded();
        }
    }

    public void Remove(GameObject obj)
    {
        if (_locked)
        {
            _removedGameObjects.Add(obj);
        }
        else if (_gameObjects.Remove(obj))
        {
            _world.Remove(obj);
            obj.OnRemoved();
        }
    }

    public void Clear()
    {
        foreach (var obj in _gameObjects)
        {
            obj.OnRemoved();
        }
        _gameObjects.Clear();
        _world.Clear();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var obj in _gameObjects)
        {
            obj.Update(gameTime);
        }

        // Block the ability to add and remove objects on the stage while the physics engine is running.
        Lock();
        _world.Step(gameTime.ElapsedGameTime);
        Unlock();
    }

    IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
    {
        return ((IEnumerable<GameObject>)_gameObjects).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return ((System.Collections.IEnumerable)_gameObjects).GetEnumerator();
    }
}