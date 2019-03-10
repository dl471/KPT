using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Elements
{
    class SpriteInfo : IElement
    {

        public enum Characters { Taichi = 0, Inaba = 1, Iori = 2, Yui = 3, Yoshifumo = 4, Mariko = 9}

        /// <summary>
        /// The character which the sprite is of
        /// </summary>
        byte character;
        /// <summary>
        /// The specific sprite of said character that should be displayed
        /// </summary>
        byte sprite;
        /// <summary>
        /// Where on the screen the sprite should be displayed
        /// </summary>
        byte position;
        /// <summary>
        /// Purpose unknown - seems to almost always be zero?
        /// </summary>
        byte unknown;

        public bool Read(BinaryReader br)
        {
            character = br.ReadByte();
            sprite = br.ReadByte();
            position = br.ReadByte();
            unknown = br.ReadByte();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(character);
            bw.Write(sprite);
            bw.Write(position);
            bw.Write(unknown);
            return true;
        }

    }
}
