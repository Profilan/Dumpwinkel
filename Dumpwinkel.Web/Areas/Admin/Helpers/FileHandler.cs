using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Areas.Admin.Helpers
{
    public class FileHandler
    {
        internal static bool Save(string path, string physicalPath)
        {
            try
            {
                File.Copy(path, physicalPath);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}