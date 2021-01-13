using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.AutomationInstructions.Configuration;
using Tricentis.Automation.AutomationInstructions.Dynamic.Values;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.SpecialExecutionTasks;
using Tricentis.Automation.Engines.SpecialExecutionTasks.Attributes;
using Tricentis.Automation.Execution.Results;

namespace ToscaAE2
{
    [SpecialExecutionTaskName("SetBuffer")]
    public class SetBuffer : SpecialExecutionTaskEnhanced
    {
        public SetBuffer(Validator validator) : base(validator)
        {
        }

        public override void ExecuteTask(ISpecialExecutionTaskTestAction testAction)
        {
            //Iterate over each TestStepValue
            foreach (IParameter parameter in testAction.Parameters)
            {
                //ActionMode input means set the buffer
                if (parameter.ActionMode == ActionMode.Input)
                {
                    IInputValue inputValue = parameter.GetAsInputValue();
                    Buffers.Instance.SetBuffer(parameter.Name, inputValue.Value, false);
                    testAction.SetResultForParameter(parameter, SpecialExecutionTaskResultState.Ok, string.Format("Buffer {0} set to value {1}.", parameter.Name, inputValue.Value));
                }
                //Otherwise we let TBox handle the verification. Other ActionModes like WaitOn will lead to an exception.
                else
                {
                    //Don't need the return value of HandleActualValue in this case.
                    HandleActualValue(testAction, parameter, Buffers.Instance.GetBuffer(parameter.Name));
                }
            }
            string buffer = Buffers.Instance.GetBuffer("Buff1");
            Console.WriteLine("The value of the buffer read using GetBuffer method is " + buffer);
            Buffers.Instance.SetBuffer("Buff1", "NewValue", false);
        }
    }
}
