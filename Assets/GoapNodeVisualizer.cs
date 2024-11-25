using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoapNodeVisualizer : MonoBehaviour
{
    [SerializeField] TMP_Text _description;
    [SerializeField] Image _image;
    public goapAstarNode Node;
    public void UpdateUI()
    {
        _description.text = Node.SimulatedOutcomeContext.ToString();
       

        if (Node == Node.engine.CurrentNode)
        {
            _image.color = Color.cyan;
        }
        else if (Node.State.CanBeEnteredFromContext(((goapAstarNode)Node.previousNode).SimulatedOutcomeContext)) _image.color = Color.white;
        else _image.color = Color.black;
    }

    public void SetRed()
    {
        _image.color = Color.red;
    }
}
