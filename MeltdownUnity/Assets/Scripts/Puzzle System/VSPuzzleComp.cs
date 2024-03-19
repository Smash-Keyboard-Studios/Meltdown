using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VSPuzzleComp : Unit, IPuzzle
{
	bool CurrentState = false;

	bool IPuzzle.Active
	{
		get
		{
			return CurrentState;
		}
		set
		{
			value = CurrentState;
		}
	}

	[DoNotSerialize]
	public ControlInput inputTrigger;

	[DoNotSerialize]
	public ControlOutput outputTrigger;

	[DoNotSerialize] // No need to serialize ports
	public ValueInput myValueA; // Adding the ValueInput variable for myValueA

	[DoNotSerialize] // No need to serialize ports
	public ValueInput myValueB; // Adding the ValueInput variable for myValueB

	[DoNotSerialize] // No need to serialize ports
	public ValueOutput result; // Adding the ValueOutput variable for result

	private bool resultValue; // Adding the string variable for the processed result value
	protected override void Definition()
	{
		inputTrigger = ControlInput("inputTrigger", (flow) =>
		{
			//Making the resultValue equal to the input value from myValueA concatenating it with myValueB.
			resultValue = flow.GetValue<IPuzzle>(myValueA).Active;
			return outputTrigger;
		});

		outputTrigger = ControlOutput("outputTrigger");

		//Making the myValueA input value port visible, setting the port label name to myValueA and setting its default value to Hello.
		myValueA = ValueInput<PuzzleComp>("Puzzle Comp", null);
		//Making the myValueB input value port visible, setting the port label name to myValueB and setting its default value to an empty string.
		// myValueB = ValueInput<string>("myValueB", string.Empty);
		//Making the result output value port visible, setting the port label name to result and setting its default value to the resultValue variable.
		result = ValueOutput<bool>("result", (flow) => { return resultValue; });
	}
}
