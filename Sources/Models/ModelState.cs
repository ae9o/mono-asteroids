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
/// Defines a list of all model states.
/// </summary>
public enum ModelState
{
    /// <summary>
    /// The model can be in this state only after the start of the app, when not a single round has been played.
    /// </summary>
    Fresh,

    RoundStarted,
    RoundFinished
}