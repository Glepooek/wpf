﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Windows.Documents;
using System.Windows.Threading;

namespace System.Windows.Xps.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    internal class ReachDocumentReferenceSerializer :
                   ReachSerializer
    {
        /// <summary>
        /// Creates a new serailizer for a DocumentReference
        /// </summary>
        /// <param name="manager">serialization manager</param>
        public
        ReachDocumentReferenceSerializer(
            PackageSerializationManager   manager
            ):
        base(manager)
        {
            
        }

        private object Idle(object sender)
        {
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        internal
        override
        void
        PersistObjectData(
            SerializableObjectContext   serializableObjectContext
            )
        {
            if(serializableObjectContext.IsComplexValue)
            {
                SerializeObjectCore(serializableObjectContext);

                // Loads the document
                FixedDocument document = ((DocumentReference)serializableObjectContext.TargetObject).GetDocument(false);

                if (!document.IsInitialized)
                {
                    // Give a parser item a kick
                    document.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                        new DispatcherOperationCallback(Idle), null);
                }

                if(document != null)
                {
                    ReachSerializer serializer = SerializationManager.GetSerializer(document);

                    if(serializer!=null)
                    {
                        serializer.SerializeObject(document);
                    }
                    else
                    {
                        // This shouldn't ever happen.
                        throw new XpsSerializationException(SR.ReachSerialization_NoSerializer);
                    }
                }
            }
            else
            {
                // What about this case?  Is IsComplexValue something we really want to check for this?
            }
        }
    };
}
