﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureMongoTimeoutPersister.cs" company="Carlos Sandoval">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Carlos Sandoval
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The configure mongo timeout persister.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NServiceBus.MongoDB.TimeoutPersister
{
    using System;
    using System.Diagnostics.Contracts;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Bson.Serialization.Options;
    using NServiceBus.Timeout.Core;

    /// <summary>
    /// The configure mongo timeout persister.
    /// </summary>
    public static class ConfigureMongoTimeoutPersister
    {
        /// <summary>
        /// The mongo timeout persister.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="Configure"/>.
        /// </returns>
        public static Configure MongoTimeoutPersister(this Configure config)
        {
            Contract.Requires<ArgumentNullException>(config != null);
            Contract.Ensures(Contract.Result<Configure>() != null);

            if (!config.Configurer.HasComponent<MongoClientAccessor>())
            {
                config.MongoPersistence();
            }

            config.Configurer.ConfigureComponent<MongoTimeoutPersister>(DependencyLifecycle.SingleInstance);

            ConfigureClassMaps();

            return config;
        }

        internal static void ConfigureClassMaps()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(TimeoutData)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<TimeoutData>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(mm => mm.Time)
                          .SetSerializationOptions(
                              new DateTimeSerializationOptions(DateTimeKind.Utc, BsonType.Document));
                    });
        }
    }
}