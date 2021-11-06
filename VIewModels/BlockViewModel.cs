using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawNassiProject.Models;
using System.Drawing;

namespace DrawNassiProject.ViewModels
{
    class BlockViewModel
    {
        public List<Block> blocks;
        public BlockViewModel()
        {
            blocks = new List<Block>();
        }
        public DrawNassi CreateBlock(byte type, Color color, Color fontColor, int x, int y, DrawNassi drawNassi, int width, int height, string text)
        {
            if (type == 0)
            {
                OperationBlock operationBlock = new OperationBlock();
                if (blocks.Count > 0)
                {
                    operationBlock = new OperationBlock(color, fontColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    operationBlock = new OperationBlock(color, fontColor, 0, x, y, width, height, text);
                }
                blocks.Add(operationBlock);
                operationBlock.Draw(drawNassi);
            }
            else if (type == 1)
            {
                BranchingBlock branchingBlock = new BranchingBlock();
                if (blocks.Count > 0)
                {
                    branchingBlock = new BranchingBlock(color, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height);
                }
                else
                {
                    branchingBlock = new BranchingBlock(color, 0, x, y, width, height);
                }
                blocks.Add(branchingBlock);
                branchingBlock.Draw(drawNassi);
            }
            else if (type == 2)
            {
                WhileBlock whileBlock = new WhileBlock();
                if (blocks.Count > 0)
                {
                    whileBlock = new WhileBlock(color, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height);
                }
                else
                {
                    whileBlock = new WhileBlock(color, 0, x, y, width, height);
                }
                blocks.Add(whileBlock);
                whileBlock.Draw(drawNassi);
            }
            else if (type == 3)
            {
                WhilePreBlock whilepreBlock = new WhilePreBlock();
                if (blocks.Count > 0)
                {
                    whilepreBlock = new WhilePreBlock(color, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height);
                }
                else
                {
                    whilepreBlock = new WhilePreBlock(color, 0, x, y, width, height);
                }
                blocks.Add(whilepreBlock);
                whilepreBlock.Draw(drawNassi);
            }
            else if (type == 4)
            {
                WhilePostBlock whilePostBlock = new WhilePostBlock();
                if (blocks.Count > 0)
                {
                    whilePostBlock = new WhilePostBlock(color, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height);
                }
                else
                {
                    whilePostBlock = new WhilePostBlock(color, 0, x, y, width, height);
                }
                blocks.Add(whilePostBlock);
                whilePostBlock.Draw(drawNassi);
            }
            else if (type == 5)
            {
                Subroutine subroutine = new Subroutine();
                if (blocks.Count > 0)
                {
                    subroutine = new Subroutine(color, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height);
                }
                else
                {
                    subroutine = new Subroutine(color, 0, x, y, width, height);
                }
                blocks.Add(subroutine);
                subroutine.Draw(drawNassi);
            }
            return drawNassi;
        }
        public DrawNassi DeleteBlock(int key, DrawNassi drawNassi)
        {
            foreach (var block in blocks)
            {
                if (block.blockInternalKey == key)
                {
                    blocks.Remove(block);
                    break;
                }
            }
            return drawNassi;
        }
        public DrawNassi SetPosition(object block, DrawNassi drawNassi, int x, int y,  byte type)
        {

            if (type == 0)
            {
                OperationBlock operationBlock = block as OperationBlock;
                operationBlock.blockInternalX = x;
                operationBlock.blockInternalY = y;
                operationBlock.Draw(drawNassi);
            }
            else if (type == 1)
            {
                BranchingBlock branchingBlock = block as BranchingBlock;
                branchingBlock.blockInternalX = x;
                branchingBlock.blockInternalY = y;
                branchingBlock.Draw(drawNassi);
            }
            else if (type == 2)
            {
                WhileBlock whileBlock = block as WhileBlock;
                whileBlock.blockInternalX = x;
                whileBlock.blockInternalY = y;
                whileBlock.Draw(drawNassi);
            }
            else if (type == 3)
            {
                WhilePreBlock whilepreBlock = block as WhilePreBlock;
                whilepreBlock.blockInternalX = x;
                whilepreBlock.blockInternalY = y;
                whilepreBlock.Draw(drawNassi);
            }
            else if (type == 4)
            {
                WhilePostBlock whilePostBlock = block as WhilePostBlock;
                whilePostBlock.blockInternalX = x;
                whilePostBlock.blockInternalY = y;
                whilePostBlock.Draw(drawNassi);
            }
            else if (type == 5)
            {
                Subroutine subroutine = block as Subroutine;
                subroutine.blockInternalX = x;
                subroutine.blockInternalY = y;
                subroutine.Draw(drawNassi);
            }
            return drawNassi;
        }
        public DrawNassi TextChange(Block block, string text,DrawNassi drawNassi)
        {
            block.text = text;
            return drawNassi;
        }
        public bool AddConnections(Block block, DrawNassi drawNassi)
        {
            return false;
        }
        static bool StickingBlock(int first_block_key, int second_block_key, int x, int y)
        {
            return false;
        }
    }
}
