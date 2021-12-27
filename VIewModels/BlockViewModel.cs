using DrawNassiProject.Models;
using DrawNassiProject.Models.Composite;
using System.Collections.Generic;
using System.Drawing;

namespace DrawNassiProject.ViewModels
{
    public class BlockViewModel
    {
        public List<Unit> groups;
        public List<Block> blocks;
        public BlockViewModel()
        {
            blocks = new List<Block>();
            groups = new List<Unit>();
        }
        public DrawNassi CreateBlock(byte type, Color color, Color fontColor, Color contrColor, int x, int y, DrawNassi drawNassi, int width, int height, string text, int maxWidth)
        {
            Unit unit = new Unit();
            if (type == 0)
            {
                OperationBlock operationBlock = new OperationBlock();
                if (blocks.Count > 0)
                {
                    operationBlock = new OperationBlock(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    operationBlock = new OperationBlock(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                operationBlock.group = unit;
                unit.Add(drawNassi, operationBlock);
                blocks.Add(operationBlock);
                operationBlock.Draw(drawNassi);
            }
            else if (type == 1)
            {
                BranchingBlock branchingBlock = new BranchingBlock();
                if (blocks.Count > 0)
                {
                    branchingBlock = new BranchingBlock(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    branchingBlock = new BranchingBlock(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                branchingBlock.group = unit;
                unit.Add(drawNassi, branchingBlock);
                blocks.Add(branchingBlock);
                branchingBlock.Draw(drawNassi);
            }
            else if (type == 2)
            {
                WhileBlock whileBlock = new WhileBlock();
                if (blocks.Count > 0)
                {
                    whileBlock = new WhileBlock(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    whileBlock = new WhileBlock(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                whileBlock.group = unit;
                unit.Add(drawNassi, whileBlock);
                blocks.Add(whileBlock);
                whileBlock.Draw(drawNassi);
            }
            else if (type == 3)
            {
                WhilePreBlock whilepreBlock = new WhilePreBlock();
                if (blocks.Count > 0)
                {
                    whilepreBlock = new WhilePreBlock(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    whilepreBlock = new WhilePreBlock(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                unit.Add(drawNassi, whilepreBlock);
                whilepreBlock.group = unit;
                blocks.Add(whilepreBlock);
                whilepreBlock.Draw(drawNassi);
            }
            else if (type == 4)
            {
                WhilePostBlock whilePostBlock = new WhilePostBlock();
                if (blocks.Count > 0)
                {
                    whilePostBlock = new WhilePostBlock(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    whilePostBlock = new WhilePostBlock(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                whilePostBlock.group = unit;
                unit.Add(drawNassi, whilePostBlock);
                blocks.Add(whilePostBlock);
                whilePostBlock.Draw(drawNassi);
            }
            else if (type == 5)
            {
                Subroutine subroutine = new Subroutine();
                if (blocks.Count > 0)
                {
                    subroutine = new Subroutine(color, fontColor, contrColor, blocks[blocks.Count - 1].blockInternalKey + 1, x, y, width, height, text);
                }
                else
                {
                    subroutine = new Subroutine(color, fontColor, contrColor, 0, x, y, width, height, text);
                }
                subroutine.group = unit;
                unit.Add(drawNassi, subroutine);
                blocks.Add(subroutine);
                subroutine.Draw(drawNassi);
            }
            groups.Add(unit);
            return drawNassi;
        }
        public DrawNassi DeleteBlock(DrawNassi drawNassi, Unit unit)
        {
            foreach (var block in groups)
            {
                if (block == unit)
                {
                    groups.Remove(block);
                    for (int i = block.Blocks.Count - 1; i >= 0; i--)
                    {
                        blocks.Remove(block.Blocks[i]);
                    }
                    break;
                }
            }
            return drawNassi;
        }
        public DrawNassi SetPosition(Unit unit, DrawNassi drawNassi, int x, int y, byte type)
        {
            foreach (var item in unit.Blocks)
            {
                item.blockInternalX = x;
                item.blockInternalY = y;
                y += item.Height;
            }
            unit.Draw(drawNassi);
            return drawNassi;
        }
        public DrawNassi TextChange(Block block, string text, DrawNassi drawNassi)
        {
            block.text = text;
            return drawNassi;
        }
        public DrawNassi StickingBlock(Unit group, Unit unit, DrawNassi drawNassi)
        {
            Unit newGroup = group;
            int index = 0;
            if (group.outBlock != null)
            {
                for (int i = 0; i < group.outBlock.subgroup.Count; i++)
                {
                    if (group.outBlock.subgroup[i] == group)
                    {
                        index = i;
                    }
                }
            }
            for (int i = 0; i < unit.Blocks.Count; i++)
            {
                newGroup.Add(drawNassi, unit.Blocks[i]);
            }
            groups.Remove(unit);
            groups.Remove(group);
            for (int i = groups.Count - 1; i >= 0; i--)
            {
                if (groups[i].Blocks.Count == 0)
                {
                    groups.Remove(groups[i]);
                }
            }
            if (group.outBlock == null)
            {
                groups.Add(newGroup);
            }
            if (group.outBlock != null)
            {
                group.outBlock.subgroup[index] = newGroup;
            }
            group.Draw(drawNassi);
            return drawNassi;
        }
        public DrawNassi InBlocks(Block block, Unit unit, DrawNassi drawNassi, int index)
        {
            groups.Remove(unit);
            block.subgroup[index] = unit;
            unit.outBlock = block;
            return drawNassi;
        }
    }
}
