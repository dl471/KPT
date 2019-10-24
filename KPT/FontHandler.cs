using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using libpgf_csharp;
using System.IO;

namespace KPT
{
    class FontHandler
    {

        public static int DEFAULT_SPACE_SIZE = 5;
        public static string DEFAULT_FONT_FILE = @"PSP_GAME\USRDIR\SysFont\FontDat.pgf";
        
        PGFFont font;
        string fontSource;
        string fontDestination;

        public FontHandler()
        {
            fontSource = Path.Combine(ProjectFolder.rootDir, ProjectFolder.unpackedGameFilesDir, DEFAULT_FONT_FILE);
            fontDestination = Path.Combine(ProjectFolder.rootDir, ProjectFolder.reassembledGameFilesDir, DEFAULT_FONT_FILE);
        }

        public void ChangeSpaceSize(int newSize)
        {
            font = new PGFFont(fontSource);

            PGFGlyph space = font.GetGlyphByIndex(0x20);
            space.width = DEFAULT_SPACE_SIZE;
            space.SaveGylph();

            font.SaveFont(fontDestination);

        }
 
    }
}
