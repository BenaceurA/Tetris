using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    static class Grid
    {
        private static Texture2D texture;
        public struct Cell
        {
            public int x, y;
            public Color Color;
            public bool active;

            public Cell(int x,int y,Color c, bool a)
            {
                this.x = x;
                this.y = y;
                this.Color = c;
                this.active = a;
            }
        }
        static int cellSize = 50;
        static public int rows = Game1.graphics.PreferredBackBufferHeight / cellSize;
        static public int columns = Game1.graphics.PreferredBackBufferWidth / cellSize;
        static public Cell[,] Cells = new Cell[columns,rows];
        
        static public void initGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Cells[j, i].x = j*cellSize;
                    Cells[j, i].y = i*cellSize;
                    Cells[j, i].active = false;
                }
            }
        }

        static public void DrawGrid(SpriteBatch spriteBatch)
        {
            
            foreach (var cell in Cells)
            {
                if (cell.active)
                {
                    Vector2 position = new Vector2(cell.x, cell.y);
                    spriteBatch.Draw(Game1.texture, position, cell.Color);
                }
            }
        }

        static public void ActivateCell(int x, int y,Color Color)
        {
            if (x>=0 && y>=0)
            {
                Cells[x, y].active = true;
                Cells[x, y].Color = Color;
            }
            
        }

        static public void DeactivateCell(int x, int y)
        {
            if (x >= 0 && y >= 0)
            {
                Cells[x, y].active = false;
                Cells[x, y].Color = Color.Black;
            }
            
        }

        static public bool isCellActive(int x , int y)
        {
            if (x>=0 && y>=0)
            {
                return Cells[x, y].active;
            }
            return false;
        }

        static public bool moveDown(ref Cell c)
        {
            if (c.y < rows - 1 && !isCellActive(c.x, c.y + 1))
            {
                DeactivateCell(c.x, c.y);
                c.y++;              
                ActivateCell(c.x, c.y, c.Color);
                return true;
            }
            return false;
        }
        static public bool moveUp(ref Cell c)
        {
            if (c.y > -1)
            {
                DeactivateCell(c.x, c.y);
                c.y--;
                ActivateCell(c.x, c.y, c.Color);
                return true;
            }
            return false;
        }
        static public bool checkCollision(Cell piece)
        {
            if (piece.x > columns-1 || piece.y > rows-1 || isCellActive(piece.x,piece.y)) return true;

            return false;
        }

        static public List<int> checkCompletedRows()
        {
            List<int> completed = new List<int>();

            for (int r = 0; r < rows; r++)
            {
                bool isRowCompleted = true;
                for (int c = 0; c < columns; c++)
                {
                    if (!Cells[c,r].active)
                    {
                        isRowCompleted = false;
                        break;
                    }
                }
                if (isRowCompleted)
                {
                    completed.Add(r);
                }
            }
            return completed;
        }
        
        static public void moveGridDown(int atIndex)
        {
            for (int r = atIndex; r > 0; r--)
            {
                for (int c = 0; c < Grid.columns; c++)
                {
                    Cells[c, r].active = Cells[c, r - 1].active;
                }
            }
        }

        static public void deleteRows(List<int> rows)
        {
            foreach (var r in rows)
            {
                for (int c = 0; c < Grid.columns; c++)
                {
                    Grid.Cells[c, r].active = false;
                }
                moveGridDown(r);
            }
        }
    }
    
}
