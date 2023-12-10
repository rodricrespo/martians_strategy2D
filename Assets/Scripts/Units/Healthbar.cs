using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private GameManager gm;
    private GameObject gameLogicObject;

    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        slider = GetComponentInChildren<Slider>();

        // Oculta el Healthbar al inicio si la unidad no es del equipo actual
        UpdateHealthbarVisibility();
    }

    // Método para actualizar la visibilidad de la barra de salud basada en el turno actual
    private void UpdateHealthbarVisibility()
    {
        if (gm.currentTurn==1)
        {
            bool isVisible = false;
            if (this.tag == "EnemyUnit1" || this.tag == "EnemyUnit2") isVisible = true;
            gameObject.SetActive(isVisible);
        }
        else if ((gm.currentTurn==2)){
            bool isVisible = false;
            if (this.tag == "PlayerUnit1" || this.tag == "PlayerUnit2") isVisible = true;
            gameObject.SetActive(isVisible);
        }
    }

    // Método para actualizar la barra de salud basada en la unidad asociada
    public void UpdateHealthbar(int health, int maxHealth)
    {
        slider.value = (float)health / maxHealth;

        // Actualiza la visibilidad cada vez que se actualiza la barra de salud
        UpdateHealthbarVisibility();
    }
}
