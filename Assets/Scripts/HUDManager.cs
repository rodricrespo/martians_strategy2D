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
    public PlayerManager pm;
    public Button attackButton; 
    public Button attackPowerupButton;
    public Button lockPowerupButton;
    public Button resourcesPowerupButton;
    public bool enableButton;

    private Unit selectedUnit;

    void Start()
    {
        attackButton = GameObject.Find("AttackButton")?.GetComponent<Button>();

        if (attackButton != null)
        {
            // Agrega un listener al botón para manejar el clic
            attackButton.onClick.AddListener(HandleAttackButtonClick);
            attackPowerupButton.onClick.AddListener(HandleAttackPowerupButton);
            lockPowerupButton.onClick.AddListener(HandleLockPowerupButton);
            resourcesPowerupButton.onClick.AddListener(HandleResourcesPowerupButton);
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
            if ((unit.tag == "PlayerUnit1" || unit.tag == "PlayerUnit2") && unit.canAttack && unit.isSelected)
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

        bool enablePowerupButtons = gm.currentTurn == 1 && gm.playerResources >= gm.powerupPrice; //Solo serán interactuables si es el turno del jugador y este tiene suficientes recursos para pagarlos

        if (attackPowerupButton != null) attackPowerupButton.interactable = enablePowerupButtons;
        if (lockPowerupButton != null) lockPowerupButton.interactable = enablePowerupButtons;
        if (resourcesPowerupButton != null) resourcesPowerupButton.interactable = enablePowerupButtons;
    }

    void HandleAttackButtonClick()
    {
        //Debug.Log("Botón de ataque clicado.");
        // Llama a la función de ataque en la unidad seleccionada
        if (selectedUnit.playerTarget != null) selectedUnit.AttackEnemyUnit(selectedUnit.playerTarget);
        else {
            if (selectedUnit.playerPowerupTarget != null) {
                Debug.Log("ATACO AL POWERUP");
                selectedUnit.AttackEnemyPowerup(selectedUnit.playerPowerupTarget);
            }
        }
        selectedUnit.canAttack = false;
        selectedUnit = null;
        attackButton.interactable = false; // Desactiva el botón después de hacer clic
        gm.ResetTiles();
    }

    void HandleAttackPowerupButton() {
        pm.BuyAttackPowerup();
    }
    void HandleLockPowerupButton()  {
        pm.BuyLockPowerup();
    }
    void HandleResourcesPowerupButton () {
        pm.BuyResourcesPowerup();
    }   
}
