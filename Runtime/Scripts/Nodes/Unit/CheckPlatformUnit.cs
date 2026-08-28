using Virtuademy.SDK.Core.ApplicationManagement;
using Virtuademy.SDK.Core.SystemFramework;
using Unity.VisualScripting;

namespace Virtuademy.CreatorKit.Worlds.VisualScripting
{
  [UnitTitle("Reflectis Platform: Switch")]
  [UnitSurtitle("Platform")]
  [UnitShortTitle("Switch")]
  [UnitCategory("Reflectis\\Flow")]
  public class CheckPlatformUnit : Unit
  {
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput InputTrigger { get; private set; }
    [DoNotSerialize]
    [PortLabel("VR")]
    public ControlOutput OutputTriggerVR { get; private set; }
    [DoNotSerialize]
    [PortLabel("WebGL")]
    public ControlOutput OutputTriggerWebGL { get; private set; }
    [DoNotSerialize]
    [PortLabel("Mobile")]
    public ControlOutput OutputTriggerMobile { get; private set; }


    protected override void Definition()
    {
      InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
      {
        switch (SM.GetSystem<IPlatformSystem>().RuntimePlatform)
        {
          case ESupportedPlatform.VR:
            return OutputTriggerVR;
          case ESupportedPlatform.WebGL:
            return OutputTriggerWebGL;
          case ESupportedPlatform.Mobile:
            return OutputTriggerMobile;
        }
        // Fallback when the platform system has not resolved a platform: mirror
        // PlatformSystem.Init, which reads the build profile's REFLECTIS_* scripting
        // defines. The previous fallback keyed off UNITY_ANDROID and returned the VR
        // branch, but that define is also the mobile player's, so a mobile build
        // reaching this point would take VR decisions.
#if REFLECTIS_VR
        return OutputTriggerVR;
#elif REFLECTIS_MOBILE
        return OutputTriggerMobile;
#else
        // ESupportedPlatform has no Desktop entry: the browser build is WebGL.
        return OutputTriggerWebGL;
#endif
      });

      OutputTriggerVR = ControlOutput(nameof(OutputTriggerVR));
      OutputTriggerWebGL = ControlOutput(nameof(OutputTriggerWebGL));
      OutputTriggerMobile = ControlOutput(nameof(OutputTriggerMobile));

      Succession(InputTrigger, OutputTriggerVR);
      Succession(InputTrigger, OutputTriggerWebGL);
      Succession(InputTrigger, OutputTriggerMobile);
    }
  }

  //Better way to implement this, we should deprecate the old node and find a way to fix existing graphs before updateing
  //[UnitTitle("Reflectis Platform: Switch")]
  //[UnitSurtitle("Platform")]
  //[UnitShortTitle("Switch")]
  //[UnitCategory("Reflectis\\Flow")]
  //public class CheckPlatformUnit : Unit
  //{
  //    [DoNotSerialize]
  //    [PortLabelHidden]
  //    public ControlInput InputTrigger { get; private set; }

  //    [DoNotSerialize]
  //    public List<ControlOutput> Outputs { get; private set; }

  //    private Dictionary<ESupportedPlatform, ControlOutput> supportedPlatformsOutputs = new Dictionary<ESupportedPlatform, ControlOutput>();

  //    protected override void Definition()
  //    {

  //        InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
  //        {
  //            return supportedPlatformsOutputs[SM.GetSystem<IPlatformSystem>().RuntimePlatform];
  //        });

  //        Outputs = new List<ControlOutput>();

  //        foreach (ESupportedPlatform supportedPlatform in Enum.GetValues(typeof(ESupportedPlatform)))
  //        {
  //            ControlOutput output = ControlOutput(supportedPlatform.ToString());
  //            supportedPlatformsOutputs[supportedPlatform] = output;
  //            Succession(InputTrigger, output);
  //            Outputs.Add(output);
  //        }
  //    }
  //}
}
