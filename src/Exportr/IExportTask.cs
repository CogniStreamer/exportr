﻿/*
 * Copyright 2017 Wouter Huysentruit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;

namespace Exportr
{
    /// <summary>
    /// Interface that describes an export task.
    /// </summary>
    public interface IExportTask
    {
        /// <summary>
        /// Gets a name for the export (used by <see cref="FileStreamExporter"/> to generate a filename.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the file extension to use when the output is a file. (used by <see cref="FileStreamExporter"/> to generate a filename.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Enumerates all available sheet export tasks for the current export task.
        /// </summary>
        /// <returns>Enumeration of sheet export tasks</returns>
        IEnumerable<ISheetExportTask> EnumSheetExportTasks();
    }
}