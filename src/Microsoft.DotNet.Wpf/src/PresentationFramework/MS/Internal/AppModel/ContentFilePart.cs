// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// Description:
// ContentFilePart is an implementation of the abstract PackagePart class. It contains an override for GetStreamCore.
//

using System.IO.Packaging;
using System.Windows;
using System.IO;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
    /// <summary>
    /// ContentFilePart is an implementation of the abstract PackagePart class. It contains an override for GetStreamCore.
    /// </summary>
    internal class ContentFilePart : System.IO.Packaging.PackagePart
    {
        //------------------------------------------------------
        //
        //  Public Constructors
        //
        //------------------------------------------------------

        #region Public Constructors

        internal ContentFilePart(Package container, Uri uri) :
                base(container, uri)
        {
            Invariant.Assert(Application.ResourceAssembly != null, "If the entry assembly is null no ContentFileParts should be created");
            _fullPath = null;
        }

        #endregion

        //------------------------------------------------------
        //
        //  Protected Methods
        //
        //------------------------------------------------------

        #region Protected Methods

        protected override Stream GetStreamCore(FileMode mode, FileAccess access)
        {
            Stream stream = null;

            if (_fullPath == null)
            {
                // File name will be a path relative to the applications directory.
                // - We do not want to use SiteOfOriginContainer.SiteOfOrigin because
                //   for deployed files the <Content> files are deployed with the application.
                string location = GetEntryAssemblyLocation();

                string assemblyName, assemblyVersion, assemblyKey;
                string filePath;

                // For now, only Application assembly supports content files, 
                // so we can simply ignore the assemblyname etc.
                // In the future, we may extend this support for regular library assembly,
                // assemblyName will be used to predict the right file path.

                BaseUriHelper.GetAssemblyNameAndPart(Uri, out filePath, out assemblyName, out assemblyVersion, out assemblyKey);

                // filePath should not have leading slash.  GetAssemblyNameAndPart( ) can guarantee it.
                _fullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(location), filePath);
            }

            stream = CriticalOpenFile(_fullPath);

            if (stream == null)
            {
                throw new IOException(SR.Format(SR.UnableToLocateResource, Uri.ToString()));
            }

            return stream;
        }

        protected override string GetContentTypeCore()
        {
            return MS.Internal.MimeTypeMapper.GetMimeTypeFromUri(new Uri(Uri.ToString(), UriKind.RelativeOrAbsolute)).ToString();
        }

        #endregion

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private Methods


        private string GetEntryAssemblyLocation()
        {
            string entryLocation = null;

            try
            {
                entryLocation = Application.ResourceAssembly.Location;
            }
            catch (Exception ex)
            {
                if (CriticalExceptions.IsCriticalException(ex))
                {
                    throw;
                }
                // `Swallow any other exceptions to avoid disclosing the critical path.
                // 
                // Possible Exceptions: ArgumentException, ArgumentNullException, PathTooLongException
                // DirectoryNotFoundException, IOException, UnauthorizedAccessException, 
                // ArgumentOutOfRangeException, FileNotFoundException, NotSupportedException
            }

            return entryLocation;
        }

        private Stream CriticalOpenFile(string filename)
        {
            return System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, ResourceContainer.FileShare);
        }

        #endregion

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private Members

        private string _fullPath;

        #endregion Private Members
    }
}

