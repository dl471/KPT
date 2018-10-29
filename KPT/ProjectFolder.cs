using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPT
{
    // this is just a shell of a class at the moment that will become an important reference point for the UI later - it is possible that the directory structure from the Dumper will be moved here as well
    static class ProjectFolder
    {
        public static string rootDir = @"H:\kokoro_connect\BuildingTest";

        public static string GetRootDir()
        {
            return rootDir;
        }

    }
}
