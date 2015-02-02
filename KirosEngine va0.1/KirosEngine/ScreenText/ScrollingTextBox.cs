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
    class ScrollingTextBox : TextBox
    {
        //number of lines in one page of the text box
        protected int _pageSize;
        protected int _scrollPosition = 0;

        //TODO: position updates based on page size and scroll position, scroll method

        public ScrollingTextBox(Vector2 position, int height, int width, Font font, Device device, int screenHeight, int screenWidth, int pageCount) 
            : base(position, height, width, font, device, screenHeight, screenWidth)
        {
            _pageSize = height / _font.GetFontSize() + 2;
            _maxLines = _pageSize * pageCount;
        }

        protected override void UpdateTextPositions()
        {
            //base.UpdateTextPositions();
            for (int index = 0; index < _maxLines; index++)
            {
                if (_lines[index] != null)
                {
                    _lines[index].Position = new Vector2(this.Position.X, this.Position.Y + ((_font.GetFontSize() + 2) * (index + _scrollPosition)));
                }
            }
        }

        public override void Draw(DeviceContext context, Matrix world, Matrix view, Matrix ortho)
        {
            //base.Draw(context, world, view, ortho);
            if (this.Text != string.Empty)
            {
                for (int index = _scrollPosition; index < (_pageSize + _scrollPosition); index++)
                {
                    if (_lines[index] != null)
                    {
                        //_lines[index].Position = new Vector2(this.Position.X, this.Position.Y + ((_font.GetFontSize() + 2) * index));
                        _lines[index].Draw(context, world, view, ortho);
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
