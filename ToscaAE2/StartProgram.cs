using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tricentis.Automation.AutomationInstructions.Configuration;
using Tricentis.Automation.AutomationInstructions.Dynamic.Values;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines;
using Tricentis.Automation.Engines.SpecialExecutionTasks;
using Tricentis.Automation.Engines.SpecialExecutionTasks.Attributes;

namespace ToscaAE2
{

    [SpecialExecutionTaskName("StartProgram")]
    public class StartProgram : SpecialExecutionTask
    {

        public StartProgram(Validator validator) : base(validator) { }

        public override ActionResult Execute(ISpecialExecutionTaskTestAction testAction)
        {
            // get the input values of the teststep, created with XModule >StartProgram<
            IInputValue path = testAction.GetParameterAsInputValue((string)"Path", (bool)false);
            IParameter parameter = testAction.GetParameter((string)"Arguments", (bool)true);
            string processArguments = string.Empty;

            // Arguments has sub parameters – retrieve and concatenate them
            if (parameter != null)
            {
                IEnumerable<IParameter> arguments = parameter.GetChildParameters((string)"Argument");

                foreach (IParameter argument in arguments)
                {
                    IInputValue processArgument = argument.Value as IInputValue;
                    processArguments += processArgument.Value + " ";
                }
            }

            Process.Start(path.Value, processArguments);


            // return, that the action worked
            return new PassedActionResult("Started successfully");
        }
    }
}