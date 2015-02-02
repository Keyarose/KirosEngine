using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using KirosEngine.Shader;

using Device = SlimDX.Direct3D11.Device;

namespace KirosEngine.ScreenText
{
    class TextBox
    {
        protected Device _device;

        protected Vector4 _textColor;

        protected Font _font;

        protected int _screenHeight, _screenWidth;
        protected int _maxLines;
        protected int _height, _width;

        protected Vector2 _position;

        protected Text[] _lines;
        protected string _text = string.Empty;

        //TODO: update text positions
        /// <summary>
        /// Public accessor for the text box's position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                this.UpdateTextPositions();
            }
        }

        /// <summary>
        /// Public accessor for the text box's text color
        /// </summary>
        public Vector4 TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                _textColor = value;
            }
        }

        /// <summary>
        /// Public accessor for the text box's font
        /// </summary>
        public Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        /// <summary>
        /// Public accessor for the text box's text
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                this.SetText(value);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">The position the textbox is located, (its upper left corner)</param>
        /// <param name="height">The height of the text box</param>
        /// <param name="width">The width of the text box</param>
        /// <param name="font">The font the text box uses</param>
        /// <param name="device">The graphics device</param>
        /// <param name="screenHeight">The height of the screen</param>
        /// <param name="screenWidth">The width of the screen</param>
        public TextBox(Vector2 position, int height, int width, Font font, Device device, int screenHeight, int screenWidth)
        {
            _position = position;
            _height = height;
            _width = width;
            _font = font;

            _device = device;
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;

            _maxLines = height / _font.GetFontSize() + 2;
            _lines = new Text[_maxLines];
        }

        /// <summary>
        /// Set the text of the box and perform updates
        /// </summary>
        /// <param name="text">the text to update to</param>
        public void SetText(string text)
        {
            //set the new text, get the default shader and clear the old text
            _text = text;
            BaseShader textShader = ShaderManager.Instance.GetShaderForKey(ScreenText.Text.DefaultTextShaderKey);
            _lines = new ScreenText.Text[_maxLines];

            int lineCount = 0;

            List<string> words = new List<string>(text.Split(' '));
            string currentLine = string.Empty;
            string lastWord = string.Empty;
            while (true)
            {
                string newString = (currentLine + ' ').TrimStart(' ') + words[0];

                if(currentLine != string.Empty && _font.GetTextLength(newString) > _width)
                {
                    //doesn't fit add the current line to the lines and move on
                    _lines[lineCount] = new ScreenText.Text(_device, _screenHeight, _screenWidth, _font, textShader);
                    _lines[lineCount].Verse = currentLine;
                    lineCount++;
                    currentLine = string.Empty;
                }
                else
                {
                    lastWord = words[0];
                    words.RemoveAt(0);
                    currentLine = newString;
                }

                if(words.Count == 0)
                {
                    //done
                    _lines[lineCount] = new ScreenText.Text(_device, _screenHeight, _screenWidth, _font, textShader);
                    _lines[lineCount].Verse = currentLine;
                    break;
                }
            }

            this.UpdateTextPositions();
        }

        /// <summary>
        /// Update the positions of all lines in use by the box
        /// </summary>
        protected virtual void UpdateTextPositions()
        {
            for(int index = 0; index < _maxLines; index++)
            {
                if(_lines[index] != null)
                {
                    _lines[index].Position = new Vector2(this.Position.X, this.Position.Y + ((_font.GetFontSize() + 2) * index));
                }
            }
        }

        /// <summary>
        /// Draw the text box
        /// </summary>
        /// <param name="context">the device context</param>
        /// <param name="world">the world matrix</param>
        /// <param name="view">the view matrix</param>
        /// <param name="ortho">the orthographic matrix</param>
        public virtual void Draw(DeviceContext context, Matrix world, Matrix view, Matrix ortho)
        {
            if(this.Text != string.Empty)
            {
                for(int index = 0; index < _maxLines; index++)
                {
                    if(_lines[index] != null)
                    {
                        //_lines[index].Position = new Vector2(this.Position.X, this.Position.Y + ((_font.GetFontSize() + 2) * index));
                        _lines[index].Draw(context, world, view, ortho);
                    }                    
                }
            }
        }

        /// <summary>
        /// Dispose of the text box's resources
        /// </summary>
        public virtual void Dispose()
        {
            foreach (Text t in _lines)
            {
                if(t != null)
                {
                    t.Dispose();
                }
            }
        }
    }
}
