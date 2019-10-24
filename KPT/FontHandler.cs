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
            glyphWidths = BuildGlypthWidthDictionary();
        }

        public void ChangeSpaceSize(int newSize)
        {
            font = new PGFFont(fontSource);

            PGFGlyph space = font.GetGlyphByIndex(0x20);
            space.width = DEFAULT_SPACE_SIZE;
            space.SaveGylph();

            font.SaveFont(fontDestination);
        }

        private Dictionary<int, int> BuildGlypthWidthDictionary()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            font = new PGFFont(fontSource);

            for (int i = 0; i < 65536; i++) // Maximum number of glyphs in PGF - this is contained in a property in libpgf-csharp but said property is not accessible outside its own assembly. May change this.
            {

                PGFGlyph glyph = font.GetGlyphByUcs(i);
                if (glyph == null)
                {
                    dict[i] = DEFAULT_SPACE_SIZE;
                }
                else
                {
                    dict[i] = glyph.width;
                }

            }

            return dict;
        }
        
        public int CalcuateSegmentWidth(string segment)
        {
            int length = 0;

            foreach (char letter in segment)
            {
                int ucs = Convert.ToInt32(letter);
                length += glyphWidths[ucs];
            }

            return length;
        }
 
    }
}
