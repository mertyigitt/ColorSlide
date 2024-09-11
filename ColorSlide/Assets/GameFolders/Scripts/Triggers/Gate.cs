using UnityEngine;

public enum GateType
{
    Multiplication,
    Addition
}
public class Gate : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables
    
    [SerializeField] private GateType gateType;
    [SerializeField] private Material gateMaterial;
    [SerializeField] private int gateValue;
    
    #endregion

    #endregion
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if(gateMaterial.color == playerController.CurrentLevelSo.Materials[playerController.MaterialNumber].color)
            {
                if (gateType == GateType.Addition)
                {
                    GateAddition(playerController, gateValue);
                }
                else
                {
                    GateMultiplication(playerController, gateValue);
                }
            }
        }
    }

    private void GateMultiplication(PlayerController player, int number)
    {
        player.MakeStickman(player.Stickmans.Count * number);
    }

    private void GateAddition(PlayerController player, int number)
    {
        player.MakeStickman(number);
    }
}


