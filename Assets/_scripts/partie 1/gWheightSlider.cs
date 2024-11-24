using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gWheightSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;

    public void OnValueChanged()
    {
        //Node.gWheight = _slider.value;
    }
}
