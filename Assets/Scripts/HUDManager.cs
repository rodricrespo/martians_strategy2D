using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI playerResourcesText;
    public TextMeshProUGUI AIResourcesText;
    public GameManager gm;
    public Button attackButton; 
    public bool enableButton;

    private Unit selectedUnit;

    void Start()
    {
        attackButton = GameObject.Find("AttackButton")?.GetComponent<Button>();

        if (attackButton != null)
        {
            // Agrega un listener al botón para manejar el clic
            attackButton.onClick.AddListener(HandleAttackButtonClick);
        }
    }

    void Update()
    {
        playerResourcesText.text = gm.playerResources.ToString();
        AIResourcesText.text = gm.AIresources.ToString();

        // Encuentra todas las instancias de la clase Unit
        Unit[] units = FindObjectsOfType<Unit>();

        bool enableButton = false;

        foreach (Unit unit in units)
        {
            if (unit.tag == "PlayerUnit1" && unit.canAttack && unit.isSelected)
            {
                enableButton = true;
                selectedUnit = unit;
                break; // Si al menos una unidad cumple con la condición, activa el botón
            }
        }

        if (attackButton != null)
        {
            attackButton.interactable = enableButton;
        }
    }

    void HandleAttackButtonClick()
    {
        Debug.Log("Botón de ataque clicado.");
        // Llama a la función de ataque en la unidad seleccionada
        //selectedUnit.AttackEnemy();
        selectedUnit.canAttack = false;
        selectedUnit = null;
        attackButton.interactable = false; // Desactiva el botón después de hacer clic
        gm.ResetTiles();
    }
}
