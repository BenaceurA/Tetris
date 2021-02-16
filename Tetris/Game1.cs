using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
namespace Tetris
{

    public class Game1 : Game
    {        
        public static GraphicsDeviceManager graphics;
        public static Texture2D texture;
        SpriteBatch spriteBatch;
        Grid.Cell piece;
        Grid.Cell[] block;
        Grid.Cell[,] blocks;
        double elapsed;
        int speed;
        kbd kbd;

        public Game1()
        {
            speed = 500;
            kbd = new kbd();
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
           
        }
       
        protected override void Initialize()
        {
            block = new Grid.Cell[4];
            blocks = new Grid.Cell[7,4];

            

            blocks[0, 3] = new Grid.Cell(5, 0, Color.Red, true);     //
            blocks[0, 0] = new Grid.Cell(5,-1, Color.Red, true);     //
            blocks[0, 1] = new Grid.Cell(5,-2, Color.Red, true);     //
            blocks[0, 2] = new Grid.Cell(5,-3, Color.Red, true);     //

            blocks[1, 0] = new Grid.Cell(5,0, Color.Red, true);        //
            blocks[1, 1] = new Grid.Cell(6,0, Color.Red, true);        //
            blocks[1, 2] = new Grid.Cell(5,-1, Color.Red, true);       ////
            blocks[1, 3] = new Grid.Cell(5,-2, Color.Red, true);

            blocks[2, 0] = new Grid.Cell(5,0, Color.Red, true);           //
            blocks[2, 1] = new Grid.Cell(5,-1, Color.Red, true);        ////
            blocks[2, 2] = new Grid.Cell(4,-1, Color.Red, true);          //   
            blocks[2, 3] = new Grid.Cell(5,-2, Color.Red, true);

            blocks[3, 0] = new Grid.Cell(5,0, Color.Red, true);        ////
            blocks[3, 1] = new Grid.Cell(6,0, Color.Red, true);        ////
            blocks[3, 2] = new Grid.Cell(5,-1, Color.Red, true);
            blocks[3, 3] = new Grid.Cell(6,-1, Color.Red, true);

            elapsed = 0;
            piece.x = 5;
            piece.y = 0;
            piece.Color = Color.Red;
            texture = Content.Load<Texture2D>("tile");
            Grid.initGrid();
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
        }
        
        protected override void UnloadContent()
        {
          
        }

        private void copyBlock(ref Grid.Cell[] dst,ref Grid.Cell[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dst[i].x = src[i].x;
                dst[i].y = src[i].y;
                dst[i].Color = src[i].Color;
                dst[i].active = src[i].active;
            }
        }

        private void updateMovement(GameTime gameTime)
        {
            foreach (var cell in block)
            {
                Grid.DeactivateCell(cell.x, cell.y);
            }            
            if (kbd.KeyTriggered(Keys.Right) || kbd.isKeyHeldDown(Keys.Right, gameTime))
            {
                Grid.Cell[] oldblock = new Grid.Cell[4];

                copyBlock(ref oldblock,ref block);
                for (int i = 0; i < block.Length; i++)
                {
                    if (block[i].x < Grid.columns - 1)
                    {
                        block[i].x++;
                    }
                    else
                    {
                        copyBlock(ref block,ref oldblock);
                        break;
                    }
                    if (Grid.checkCollision(block[i]))
                    {
                        copyBlock(ref block,ref oldblock);
                        break;
                    }
                }
                
            }
            if (kbd.KeyTriggered(Keys.Left) || kbd.isKeyHeldDown(Keys.Left, gameTime))
            {
                Grid.Cell[] oldblock = new Grid.Cell[4];

                copyBlock(ref oldblock, ref block);
                for (int i = 0; i < block.Length; i++)
                {
                    if (block[i].x > 0)
                    {
                        block[i].x--;
                    }
                    else
                    {
                        copyBlock(ref block, ref oldblock);
                        break;
                    }
                    if (Grid.checkCollision(block[i]))
                    {
                        copyBlock(ref block, ref oldblock);
                        break;
                    }
                }
            }
            kbd.KeyTriggered(Keys.Down);
            if (kbd.isKeyHeldDown(Keys.Down, gameTime))
            {
                speed = 100;
            }
            else
            {
                speed = 500;
            }
            foreach (var cell in block)
            {
                Grid.ActivateCell(cell.x, cell.y, cell.Color);
            }
            
            kbd.updateState();
        }

        private Color generateRandomColor(GameTime gameTime)
        {
            int r, g, b;
            Color color;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] rno = new byte[1];

            rng.GetBytes(rno);
            r = rno[0];
            
            rng.GetBytes(rno);
            g = rno[0];

            rng.GetBytes(rno);
            b = rno[0];

            color = new Color(r, g, b, 1);
            return color;
            
        }

        protected override void Update(GameTime gameTime)
        {
            //calculate elapsed time beetwen frames;
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds; 
            
            //this function is outside the if statement because we want the keyboard mouvements to be quickly reactive and responsive
            updateMovement(gameTime);     
            
            //every 500ms move the piece down by 1
            if (elapsed > speed)
            {         
                for (int i = 0; i < block.Length; i++)
                {
                    if(!Grid.moveDown(ref block[i]))
                    {
                        for (int j = i-1; j >= 0; j--)
                        {
                            Grid.moveUp(ref block[j]);
                        }
                        Grid.deleteRows(Grid.checkCompletedRows());
                        Color c = generateRandomColor(gameTime);
                        Random r = new Random((int)gameTime.TotalGameTime.Seconds);

                        int blockIndex = r.Next(0, 4);
                        
                        for (int b = 0; b < 4; b++)
                        {
                            block[b].x = blocks[blockIndex, b].x;
                            block[b].y = blocks[blockIndex, b].y;
                            block[b].Color = c;
                            block[b].active = blocks[blockIndex, b].active;
                        }
                        break;
                    }
                }

                elapsed = 0;
            }
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Grid.DrawGrid(spriteBatch);
            spriteBatch.End();            

            base.Draw(gameTime);
        }
    }
}
