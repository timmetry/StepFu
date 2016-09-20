using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepFu
{
    public class Movement
    {
        private bool finished = false;
        private DancePad pad;
        private Options options;
        private Step lastStep;
        private StepType stepType;
        private FootType footType;
        private LinkedList<Arrow> possibleArrows;

        public bool IsFinished() { return finished; }

        public Movement(DancePad pad, Options options, Step lastStep)
        {
            this.pad = pad;
            this.options = options;
            this.lastStep = lastStep;
            this.stepType = StepType.Step;
            this.footType = (FootType)((int)lastStep.Foot * -1);
            this.possibleArrows = pad.GetActiveArrowLinkedList();
        }

        public void SetStepType(StepType stepType)
        {
            this.stepType = stepType;

            // NOTE: other code may be needed here
        }

        public void Move()
        {
            if (finished) throw new Exception("ERROR: Movement already finished!");

            FilterArrowLinkedList();

            WeighArrowLinkedList();

            StepToArrow();

            //finished = true;
        }

        private void FilterArrowLinkedList()
        {
            LinkedListNode<Arrow> node = possibleArrows.First;
            while (node != null)
            {
                Arrow arrow = node.Value;

                // remove arrows on the same panel as last step
                if (arrow == lastStep.Arrow)
                { node = node.Next; possibleArrows.Remove(node); continue; }

                // remove arrows
            }
        }

        private void WeighArrowLinkedList()
        {
        }

        private void StepToArrow()
        {
        }
    }
}
