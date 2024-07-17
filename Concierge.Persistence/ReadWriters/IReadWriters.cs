// <copyright file="IReadWriters.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for reading and writing data to and from files.
    /// </summary>
    public interface IReadWriters
    {
        /// <summary>
        /// Reads a list of objects from the specified file path.
        /// </summary>
        /// <typeparam name="T">The type of objects to read.</typeparam>
        /// <param name="filePath">The file path to read from.</param>
        /// <returns>A list of objects read from the file.</returns>
        public List<T> ReadList<T>(string filePath);

        /// <summary>
        /// Reads a list of objects from the specified byte array.
        /// </summary>
        /// <typeparam name="T">The type of objects to read.</typeparam>
        /// <param name="file">The byte array containing the data to read.</param>
        /// <returns>A list of objects read from the byte array.</returns>
        public List<T> ReadList<T>(byte[] file);

        /// <summary>
        /// Reads a JSON object from the specified file path.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="filePath">The file path to read from.</param>
        /// <returns>The deserialized object from the JSON data.</returns>
        /// <remarks>If the file does not exist, a new instance of the object will be returned.</remarks>
        public T ReadJson<T>(string filePath)
            where T : new();

        /// <summary>
        /// Reads a JSON object from the specified byte array.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="file">The byte array containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON data.</returns>
        /// <remarks>If the byte array is empty or null, a new instance of the object will be returned.</remarks>
        public T ReadJson<T>(byte[] file)
            where T : new();

        /// <summary>
        /// Writes a list of objects to the specified file path.
        /// </summary>
        /// <typeparam name="T">The type of objects to write.</typeparam>
        /// <param name="filePath">The file path to write to.</param>
        /// <param name="value">The list of objects to write.</param>
        public void WriteList<T>(string filePath, List<T> value);

        /// <summary>
        /// Writes an object to the specified file path in JSON format.
        /// </summary>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="filePath">The file path to write to.</param>
        /// <param name="value">The object to write.</param>
        /// <returns><c>true</c> if the write operation is successful, otherwise <c>false</c>.</returns>
        public bool WriteJson<T>(string filePath, T value);

        /// <summary>
        /// Appends an object to the end of the specified file.
        /// </summary>
        /// <typeparam name="T">The type of object to append.</typeparam>
        /// <param name="filePath">The file path to append to.</param>
        /// <param name="value">The object to append.</param>
        public void Append<T>(string filePath, T value);

        /// <summary>
        /// Clears the contents of the specified file.
        /// </summary>
        /// <param name="filePath">The file path to clear.</param>
        public void Clear(string filePath);

        /// <summary>
        /// Reads anything from the specified file path.
        /// </summary>
        /// <returns><c>true</c> if the read operation is successful, otherwise <c>false</c>.</returns>
        public bool Read(string filePath);

        /// <summary>
        /// Writes anything to the specified file path.
        /// </summary>
        /// <returns><c>true</c> if the write operation is successful, otherwise <c>false</c>.</returns>
        public bool Write(string filePath);
    }
}
