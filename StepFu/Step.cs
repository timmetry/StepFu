using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepFu
{
    public class Arrow
    {
        private int x;
        private int y;
        public int X { get { return x; } }
        public int Y { get { return y; } }

        public Arrow(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public enum FootType
    {
        None = 0,
        Left = -1,
        Right = 1,
    }

    public enum StepType
    {
        None = 0,
        Step,
        Hold,
        Roll,
        Jump,
        //Drill,  // NOTE: make another enum for this maybe?
        //DoubleTap,
        //Crossover,
    }

    public class Step
    {
        private Step prev;
        public Step Prev { get { return prev; } }
        private Arrow arrow;
        public Arrow Arrow { get { return arrow; } }
        private FootType foot;
        public FootType Foot { get { return foot; } }
        private StepType type;
        public StepType Type { get { return type; } }

        public Step(DancePad pad, bool startOfChart)
        {
            if (startOfChart)
            {
                // start differently and create a couple extra steps to be safe
                Step first = new Step(pad, false);
                Step second = new Step(pad, false);
                second.prev = first;
                second.arrow = pad.GetNeutralRightFootArrow();
                second.foot = FootType.Right;
                this.prev = second;
                this.arrow = pad.GetNeutralLeftFootArrow();
                this.foot = FootType.Left;
                this.type = StepType.Step;
            }
            else
            {
                this.prev = null;
                this.arrow = pad.GetNeutralLeftFootArrow();
                this.foot = FootType.Left;
                this.type = StepType.Step;
            }
        }

        public Step(Step prev, Arrow arrow, FootType foot, StepType type)
        {
            this.prev = prev;
            this.arrow = arrow;
            this.foot = foot;
            this.type = type;
        }
    }
}
