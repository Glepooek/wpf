// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/***************************************************************************\
*
*
*  Class for Xaml markup extension for static resource references.
*
*
\***************************************************************************/
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal; // Helper

namespace System.Windows
{

    /// <summary>
    ///  Class for Xaml markup extension for static resource references.
    /// </summary>
    [TypeConverter(typeof(DynamicResourceExtensionConverter))]
    [MarkupExtensionReturnType(typeof(object))]
    public class DynamicResourceExtension : MarkupExtension
    {
        /// <summary>
        ///  Constructor that takes no parameters
        /// </summary>
        public DynamicResourceExtension()
        {
        }

        /// <summary>
        ///  Constructor that takes the resource key that this is a static reference to.
        /// </summary>
        public DynamicResourceExtension(
            object resourceKey)
        {
            ArgumentNullException.ThrowIfNull(resourceKey);
            _resourceKey = resourceKey;
        }


        /// <summary>
        ///  Return an object that should be set on the targetObject's targetProperty
        ///  for this markup extension.  For DynamicResourceExtension, this is the object found in
        ///  a resource dictionary in the current parent chain that is keyed by ResourceKey
        /// </summary>
        /// <returns>
        ///  The object to set on this property.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourceKey == null)
            {
                throw new InvalidOperationException(SR.MarkupExtensionResourceKey);
            }

            if (serviceProvider != null)
            {
                // DynamicResourceExtensions are not allowed On CLR props except for Setter,Trigger,Condition (bugs 1183373,1572537)

                DependencyObject targetDependencyObject;
                DependencyProperty targetDependencyProperty;
                Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out targetDependencyObject, out targetDependencyProperty);
            }

            return new ResourceReferenceExpression(ResourceKey);
        }



        /// <summary>
        ///  The key in a Resource Dictionary used to find the object refered to by this
        ///  Markup Extension.
        /// </summary>
        [ConstructorArgument("resourceKey")] // Uses an instance descriptor
        public object ResourceKey
        {
            get { return _resourceKey; }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _resourceKey = value;
            }
        }

        private object _resourceKey;



    }
}


