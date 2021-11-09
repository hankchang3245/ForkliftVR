using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.vehicle;

public class RevsDisplay : MonoBehaviour
{
    public Sprite[] revsImages;
    public Image sourceImage;


    public VehicleController _vehicleController;


    private void Update()
    {
        if (_vehicleController.AccelerationInt < 0 || _vehicleController.AccelerationInt >= revsImages.Length)
        {
            _vehicleController.AccelerationInt = 0;
        }
        sourceImage.sprite = revsImages[_vehicleController.AccelerationInt];
    }

    public void SpriteChanged()
    {
        if (_vehicleController.AccelerationInt < 0 || _vehicleController.AccelerationInt >= revsImages.Length)
        {
            _vehicleController.AccelerationInt = 0;
        }
        sourceImage.sprite = revsImages[_vehicleController.AccelerationInt];
    }


}
