using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.vehicle
{
    [RequireComponent(typeof(VehicleController))]
    public class VehicleUIDisplay : MonoBehaviour
    {
        private VehicleController _vehicleController;

        void Start()
        {
            _vehicleController = GetComponent<VehicleController>();

            Assert.IsNotNull(_vehicleController);
        }

        void Update()
        {

        }
    }
}