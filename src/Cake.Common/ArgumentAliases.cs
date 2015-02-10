﻿using System;
using System.ComponentModel;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to arguments.
    /// </summary>
    [CakeAliasCategory("Arguments")]
    public static class ArgumentAliases
    {
        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>Whether or not the specified argument exist.</returns>
        /// <example>
        /// This sample shows how to call the <see cref="HasArgument"/> method.
        /// <code>
        /// var argumentName = "myArgument";
        /// //Cake.exe .\hasargument.cake -myArgument="is specified"
        /// if (HasArgument(argumentName))
        /// {
        ///     Information("{0} is specified", argumentName);
        /// }
        /// //Cake.exe .\hasargument.cake
        /// else
        /// {
        ///     Warning("{0} not specified", argumentName);
        /// } 
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static bool HasArgument(this ICakeContext context, string name)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.Arguments.HasArgument(name);
        }

        /// <summary>
        /// Gets an argument and throws if the argument is missing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>The value of the argument.</returns>
        /// <example>
        /// <code>
        /// //Cake.exe .\argument.cake -myArgument="is valid" -loopCount = 5
        /// Information("Argument {0}", Argument&lt;string&gt;("myArgument"));
        /// var loopCount = Argument&lt;int&gt;("loopCount");
        /// for(var index = 0;index&lt;loopCount; index++)
        /// {
        ///     Information("Index {0}", index);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var value = context.Arguments.GetArgument(name);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

        /// <summary>
        /// Gets an argument and returns the provided <paramref name="defaultValue"/> if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValue">The value to return if the argument is missing.</param>
        /// <returns>The value of the argument if it exist; otherwise <paramref name="defaultValue"/>.</returns>
        /// <example>
        /// <code>
        /// //Cake.exe .\argument.cake -myArgument="is valid" -loopCount = 5
        /// Information("Argument {0}", Argument&lt;string&gt;("myArgument", "is NOT valid"));
        /// var loopCount = Argument&lt;int&gt;("loopCount", 10);
        /// for(var index = 0;index&lt;loopCount; index++)
        /// {
        ///     Information("Index {0}", index);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name, T defaultValue)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var value = context.Arguments.GetArgument(name);
            return value == null
                ? defaultValue
                : Convert<T>(value);
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}