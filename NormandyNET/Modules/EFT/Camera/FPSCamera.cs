using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects.Components;
using System.Numerics;

namespace NormandyNET.Modules.EFT.Objects
{
    internal class FPSCamera : GameObject
    {
        private bool debugLog = false;

        internal Transform transform;
        internal Camera cameraComponent;

        internal VisorEffect visorEffect;
        internal ThermalVision thermalVision;
        internal NightVision nightVision;

        public FPSCamera(GameObject gameObject)
        {
            address = gameObject.address;
            componentsList = gameObject.componentsList;
        }

        internal Matrix4x4 GetViewMartix()
        {
            if (cameraComponent != null)
            {
                return cameraComponent.GetViewMatrix(false);
            }

            return default;
        }

        internal float GetViewFOV()
        {
            if (cameraComponent != null)
            {
                return cameraComponent.GetViewFov(false);
            }

            return default;
        }

        internal Camera GetCameraComponent()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return default;
            }

            var result = GetComponentByName<Camera>("Camera");

            if (result == null || Memory.IsValidPointer(result.component) == false)
            {
                return default;
            }

            return result;
        }

        internal Transform GetCameraTransform()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                return default;
            }

            var result = GetComponentByName<Transform>("Transform");

            if (result == null || Memory.IsValidPointer(result.component) == false)
            {
                return default;
            }

            return result;
        }

        internal VisorEffect GetVisorEffectsComponent()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return default;
            }

            var result = GetComponentByName<VisorEffect>("VisorEffect");

            if (result == null || Memory.IsValidPointer(result.component) == false)
            {
                                return default;
            }

                        return result;
        }

        internal ThermalVision GetThermalVisionComponent()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return default;
            }

            var result = GetComponentByName<ThermalVision>("ThermalVision");

            if (result == null || Memory.IsValidPointer(result.component) == false)
            {
                                return default;
            }

                        return result;
        }

        internal NightVision GetNightVisionComponent()
        {
            if (Memory.IsValidPointer(address) == false)
            {
                                return default;
            }

            var result = GetComponentByName<NightVision>("NightVision");

            if (result == null || Memory.IsValidPointer(result.component) == false)
            {
                                return default;
            }

                        return result;
        }

        internal void EnableThermalVision()
        {
            if (thermalVision == null || thermalVision.component == 0)
            {
                thermalVision = GetThermalVisionComponent();
            }

            thermalVision?.EnableThermal();
        }

        internal void DisableThermalVision()
        {
            if (thermalVision == null)
            {
                thermalVision = GetThermalVisionComponent();
            }

            thermalVision?.DisableThermal();
        }

        internal void DisableVisorEffects()
        {
            if (visorEffect == null || visorEffect.component == 0)
            {
                visorEffect = GetVisorEffectsComponent();
            }

            visorEffect?.DisableVisorEffects();
        }

        internal void EnableNightVision()
        {
            if (nightVision == null || nightVision.component == 0)
            {
                nightVision = GetNightVisionComponent();
            }

            nightVision?.Enable();
        }

        internal void DisableNightVision()
        {
            if (nightVision == null)
            {
                nightVision = GetNightVisionComponent();
            }

            nightVision?.Disable();
        }
    }
}