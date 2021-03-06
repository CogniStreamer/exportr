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

using System;
using System.IO;
using System.Linq;

namespace Exportr
{
    /// <summary>
    /// Takes an export task, proposes a filename and writes it to a stream.
    /// </summary>
    public sealed class FileStreamExporter : IFileStreamExporter
    {
        private readonly IDocumentFactory _documentFactory;
        private readonly IExportTask _task;

        /// <summary>
        /// Constructs a new <see cref="FileStreamExporter"/> instance.
        /// </summary>
        /// <param name="documentFactory">The export document factory.</param>
        /// <param name="task">The export task.</param>
        public FileStreamExporter(IDocumentFactory documentFactory, IExportTask task)
        {
            _documentFactory = documentFactory ?? throw new ArgumentNullException(nameof(documentFactory));
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        /// <summary>
        /// Gets the proposed filename.
        /// </summary>
        /// <returns>The proposed filename.</returns>
        public string GetFileName()
        {
            var name = _task.Name;
            if (string.IsNullOrEmpty(name)) throw new InvalidOperationException("Failed to get the name of the export task");
            var extension = _documentFactory.FileExtension ?? string.Empty;
            if (!extension.StartsWith(".")) extension = $".{extension}";
            return $"{FileNameEncode(name)} {DateTime.Now:yyyyMMdd}{extension}";
        }

        /// <summary>
        /// Exports the export data to the given stream.
        /// </summary>
        /// <param name="stream">The stream to write the export to.</param>
        public void ExportToStream(Stream stream)
        {
            using (var document = _documentFactory.CreateDocument(stream))
            {
                foreach (var sheetTask in _task.EnumSheetExportTasks())
                {
                    using (var sheet = document.CreateSheet(sheetTask.Name))
                    {
                        sheet.AddHeaderRow(sheetTask.GetColumnLabels());

                        foreach (var rowData in sheetTask.EnumRowData())
                        {
                            sheet.AddRow(rowData);
                        }
                    }
                }
            }
        }

        private static string FileNameEncode(string value)
            => Path.GetInvalidFileNameChars().Aggregate(value, (seed, invalidChar) => seed.Replace(invalidChar, '_'));
    }
}
