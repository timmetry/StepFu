using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepFu
{
    public abstract class DancePad
    {
        private int width;
        private int height;
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        protected bool[,] pad;

        protected DancePad(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.pad = new bool[width, height];
            {
                // for starters, set every panel to false
                // this means no arrows will be used for this game type until assigned
                for (int i = 0; i < width; ++i)
                    for (int j = 0; j < height; ++j)
                        this.pad[i, j] = false;
            }
        }

        public bool IsPanelOnPad(int x, int y)
        {
            if (x < 0 || x >= Width)
                // the X coordinate is off the pad
                return false;
            else if (y < 0 || y >= Height)
                // the Y coordinate is off the pad
                return false;
            else
                // both coordinates are on the pad
                return true;
        }
        public bool IsPanelOnPad(Arrow a)
        { return IsPanelOnPad(a.X, a.Y); }

        public bool IsPanelActive(int x, int y)
        {
            if (IsPanelOnPad(x, y))
                // if the coordinates are off the pad, 
                // this panel doesn't even exist
                return false;
            else
                // since this panel exists on the pad,
                // check to see if this panel is used for this game type
                return pad[x, y];
        }
        public bool IsPanelActive(Arrow a)
        { return IsPanelActive(a.X, a.Y); }

        public LinkedList<Arrow> GetActiveArrowLinkedList()
        {
            LinkedList<Arrow> arrowLinkedList = new LinkedList<Arrow>();
            {
                for (int i = 0; i < Width; ++i)
                    for (int j = 0; j < Height; ++j)
                        if (IsPanelActive(i, j))
                            arrowLinkedList.AddLast(new Arrow(i, j));
            }
            return arrowLinkedList;
        }

        public abstract int GetActivePanelNum();

        public abstract Arrow GetNeutralLeftFootArrow();
        public abstract Arrow GetNeutralRightFootArrow();

        public abstract int GetStringPosition(int x, int y);

        public int GetStringPosition(Arrow a)
        { return GetStringPosition(a.X, a.Y); }
    }

    public abstract class Single9PanelPad : DancePad
    {
        // x,y  0   1   2
        //    +---+---+---+
        //  0 |0,0|1,0|2,0|
        //    +---+---+---+
        //  1 |0,1|1,1|2,1|
        //    +---+---+---+
        //  2 |0,2|1,2|2,2|
        //    +---+---+---+

        public Single9PanelPad()
            : base(3, 3)
        { }

        protected void UtilizeAllSidePanels()
        {
            pad[0, 1] = true; // left arrow
            pad[2, 1] = true; // right arrow
            pad[1, 0] = true; // up arrow
            pad[1, 2] = true; // down arrow
        }
        protected void UtilizeAllCornerPanels()
        {
            pad[0, 0] = true; // up-left arrow
            pad[2, 0] = true; // up-right arrow
            pad[0, 2] = true; // down-left arrow
            pad[2, 2] = true; // down-right arrow
        }
        protected void UtilizeCenterPanel()
        {
            pad[1, 1] = true; // center panel
        }

        public override Arrow GetNeutralLeftFootArrow()
        { return new Arrow(0, 1); }
        public override Arrow GetNeutralRightFootArrow()
        { return new Arrow(2, 1); }
    }

    public abstract class Double9PanelPad : DancePad
    {
        // x,y  0   1   2     3   4   5
        //    +---+---+---+ +---+---+---+
        //  0 |0,0|1,0|2,0| |3,0|4,0|5,0|
        //    +---+---+---+ +---+---+---+
        //  1 |0,1|1,1|2,1| |3,1|4,1|5,1|
        //    +---+---+---+ +---+---+---+
        //  2 |0,2|1,2|2,2| |3,2|4,2|5,2|
        //    +---+---+---+ +---+---+---+

        private Single9PanelPad singlePad;

        protected Double9PanelPad(Single9PanelPad singlePad)
            : base(6, 3)
        {
            this.singlePad = singlePad;

            // copy the panels for the first pad
            {
                for (int i = 0; i < 3; ++i)
                    for (int j = 0; j < 3; ++j)
                        this.pad[i, j] = singlePad.IsPanelActive(i, j);
            }
            // copy the panels for the second pad
            {
                for (int i = 3; i < 6; ++i)
                    for (int j = 0; j < 3; ++j)
                        this.pad[i, j] = singlePad.IsPanelActive(i - 3, j);
            }
        }

        public override int GetActivePanelNum()
        { return singlePad.GetActivePanelNum() * 2; }

        public override Arrow GetNeutralLeftFootArrow()
        { return new Arrow(2, 1); }
        public override Arrow GetNeutralRightFootArrow()
        { return new Arrow(3, 1); }

        public override int GetStringPosition(int x, int y)
        {
            if (x < 3) return singlePad.GetStringPosition(x, y);
            else return singlePad.GetStringPosition(x - 3, y) + singlePad.GetActivePanelNum();
        }
    }

    public class DDRSinglePad : Single9PanelPad
    {
        // x,y  0   1   2
        //    +---+---+---+
        //  0 |   |1,0|   |
        //    +---+---+---+
        //  1 |0,1|   |2,1|
        //    +---+---+---+
        //  2 |   |1,2|   |
        //    +---+---+---+
        // 
        //       ← ↑ ↓ →

        public DDRSinglePad()
        {
            // for a Dance Dance Revolution (DDR) style pad, we only need the side panels
            UtilizeAllSidePanels();
        }

        public override int GetActivePanelNum() { return 4; }

        public override int GetStringPosition(int x, int y)
        {
            if (x == 0 && y == 1) // left arrow
                return 0; // first position in the .sm string
            if (x == 1 && y == 0) // up arrow
                return 1; // second position in the .sm string
            if (x == 1 && y == 2) // down arrow
                return 2; // third position in the .sm string
            if (x == 2 && y == 1) // right arrow
                return 3; // fourth position in the .sm string

            throw new Exception("ERROR: Invalid String Position for DDR Pad!");
        }
    }

    public class PIUSinglePad : Single9PanelPad
    {
        // x,y  0   1   2
        //    +---+---+---+
        //  0 |0,0|   |2,0|
        //    +---+---+---+
        //  1 |   |1,1|   |
        //    +---+---+---+
        //  2 |0,2|   |2,2|
        //    +---+---+---+
        // 
        //    ↙ ↖ O ↗ ↘

        public PIUSinglePad()
        {
            // for a Pump It Up (PIU) style pad, we need the corner and center panels
            UtilizeAllCornerPanels();
            UtilizeCenterPanel();
        }

        public override int GetActivePanelNum() { return 5; }

        public override int GetStringPosition(int x, int y)
        {
            if (x == 0 && y == 2) // down-left arrow
                return 0; // first position in the .sm string
            if (x == 0 && y == 0) // up-left arrow
                return 1; // second position in the .sm string
            if (x == 1 && y == 1) // center panel
                return 2; // third position in the .sm string
            if (x == 2 && y == 0) // up-right arrow
                return 3; // fourth position in the .sm string
            if (x == 2 && y == 2) // down-right arrow
                return 4; // fifth position in the .sm string

            throw new Exception("ERROR: Invalid String Position for ITG Pad!");
        }
    }

    public class TMSinglePad : Single9PanelPad
    {
        // x,y  0   1   2
        //    +---+---+---+
        //  0 |0,0|1,0|2,0|
        //    +---+---+---+
        //  1 |0,1|   |2,1|
        //    +---+---+---+
        //  2 |0,2|1,2|2,2|
        //    +---+---+---+
        // 
        // ↙ ← ↖ ↑ ↓ ↗ → ↘

        public TMSinglePad()
        {
            // for a TechnoMotion (TM) style pad, we only need all panels except the center
            UtilizeAllSidePanels();
            UtilizeAllCornerPanels();
        }

        public override int GetActivePanelNum() { return 8; }

        public override int GetStringPosition(int x, int y)
        {
            if (x == 0 && y == 2) // down-left arrow
                return 0; // first position in the .sm string
            if (x == 0 && y == 1) // left arrow
                return 1; // second position in the .sm string
            if (x == 0 && y == 0) // up-left arrow
                return 2; // third position in the .sm string
            if (x == 1 && y == 0) // up arrow
                return 3; // fourth position in the .sm string
            if (x == 1 && y == 2) // down arrow
                return 4; // fifth position in the .sm string
            if (x == 2 && y == 0) // up-right arrow
                return 5; // sixth position in the .sm string
            if (x == 2 && y == 1) // right arrow
                return 6; // seventh position in the .sm string
            if (x == 2 && y == 2) // down-right arrow
                return 7; // eighth position in the .sm string

            throw new Exception("ERROR: Invalid String Position for TM Pad!");
        }
    }

    public class DDRDoublePad : Double9PanelPad
    {
        // x,y  0   1   2     3   4   5
        //    +---+---+---+ +---+---+---+
        //  0 |   |1,0|   | |   |4,0|   |
        //    +---+---+---+ +---+---+---+
        //  1 |0,1|   |2,1| |3,1|   |5,1|
        //    +---+---+---+ +---+---+---+
        //  2 |   |1,2|   | |   |4,2|   |
        //    +---+---+---+ +---+---+---+
        //  
        //          ← ↑ ↓ → ← ↑ ↓ →

        public DDRDoublePad() :
            base(new DDRSinglePad())
        { }

        // NOTE: All overloads are handled in base class
    }

    public class PIUDoublePad : Double9PanelPad
    {
        // x,y  0   1   2     3   4   5
        //    +---+---+---+ +---+---+---+
        //  0 |0,0|   |2,0| |3,0|   |5,0|
        //    +---+---+---+ +---+---+---+
        //  1 |   |1,1|   | |   |4,1|   |
        //    +---+---+---+ +---+---+---+
        //  2 |0,2|   |2,2| |3,2|   |5,2|
        //    +---+---+---+ +---+---+---+
        //  
        //     ↙ ↖ O ↗ ↘ ↙ ↖ O ↗ ↘

        public PIUDoublePad() :
            base(new PIUSinglePad())
        { }

        // NOTE: All overloads are handled in base class
    }
}
