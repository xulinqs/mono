//
// System.Security.Permissions.StrongNameIdentityPermissionAttribute.cs
//
// Authors:
//	Duncan Mak <duncan@ximian.com>
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002 Ximian, Inc.			http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions {

#if NET_2_0
	[ComVisible (true)]
#endif
	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Class |
			 AttributeTargets.Struct | AttributeTargets.Constructor |
			 AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
	[Serializable]
	public sealed class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute	{

		// Fields
		private string name;
		private string key;
		private string version;
		
		// Constructor
		public StrongNameIdentityPermissionAttribute (SecurityAction action) 
			: base (action)
		{
		}
		
		// Properties
		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string PublicKey {
			get { return key; }
			set { key = value; }
		}

		public string Version {
			get { return version; }
			set { version = value; }
		}
			 
		// Methods
		public override IPermission CreatePermission ()
		{
#if NET_2_1
			return null;
#else
			if (this.Unrestricted)
				return new StrongNameIdentityPermission (PermissionState.Unrestricted);

			if ((name == null) && (key == null) && (version == null))
				return new StrongNameIdentityPermission (PermissionState.None);

			if (key == null) {
				throw new ArgumentException (Locale.GetText (
					"PublicKey is required"));
			}
			StrongNamePublicKeyBlob blob = StrongNamePublicKeyBlob.FromString (key);
				
			Version v = null;
			if (version != null)
				v = new Version (version);

			return new StrongNameIdentityPermission (blob, name, v);
#endif
		}
	}
}
