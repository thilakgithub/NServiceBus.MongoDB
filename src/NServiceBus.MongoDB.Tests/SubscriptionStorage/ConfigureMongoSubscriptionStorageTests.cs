﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureMongoSubscriptionStorageTests.cs" company="Carlos Sandoval">
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
//   Defines the ConfigureMongoSubscriptionStorageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NServiceBus.MongoDB.Tests.SubscriptionStorage
{
    using FluentAssertions;
    using NServiceBus.MongoDB.SubscriptionStorage;
    using NServiceBus.MongoDB.Tests.TestingUtilities;
    using Xunit.Extensions;

    public class ConfigureMongoSubscriptionStorageTests
    {
        [Theory, UnitTest]
        [AutoConfigureData]
        public void MongoSubscriptionStorageCalledOnce(Configure config)
        {
            config.MongoSubscriptionStorage();

            Configure.Instance.Configurer.HasComponent<MongoClientAccessor>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoDatabaseFactory>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoSubscriptionStorage>().Should().BeTrue();
        }

        [Theory, UnitTest]
        [AutoConfigureData]
        public void MongoSubscriptionStorageAfterPersistenceConfigure(Configure config)
        {
            config.MongoPersistence();
            Configure.Instance.Configurer.HasComponent<MongoClientAccessor>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoDatabaseFactory>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoSubscriptionStorage>().Should().BeFalse();

            config.MongoSubscriptionStorage();
            Configure.Instance.Configurer.HasComponent<MongoSubscriptionStorage>().Should().BeTrue();
        }

        [Theory, UnitTest]
        [AutoConfigureData]
        public void MongoSubscriptionStorageCalledTwice(Configure config)
        {
            config.MongoSubscriptionStorage();

            Configure.Instance.Configurer.HasComponent<MongoClientAccessor>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoDatabaseFactory>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoSubscriptionStorage>().Should().BeTrue();

            config.MongoSubscriptionStorage();

            Configure.Instance.Configurer.HasComponent<MongoClientAccessor>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoDatabaseFactory>().Should().BeTrue();
            Configure.Instance.Configurer.HasComponent<MongoSubscriptionStorage>().Should().BeTrue();
        }
    }
}