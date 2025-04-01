using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris_Main.Game_Models;

namespace Tetris_Main
{
    internal class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get=>currentBlock; 
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
            }
        }

        public Grid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }

        public GameState()
        {
            GameGrid = new Grid(22,10);
            BlockQueue = new BlockQueue();
            currentBlock = BlockQueue.GetAndUpdate();
        }

        private bool BlockFits()
        {
            foreach(Position p in currentBlock.TilePositions())
            {
                if(!GameGrid.IsEmpty(p.Row,p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.Rotatecw();
            
            if(!BlockFits())
            {
                CurrentBlock.Rotateccw();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.Rotateccw();

            if(!BlockFits())
            {
                CurrentBlock.Rotatecw();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if(!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            } 
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in currentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = currentBlock.Id;
            }

            GameGrid.clearFullRows();

            if(IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if(!BlockFits())
            {
                CurrentBlock.Move(-1,0);
                PlaceBlock();
            }
        }
    }
}
