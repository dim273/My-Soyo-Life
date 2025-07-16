using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;


    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            AddExp(300);
        }
    }

    public void AddExp(float amount)
    {
        stats.CurrentExp += amount;
        while(stats.CurrentExp > stats.NextLevelExp)
        {
            stats.CurrentExp -= stats.NextLevelExp;
            NextLevel();
        }
    }

    private void NextLevel()
    {
        stats.Level++;
        float currentExpRequired = stats.NextLevelExp;
        float newNextLevelExp = Mathf.Round(currentExpRequired + stats.NextLevelExp * (stats.ExpMultiplier / 100f));
        stats.NextLevelExp = newNextLevelExp;
    }
}
