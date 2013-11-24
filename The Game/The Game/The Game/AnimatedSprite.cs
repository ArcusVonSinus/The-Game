using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game // TODO COMMENTS uplne vsude v tomto souboru
{
    public class AnimatedSprite // Pouziva se tato trida nekde?
    {
        public Texture2D Texture;
        public int Rows;
        public int Columns;
        private int currentFrame;
        private int totalFrames;
        public int width
        {
            get { return Texture.Width / Columns; }
        }
        public int height
        {
            get { return Texture.Height / Rows; }
        }
        public void stop()
        {
            currentFrame = 0;
            return;
        }
        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }
        public void Update()
        {
            currentFrame++;
            currentFrame %= totalFrames;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location, int width)
        {
            int width2 = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width2 * column, height * row, width2, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, (width * height) / width2);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

        }
    }
    public class AnimatedSpriteHead // Tady mi chybi nejaky rectangle, jakozto zaladni parametr...mnohem jednoduseji by se pak resili kolize s priserkama
    {
        public Texture2D Texture;
        public Texture2D TextureHead;
        public Texture2D TextureFront;
        public int Rows,RowsHead;
        public int Columns,ColumnsHead;
        private int currentFrame,currentFrameHead;
        private int totalFrames,totalFramesHead;
        float relSize, relPosX, relPosY;
        public int width
        {
            get { return Texture.Width / Columns; }
        }
        public int height
        {
            get { return Texture.Height / Rows; }
        }
        public int widthHead
        {
            get { return TextureHead.Width / Columns; }
        }
        public int heightHead
        {
            get { return TextureHead.Height / Rows; }
        }
        public void stop()
        {
            currentFrame = 0;
            return;
        }
        public AnimatedSpriteHead(Texture2D[] texture, int rows, int columns, int rowsHead, int columnsHead, Vector3 pos)
        {
           AnimatedSpriteHeadCons (texture, rows, columns, rowsHead, columnsHead, pos.X, pos.Y, pos.Z);           
        }
        void AnimatedSpriteHeadCons(Texture2D[] texture,int rows, int columns,int rowsHead, int columnsHead,float relSize, float relPosX,float relPosY)
        {
            Texture = texture[0];

            if (texture.Length >= 2)
            {
                TextureHead = texture[1];
            }
            else
            {
                TextureHead = null;
            }
            if(texture.Length >=3)
            {
                TextureFront = texture[2];
            }
            else
            {
                TextureFront = null;
            }
            Rows = rows;
            Columns = columns;
            RowsHead = rowsHead;
            ColumnsHead = columnsHead;
            currentFrame = 0;
            currentFrameHead = 0;
            totalFrames = Rows * Columns;
            totalFramesHead = RowsHead * columnsHead;
            this.relSize = relSize;
            this.relPosX = relPosX;
            this.relPosY = relPosY;

        }
        public void Update()
        {
            currentFrame++;
            currentFrame %= totalFrames;
        }
        public void UpdateHead()
        {
            currentFrameHead++;
            currentFrameHead %= totalFramesHead;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location, int width)
        {
            int width2 = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int width2Head = TextureHead.Width / ColumnsHead;
            int heightHead = TextureHead.Height / RowsHead;
            int row = (int)((float)currentFrame / (float)Columns);
            int rowHead = (int)((float)currentFrameHead / (float)ColumnsHead);
            int column = currentFrame % Columns;
            int columnHead = currentFrameHead % ColumnsHead;

            Rectangle sourceRectangle = new Rectangle(width2 * column, height * row, width2, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, (width * height) / width2);

            int widthHead=(int) (relSize*width);
            Rectangle sourceRectangleHead = new Rectangle(width2Head * columnHead, heightHead * rowHead, width2Head, heightHead);
            Rectangle destinationRectangleHead = new Rectangle((int)location.X + (int)(relPosX * width), (int)location.Y + (int)(relPosY * width * height/ width2), widthHead, (int)(widthHead * heightHead) / width2Head);
            //Rectangle destinationRectangleHead = new Rectangle((int)location.X + (int)(relPosX * width), (int)location.Y + (int)(relPosY * width * height / width2), widthHead, 50);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            if (TextureHead != null)
            spriteBatch.Draw(TextureHead, destinationRectangleHead, sourceRectangleHead, Color.White);
            if(TextureFront!=null)
                spriteBatch.Draw(TextureFront, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
