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

namespace MonoAsteroids;

/// <summary>
/// This delegate is called by an object that wants to return to the parent pool.
/// </summary>
/// <param name="item">Object to be returned to the pool.</param>
public delegate void ReturnToPoolDelegate(IPoolable item);

/// <summary>
///
/// <para>This is a wrapper over a Pool from the MonoGame.Extended library, which allows to get functionality similar to
/// the functionality of the ObjectPool class from the same MonoGame.Extended library.</para>
///
/// <para>The question is, why not just immediately use this ObjectPool?<para>
///
/// <para>And because it's broken. Here you can read: https://github.com/craftworkgames/MonoGame.Extended/issues/471
/// </para>
///
/// <para>For 4 years now (OMG!) they can't fix the NullReferenceException when returning objects.</para>
///
/// </summary>
/// /// <typeparam name="T">Poolable object type.</typeparam>
public class Pool<T>
    where T : class, IPoolable
{
    private readonly MonoGame.Extended.Collections.Pool<T> _pool;
    private readonly ReturnToPoolDelegate _returnToPoolDelegate;

    public Pool(Func<T> createItem, int capacity = 16, int maximum = int.MaxValue)
    {
        _pool = new MonoGame.Extended.Collections.Pool<T>(createItem, capacity, maximum);
        _returnToPoolDelegate = Return;
    }

    public T Obtain()
    {
        var item = _pool.Obtain();
        item.SetReturnToPoolDelegate(_returnToPoolDelegate);
        return item;
    }

    public void Return(IPoolable item)
    {
        item.SetReturnToPoolDelegate(null);
        item.Reset();
        _pool.Free((T)item);
    }
}