﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Windows.Markup.Tests;

public class RuntimeNamePropertyAttributeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("name")]
    public void Ctor_String(string? name)
    {
        var attribute = new RuntimeNamePropertyAttribute(name);
        Assert.Equal(name, attribute.Name);
    }
}
