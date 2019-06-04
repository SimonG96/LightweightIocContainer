// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The registration of a Factory is not valid
    /// </summary>
    public class InvalidFactoryRegistrationException : InvalidRegistrationException
    {
        public InvalidFactoryRegistrationException(string message)
            : base(message)
        {

        }
    }
}