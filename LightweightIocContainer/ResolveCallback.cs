// Author: Gockner, Simon
// Created: 2019-10-15
// Copyright(c) 2019 SimonG. All Rights Reserved.

namespace LightweightIocContainer
{
    /// <summary>
    /// The resolve callback delegate
    /// </summary>
    public delegate TInterface ResolveCallback<out TInterface>(params object[] parameters);
}