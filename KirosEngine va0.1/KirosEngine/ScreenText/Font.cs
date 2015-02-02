using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using KirosEngine.Textures;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.ScreenText
{
    struct FontType
    {
        public float x, y;
        public int width, height;
    }

    class Font
    {
        private FontType[] _fontArray;
        private Texture _texture;
        private Device _device;
        private float _bitmapHeight;
        private float _bitmapWidth;

        private int _fontSize;

        public void Initialize(Device device, string fontFileName, string textureFileName, int fontSize)
        {
            _device = device;
            LoadTexture(textureFileName);
            _bitmapHeight = _texture.GetTextureHeight();
            _bitmapWidth = _texture.GetTextureWidth();
            _fontSize = fontSize;

            LoadFontData(fontFileName);
        }

        /// <summary>
        /// load the font data with the given filename
        /// </summary>
        /// <param name="fileName">file to load from</param>
        protected virtual void LoadFontData(string fileName)
        {
            //TODO: replace with a function that can handle multiple file types
            //array length is number of characters in font
            _fontArray = new FontType[95];
            string[] lines = File.ReadAllLines(fileName);

            int index = 0;
            foreach (string line in lines)
            {
                string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                _fontArray[index].x = float.Parse(split[2].Split('=')[1]) / _bitmapWidth;
                _fontArray[index].y = float.Parse(split[3].Split('=')[1]) / _bitmapHeight;
                _fontArray[index].width = int.Parse(split[4].Split('=')[1]);
                _fontArray[index].height = int.Parse(split[5].Split('=')[1]);
                index++;
            }
        }

        /// <summary>
        /// load the texture
        /// </summary>
        /// <param name="fileName">the filename of the texture to load</param>
        /// <returns>true if successful</returns>
        private bool LoadTexture(string fileName)
        {
            _texture = new Texture(fileName, "font");
            _texture.Initialize(_device);

            return true;
        }

        /// <summary>
        /// Returns the texture as a ShaderResourceView
        /// </summary>
        /// <returns></returns>
        public ShaderResourceView GetTexture()
        {
            return _texture.GetTexture();
        }

        /// <summary>
        /// Build the vertex array for the given text
        /// </summary>
        /// <param name="vertexCount">number of vertices</param>
        /// <param name="sentence">the text to be used</param>
        /// <param name="positionX">the starting x position</param>
        /// <param name="positionY">the starting y position</param>
        /// <returns></returns>
        public virtual Vertex2D[] BuildVertexArrayF(int vertexCount, string sentence, float positionX, float positionY)
        {
            //TODO: look into rework(needed or not?)
            Vertex2D[] vertices = new Vertex2D[vertexCount];
            int numLetters, index;
            int letter;

            numLetters = sentence.Length;
            index = 0;

            for (int i = 0; i < numLetters; i++)
            {
                letter = sentence.ToCharArray()[i] - 32;

                if (letter == 0)
                {
                    positionX = positionX + 3.0f;
                }
                else
                {
                    //tri 1
                    //top left
                    vertices[index].position = new Vector3(positionX, positionY, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x, _fontArray[letter].y);
                    index++;

                    //bottom right
                    vertices[index].position = new Vector3(positionX + _fontArray[letter].width, positionY - _fontArray[letter].height, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x + (float)(_fontArray[letter].width / _bitmapWidth), _fontArray[letter].y + (float)(_fontArray[letter].height / _bitmapHeight));
                    index++;

                    //bottom left
                    vertices[index].position = new Vector3(positionX, positionY - _fontArray[letter].height, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x, _fontArray[letter].y + (float)(_fontArray[letter].height / _bitmapHeight));
                    index++;

                    //tri 2
                    //top left
                    vertices[index].position = new Vector3(positionX, positionY, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x, _fontArray[letter].y);
                    index++;

                    //top right
                    vertices[index].position = new Vector3(positionX + _fontArray[letter].width, positionY, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x + (float)(_fontArray[letter].width / _bitmapWidth), _fontArray[letter].y);
                    index++;

                    //bottom right
                    vertices[index].position = new Vector3(positionX + _fontArray[letter].width, positionY - _fontArray[letter].height, 0.0f);
                    vertices[index].texture = new Vector2(_fontArray[letter].x + (float)(_fontArray[letter].width / _bitmapWidth), _fontArray[letter].y + (float)(_fontArray[letter].height / _bitmapHeight));
                    index++;

                    //increment the start position for the next letter by the width of this one and one pixel
                    positionX = positionX + _fontArray[letter].width + 1.0f;
                }
            }

            return vertices;
        }

        /// <summary>
        /// Get the length of the text in pixels
        /// </summary>
        /// <param name="text">text to get the length of</param>
        /// <returns>the length as an integer</returns>
        public int GetTextLength(string text)
        {
            int result = 0;
            int stringLength = text.Length;

            for (int i = 0; i < stringLength; i++)
            {
                int letter = text.ToCharArray()[i] - 32;

                //if space its 3px otherwise its the letter length +1
                if (letter == 0)
                {
                    result += 3;
                }
                else
                {
                    result += _fontArray[letter].width + 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the font size
        /// </summary>
        /// <returns>the font size in pixels</returns>
        public int GetFontSize()
        {
            return _fontSize;
        }

        public void Dispose()
        {
            _fontArray = null;
            _texture.Dispose();
        }
    }
}
