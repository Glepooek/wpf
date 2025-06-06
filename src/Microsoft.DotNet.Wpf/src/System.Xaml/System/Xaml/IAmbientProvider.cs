﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace System.Xaml
{
    public interface IAmbientProvider
    {
        AmbientPropertyValue GetFirstAmbientValue(IEnumerable<XamlType> ceilingTypes,
                                                    params XamlMember[] properties);
        object GetFirstAmbientValue(params XamlType[] types);

        IEnumerable<AmbientPropertyValue> GetAllAmbientValues(IEnumerable<XamlType> ceilingTypes,
                                                    params XamlMember[] properties);

        IEnumerable<object> GetAllAmbientValues(params XamlType[] types);

        IEnumerable<AmbientPropertyValue> GetAllAmbientValues(IEnumerable<XamlType> ceilingTypes,
                                                              bool searchLiveStackOnly,
                                                              IEnumerable<XamlType> types,
                                                              params XamlMember[] properties);
    }

    public class AmbientPropertyValue
    {
        private XamlMember _property;
        private object _value;

        public AmbientPropertyValue(XamlMember property, object value)
        {
            _property = property;
            _value = value;
        }

        public object Value { get { return _value; } }
        public XamlMember RetrievedProperty { get { return _property; }  }
    }
}
