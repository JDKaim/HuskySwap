﻿using Humanizer;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.AssemblyManager
{
    /// <summary>
    /// Represents the details of a type property.
    /// </summary>
    public class TypePropertyDetails
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public required Type Type { get; set; }

        /// <summary>
        /// The property name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Camel case.
        /// </summary>
        public required string CamelName { get; set; }

        /// <summary>
        /// The string representation of the property type for the server (C#).
        /// </summary>
        public required string ServerTypeString { get; set; }

        /// <summary>
        /// The string representation of the property type for the client (Typescript).
        /// </summary>
        public required string ClientTypeString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypePropertyDetails"/> class.
        /// </summary>
        /// <param name="type">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        [SetsRequiredMembers]
        public TypePropertyDetails(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
            this.CamelName = this.Name.Camelize();
            this.ServerTypeString = TypePropertyDetails.GetServerTypeString(type);
            this.ClientTypeString = TypePropertyDetails.GetClientTypeString(type);
        }

        /// <summary>
        /// Gets the server type string for the specified type.
        /// </summary>
        /// <param name="type">The type to get the server type string for.</param>
        /// <returns>The C# type string.</returns>
        public static string GetServerTypeString(Type type)
        {
            if (type == typeof(int)) { return "int"; }
            if (type == typeof(long)) { return "long"; }
            if (type == typeof(double)) { return "double"; }
            if (type == typeof(float)) { return "float"; }
            if (type == typeof(decimal)) { return "decimal"; }
            if (type == typeof(short)) { return "short"; }
            if (type == typeof(byte)) { return "byte"; }
            if (type == typeof(ushort)) { return "ushort"; }
            if (type == typeof(uint)) { return "uint"; }
            if (type == typeof(ulong)) { return "ulong"; }
            if (type == typeof(bool)) { return "bool"; }
            if (type == typeof(char)) { return "char"; }
            if (type == typeof(string)) { return "string"; }
            if (type == typeof(Guid)) { return "Guid"; }
            if (type == typeof(DateTime)) { return "DateTime"; }
            return type.Name;
        }

        /// <summary>
        /// Gets the client type string for the specified type.
        /// </summary>
        /// <param name="type">The type to get the client type string for.</param>
        /// <returns>The TypeScript type string.</returns>
        public static string GetClientTypeString(Type type)
        {
            if (type == typeof(int) ||
                type == typeof(long) ||
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(decimal) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(ushort) ||
                type == typeof(uint) ||
                type == typeof(ulong))
            {
                return "number";
            }
            return "string";
        }
    }
}
