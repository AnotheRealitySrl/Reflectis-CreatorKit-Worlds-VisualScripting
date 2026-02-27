
using Reflectis.SDK.Core.SystemFramework;

using System.Collections.Generic;

using Unity.VisualScripting;
using Reflectis.SDK.Core.CharacterController;
using Reflectis.SDK.Core;
using System;
using System.Reflection;
using System.Linq;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    [UnitTitle("Reflectis Camera: Set camera mode")]
    [UnitSurtitle("SetCameraMode")]
    [UnitShortTitle("Set Camera Mode")]
    [UnitCategory("Reflectis\\Flow")]
    public class SetCameraModeNode : Unit
    {
        [SerializeAs(nameof(Mode))]
        private ECameraModes mode = ECameraModes.DefaultCamera;

        [DoNotSerialize]
        [Inspectable, UnitHeaderInspectable(nameof(mode))]
        public ECameraModes Mode
        {
            get => mode;
            set => mode = value;
        }

        [DoNotSerialize]
        public List<ValueInput> Arguments { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), (f) =>
            {
                Type type = ICharacterControllerSystem.CameraTypes[Mode];
                var typeInstance = type.Instantiate();

                foreach (var argument in Arguments)
                {
                    if (argument.hasValidConnection || argument.hasDefaultValue)
                    {
                        var value = f.GetConvertedValue(argument);
                        if (value != null)
                        {
                            //Set field value
                            type.GetRuntimeFields().FirstOrDefault(x => x.Name.Equals(argument.key))?.SetValue(typeInstance, value);
                        }
                    }
                }

                InputSettings newInput;
                switch (mode)
                {
                    case ECameraModes.DefaultCamera: 
                        SM.GetSystem<ICharacterControllerSystem>().SetDeafultSettingsAsActive(); 
                        break;

                    case ECameraModes.StaticCamera:
                        StaticCameraType staticCameraType = typeInstance as StaticCameraType;
                        newInput = new InputSettings(false, false, false, false, false, false, false, false, false);
                        SM.GetSystem<ICharacterControllerSystem>().DisableAllButCamera(newInput);
                        break;

                    case ECameraModes.RotationCamera:
                        RotationCameraType rotationType = typeInstance as RotationCameraType;
                        newInput = new InputSettings(true, false, false, false, rotationType.leftButtonToRotate, !rotationType.leftButtonToRotate, false, false, rotationType.constrainedRotation);
                        SM.GetSystem<ICharacterControllerSystem>().DisableAllButCamera(newInput);
                        break;

                    case ECameraModes.CinemaCamera:
                        newInput = new InputSettings(false, false, false, false, true, true, false, false, false);
                        SM.GetSystem<ICharacterControllerSystem>().DisableAllButCamera(newInput);
                        break;
                }
                

                return OutputTrigger;
            });


            Arguments = new List<ValueInput>();

            Type type = ICharacterControllerSystem.CameraTypes[Mode];

            if (type != null)
            {
                foreach (var field in type.GetRuntimeFields())
                {

                    ValueInput argument;

                    argument = ValueInput(field.FieldType, field.Name);

                    //if ((!field.FieldType.IsValueType && !field.FieldType.IsPrimitive && !field.FieldType.IsClass)
                    //|| (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //|| field.FieldType.IsEnum)

                    if (field.FieldType.IsNullable() && !field.FieldType.IsClass)
                    {
                        argument.unit.defaultValues[field.Name] = null;
                    }
                    else
                    {
                        argument.SetDefaultValue(field.FieldType.Default());
                    }


                    Arguments.Add(argument);
                    Requirement(argument, InputTrigger);


                }
            }

            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

    }
}
