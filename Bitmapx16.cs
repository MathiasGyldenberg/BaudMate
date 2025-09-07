/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

namespace BaudMate
{
    /// <summary>
    /// Creates a custom square 10x10 bitmap
    /// </summary>
    internal static class CustomBitmap
    {
        internal static Bitmap GetIcon(Color color)
        {
            Bitmap bitmap = new(10, 10);
            using Graphics g = Graphics.FromImage(bitmap);
            g.Clear(color);
            return bitmap;
        }
    }
}
