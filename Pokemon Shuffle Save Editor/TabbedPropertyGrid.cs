﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public class TabbedPropertyGrid : PropertyGrid
    {
        public TabbedPropertyGrid() : base() { }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.ParentForm.ActiveControl.Equals(this))
            {
                // Get selected griditem
                if (this.SelectedGridItem == null) { return base.ProcessCmdKey(ref msg, keyData); }

                // Create a collection all visible child griditems in propertygrid
                GridItem root = this.SelectedGridItem;
                while (root.GridItemType != GridItemType.Root)
                {
                    root = root.Parent;
                }
                List<GridItem> gridItems = new List<GridItem>();
                this.FindItems(root, gridItems);

                // Get position of selected griditem in collection
                int index = gridItems.IndexOf(this.SelectedGridItem);

                if (keyData == Keys.Tab)
                {
                    if (index < gridItems.Count - 1)
                        this.SelectedGridItem = gridItems[++index];
                    else
                        this.ParentForm.SelectNextControl(this, true, true, true, true);
                    return true;
                }
                else if (keyData == (Keys.Tab | Keys.Shift))
                {
                    if (index > 0)
                        this.SelectedGridItem = gridItems[--index];
                    else
                        this.ParentForm.SelectNextControl(this, false, true, true, true);
                    return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FindItems(GridItem item, List<GridItem> gridItems)
        {
            switch (item.GridItemType)
            {
                case GridItemType.Root:
                    foreach (GridItem i in item.GridItems)
                    {
                        FindItems(i, gridItems);
                    }
                    break;
                case GridItemType.Category:
                    foreach (GridItem i in item.GridItems)
                    {
                        FindItems(i, gridItems);
                    }
                    break;
                case GridItemType.Property:
                    gridItems.Add(item);
                    if (item.Expanded)
                    {
                        foreach (GridItem i in item.GridItems)
                        {
                            FindItems(i, gridItems);
                        }
                    }
                    break;
                case GridItemType.ArrayValue:
                    break;
            }
        }
    }

    public static class PropertyGridExtensions
    {
        public static IEnumerable<GridItem> EnumerateAllItems(this PropertyGrid grid)
        {
            if (grid == null)
                yield break;

            // get to root item
            GridItem start = grid.SelectedGridItem;
            while (start.Parent != null)
            {
                start = start.Parent;
            }

            foreach (GridItem item in start.EnumerateAllItems())
            {
                yield return item;
            }
        }

        public static IEnumerable<GridItem> EnumerateAllItems(this GridItem item)
        {
            if (item == null)
                yield break;

            yield return item;
            foreach (GridItem child in item.GridItems)
            {
                foreach (GridItem gc in child.EnumerateAllItems())
                {
                    yield return gc;
                }
            }
        }
    }
}
