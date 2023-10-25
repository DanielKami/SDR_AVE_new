using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;


namespace SDRSharp.Average
{
    public unsafe partial class IFAverageWindow
    {
        int LeftMargin = 30;
        int RightMargin = 30;
        int BottomMargin = 35;
        int TopMargin = 40;


        float Height_BottomMargin;
        float Width_LeftMargin_RightMargin;

        Color white = new Color(200, 200, 200);
        Color gray = new Color(30, 30, 30, 5);

        private void Point(int y, int x, Color col)
        {
            int height = panelViewport.Height;

            spriteBatch.Draw(texture, new Vector2(x * 1, height - 50 - y * 1), null,
                              col, 0, new Vector2(0, 0),
                              1, SpriteEffects.None, 0);

        }

        protected void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point1.Y - point2.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            point1.Y = panelViewport.Height - point1.Y;
            spriteBatch.Draw(texture, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        protected void DrawLinen(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point1.Y - point2.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            //point1.Y = panelViewport.Height - point1.Y;
            spriteBatch.Draw(texture, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public void Line(Vector2 vector1, Vector2 vector2, Color color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(vector1.X, vector1.Y, 0);
            vertices[0].Color = color;
            vertices[1].Position = new Vector3(vector2.X, vector2.Y, 0);
            vertices[1].Color = color;

            //Draw the line
            mSimpleEffect.CurrentTechnique.Passes[0].Apply();
            service.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);

        }

        public void FiledRectangle(Vector2 Position, float DimensionX, float DimensionY, Color color1, Color color2)
        {
            VertexPositionColor[] vertices_tringle = new VertexPositionColor[3];
            vertices_tringle[0].Position = new Vector3(Position.X, Position.Y, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices_tringle[1].Color = color1;
            vertices_tringle[2].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices_tringle[2].Color = color2;
            mSimpleEffect.CurrentTechnique.Passes[0].Apply();
            service.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

            vertices_tringle[0].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(Position.X + DimensionX, Position.Y + DimensionY, 0);
            vertices_tringle[1].Color = color2;
            vertices_tringle[2].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices_tringle[2].Color = color2;
            mSimpleEffect.CurrentTechnique.Passes[0].Apply();
            service.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

        }

        public void FiledRectangle(float X1, float Y1, float X2, float Y2, float Y3, Color color1, Color color2)
        {
            VertexPositionColor[] vertices_tringle = new VertexPositionColor[3];
            vertices_tringle[0].Position = new Vector3(X1, Y1, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(X2, Y1, 0);
            vertices_tringle[1].Color = color1;
            vertices_tringle[2].Position = new Vector3(X1, Y3, 0);
            vertices_tringle[2].Color = color2;
            mSimpleEffect.CurrentTechnique.Passes[0].Apply();
            service.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

            vertices_tringle[0].Position = new Vector3(X2, Y1, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(X2, Y2, 0);
            vertices_tringle[1].Color = color2;
            vertices_tringle[2].Position = new Vector3(X1, Y3, 0);
            vertices_tringle[2].Color = color2;
            mSimpleEffect.CurrentTechnique.Passes[0].Apply();
            service.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

        }


        void OnFrameRender()
        {
            Vector2 Point1, Point2;
            Color col = new Color(250, 250, 250);
            Color black = new Color(0, 0, 0, 200);
            this.service.GraphicsDevice.Clear(Color.Black);


            Point1.X = LeftMargin + 1;
            Point1.Y = TopMargin;

            FiledRectangle(Point1, Width_LeftMargin_RightMargin - 4, Height_BottomMargin - TopMargin - 1, new Color(210, 250, 255, 2), new Color(0, 0, 70, 0));

            float ColHeight = 1.0f * (Height_BottomMargin - TopMargin) / 100;
            float ft = 1.0f * (Width_LeftMargin_RightMargin) / BufferFrameSize;

            Point2.X = LeftMargin;
            Point2.Y = TopMargin - (float)Data[0] * ColHeight;

            for (int j = 1; j < BufferFrameSize; j++)
            {
                Point1.X = LeftMargin + ft * j;
                Point1.Y = TopMargin - (float)(Data[j]) * ColHeight;

                FiledRectangle(Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);

                if (Point1.Y < TopMargin) Point1.Y = TopMargin;
                if (Point2.Y < TopMargin) Point2.Y = TopMargin;

                if (Point1.Y > Height_BottomMargin) Point1.Y = Height_BottomMargin;
                if (Point2.Y > Height_BottomMargin) Point2.Y = Height_BottomMargin;
                Line(Point1, Point2, col);

                Point2 = Point1;
            }

            Point1.X = LeftMargin + 1;
            Point1.Y = Height_BottomMargin;
            FiledRectangle(Point1, Width_LeftMargin_RightMargin, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            ScaleX(panelViewport.Width, panelViewport.Height);
            ScaleY(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();

        }

        private float MHz_perPixel;
        private float ScaleX_round;
        private float ScaleX_delta;
        private float PixelsXToStart;
        private float stepX;

        private void ScaleXPrepare()
        {

            Width_LeftMargin_RightMargin = panelViewport.Width - LeftMargin - RightMargin;

            //Find how menny MHz is one pixel
            MHz_perPixel = (float)(Width_LeftMargin_RightMargin / (rate / 1000000));

            //Round frequency to nearest digit after coma
            ScaleX_round = 1.0f * (int)(frequency / 1000000);
            ScaleX_delta = (float)(frequency / 1000000 - ScaleX_round);

            //Number of pixels to shift
            PixelsXToStart = (panelViewport.Width - panelViewport.Width / 2) - ScaleX_delta * MHz_perPixel;
            stepX = (float)Math.Round(100.0 / MHz_perPixel, 1);



        }

        private void ScaleYPrepare()
        {
            Height_BottomMargin = panelViewport.Height - BottomMargin;

        }

        private void ScaleX(float Width, float Height)
        {
            float x, y;
            string drawString;
            float temp = stepX * MHz_perPixel;


            for (int i = -20; i < 20; i++)
            {
                drawString = "" + (ScaleX_round + i * stepX).ToString("0.00");
                x = (float)(PixelsXToStart + temp * i);
                if (x > LeftMargin && x < Width - RightMargin)
                {
                    Line(new Vector2(x, TopMargin), new Vector2(x, Height - BottomMargin), gray);
                    Line(new Vector2(x, Height_BottomMargin), new Vector2(x, Height_BottomMargin + 5), white);
                    x -= 15;
                    y = Height - 25;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y), white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }


            Line(new Vector2(LeftMargin, Height_BottomMargin), new Vector2(Width - RightMargin, Height_BottomMargin), white);

            if (LocationX > LeftMargin && LocationX < Width - RightMargin)
                Line(new Vector2(LocationX - 1, TopMargin), new Vector2(LocationX - 1, Height_BottomMargin), Color.Yellow);

            drawString = "" + ((frequency / 1000000 - rate / 2000000) + (LocationX - LeftMargin) / MHz_perPixel).ToString("0.0000000") + " MHz    " + "Cumulations: " + cumulation + " of: " + cumulation_max+ "    Recording time: " + recordingTime + "s"; ;
            spriteBatch.DrawString(spriteFont, drawString, new Vector2(10 + LeftMargin, 0), Color.White, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);

            if (background_recording)
            {
                drawString = "Background recording!";
                spriteBatch.DrawString(spriteFont, drawString, new Vector2(10 + LeftMargin, 20), Color.Yellow, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
            }

            if (background_corrected & !background_recording)
            {
                drawString = "Corrected background!";
                spriteBatch.DrawString(spriteFont, drawString, new Vector2(10 + LeftMargin, 20), Color.Yellow, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
            }

            if (file_recording)
            {
                drawString = "Multiple file saving!";

                drawString += "File # " + FileNumber;
                spriteBatch.DrawString(spriteFont, drawString, new Vector2(160 + LeftMargin, 20), Color.Red, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);


                if (delay > 0)
                {
                    drawString = "  Pause " + delay;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(320 + LeftMargin, 20), Color.Yellow, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
                }
            }
              }

        private void ScaleY(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;
            int st;

            float stepY = -2.43f;
            int Up = (int)(1000 / (Gain + 1));
            if (Up > 15)
                st = Up / 15;
            else
                st = 1;



            float temp = Height_BottomMargin / Up;
            for (int i = 0; i < Up; i += st)
            {
                drawString = "" + ((i) * stepY - Level / (Gain / 20.5)).ToString("0");
                y = temp * i + TopMargin;
                if (y < Height_BottomMargin)
                {
                    Line(new Vector2(x, y), new Vector2(Width - RightMargin, y), gray);
                    Line(new Vector2(x, y), new Vector2(x - 5, y), white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 2;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x + 17 - lt, y - 6), white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }
            Line(new Vector2(LeftMargin, TopMargin), new Vector2(LeftMargin, Height_BottomMargin), white);
        }

    }
}
