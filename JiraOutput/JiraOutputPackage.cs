using System;
using System.Runtime.InteropServices;
using SNAGITLib;

namespace JiraOutput
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("BC12E5B0-EF60-46B7-B002-FAAD356F5AEC")]
    public class JiraOutputPackage : MarshalByRefObject, IPackageSetup
    {
        [ComVisible(true)]
        public void Install()
        {
        }

        [ComVisible(true)]
        public void Uninstall()
        {
        }
    }
}