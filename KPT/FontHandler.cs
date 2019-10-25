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

        private Dictionary<int, int> glyphWidths;
        
        PGFFont font;
        string fontSource;
        string fontDestination;

        public FontHandler()
        {
            fontSource = Path.Combine(ProjectFolder.rootDir, ProjectFolder.unpackedGameFilesDir, DEFAULT_FONT_FILE);
            fontDestination = Path.Combine(ProjectFolder.rootDir, ProjectFolder.reassembledGameFilesDir, DEFAULT_FONT_FILE);
            font = new PGFFont(fontSource);
            glyphWidths = new Dictionary<int, int>();
        }

        public void ChangeSpaceSize(int newSize)
        {
            PGFGlyph space = font.GetGlyphByIndex(0x20);
            space.width = DEFAULT_SPACE_SIZE;
            space.SaveGylph();

            font.SaveFont(fontDestination);
        }

        public int GetGlyphWidthByUcs(int ucs)
        {

            int width = 0;
            bool success;

            success = glyphWidths.TryGetValue(ucs, out width);

            if (success)
            {
                return width;
            }

            PGFGlyph glyph = font.GetGlyphByUcs(ucs);

            if (glyph == null)
            {
                return DEFAULT_SPACE_SIZE;
            }

            glyphWidths[ucs] = width;

            return width;

        }
        
        public int CalcuateSegmentWidth(string segment)
        {
            int length = 0;

            foreach (char letter in segment)
            {
                int ucs = Convert.ToInt32(letter);

               
                length += GetGlyphWidthByUcs(ucs);
            }

            return length;
        }
 
    }
}
