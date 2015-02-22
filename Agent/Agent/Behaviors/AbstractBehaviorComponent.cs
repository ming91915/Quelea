﻿using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractBehaviorComponent : AbstractActionComponent
  {
    /// <summary>
    /// Initializes a new instance of the AbstractBehaviorComponent class.
    /// </summary>
    protected AbstractBehaviorComponent(string name, string nickname, string description, 
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddBooleanParameter(RS.behaviorAppliedName, RS.behaviorNickName, RS.behaviorAppliedDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
      RegisterOutputParams2(pManager);
    }

    protected abstract void RegisterOutputParams2(GH_OutputParamManager pManager);

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance2(IGH_DataAccess da)
    {
      if (!apply)
      {
        da.SetData(nextOutputIndex++, false);
        return;
      }

      bool behaviorsApplied = Run();
      da.SetData(nextOutputIndex++, behaviorsApplied);
    }

    protected abstract bool Run();
  }
}