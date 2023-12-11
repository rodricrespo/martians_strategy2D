using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private GameManager gm;
    private GameObject gameLogicObject;

    public int powerupHealth = 12;
    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    public void CheckPowerupDeath()
    {
        if (powerupHealth <= 0) {
            //Actualizar nodo
            Node deathNode = Pathfinding.grid.NodeFromWorldPoint(transform.position);
            if (deathNode != null)
            {
                deathNode.walkable = true;
                deathNode.hasUnit = false;
                deathNode.hasPowerup = false;
                deathNode.powerup = null;
                
                // Remover la unidad del diccionario Blackboard
                if (gm.behaviourTree.Blackboard2.ContainsKey("PowerupObject"))
                {
                    gm.behaviourTree.Blackboard2.Remove("PowerupObject");
                }

                if (this.tag == "EnemyPrefab1") gm.AIpowerMultiplier -= 1;
                if (this.tag == "EnemyPrefab2") gm.AIresourcesMultiplier -= 1;
                if (this.tag == "EnemyPrefab3"){
                    gm.playerResourcesMultiplier = 1;  
                    gm.playerPowerMultiplier += 1;
                }

                if (this.tag == "PlayerPrefab1") gm.playerPowerMultiplier -= 1;
                if (this.tag == "PlayerPrefab2") gm.playerResourcesMultiplier -= 1;
                if (this.tag == "PlayerPrefab3"){
                    gm.AIresourcesMultiplier = 1;  
                    gm.AIpowerMultiplier += 1;
                }
            }

            StartCoroutine(DestroyAfterDelay(0.1f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
