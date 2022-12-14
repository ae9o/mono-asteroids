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

namespace MonoAsteroids;

/// <summary>
/// Base class for game objects intended for pooling.
/// </summary>
public abstract class PoolableGameObject : GameObject, IPoolable
{
    private ReturnToPoolDelegate _returnToPoolDelegate;

    public void SetReturnToPoolDelegate(ReturnToPoolDelegate returnToPoolDelegate)
    {
        _returnToPoolDelegate = returnToPoolDelegate;
    }

    public void ReturnToPool()
    {
        _returnToPoolDelegate?.Invoke(this);
    }

    public abstract void Reset();
}