using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelTMP;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI manaTMP;

    [Header("Image")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Image expBar;

    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;

    [Header("Stat Panel")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI statLevelTMP;
    [SerializeField] private TextMeshProUGUI statHealthTMP;
    [SerializeField] private TextMeshProUGUI statManaTMP;
    [SerializeField] private TextMeshProUGUI statDamageTMP;
    [SerializeField] private TextMeshProUGUI statCChanceTMP;
    [SerializeField] private TextMeshProUGUI statCDamageTMP;
    [SerializeField] private TextMeshProUGUI statExpTMP;
    [SerializeField] private TextMeshProUGUI statTotalExpTMP;
    [SerializeField] private TextMeshProUGUI statRequiredExpTMP;
    [SerializeField] private TextMeshProUGUI attributePointsTMP;
    [SerializeField] private TextMeshProUGUI strenghTMP;
    [SerializeField] private TextMeshProUGUI dexterityTMP;
    [SerializeField] private TextMeshProUGUI intelligenceTMP;

    [Header("Bag Panel")]
    [SerializeField] private GameObject bagPanel;

    [Header("Skill Panel")]
    [SerializeField] private GameObject SkillPanel;

    private int curPanel;
    private bool ifOpenPanel;
    private void Start()
    {
        curPanel = 1;
        ifOpenPanel = false;
    }
    private void Update()
    {
        UpdateUI();
        PanelManageInput();
    }

    private void PanelManageInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelManage();
            if (ifOpenPanel == false)
            {
                menuPanel.SetActive(true);
                ifOpenPanel = true;
            }
            else
            {
                menuPanel.SetActive(false);
                ifOpenPanel = false;
            }
        }
    }

    public void StatPanelButton()
    {
        if (curPanel == 1) return;
        PanelManage();
        curPanel = 1;
        PanelManage();
    }

    public void BagPanelButton()
    {
        if (curPanel == 2) return;
        PanelManage();
        curPanel = 2;
        PanelManage();
    }

    public void SkillPanelButton()
    {
        if (curPanel == 3) return;
        PanelManage();
        curPanel = 3;
        PanelManage();
    }

    private void PanelManage()
    {
        // 管理界面的关闭等信息
        switch(curPanel)
        {
            case 1:
                OpenCloseStatsPanel();
                break;
            case 2:
                OpenCloseBagPanel();
                break;
            case 3:
                OpenCloseSkillPanel();
                break;
        }
    }

    private void OpenCloseStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        if(statsPanel.activeSelf)
        {
            UpdateStatsPanel();
        }
    }

    private void OpenCloseBagPanel()
    {
        bagPanel.SetActive(!bagPanel.activeSelf);
    }

    private void OpenCloseSkillPanel()
    {
        SkillPanel.SetActive(!SkillPanel.activeSelf);
    }

    private void UpdateUI()
    {
        // 更新填充
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, stats.Health / stats.MaxHealth, 10f * Time.deltaTime);
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, stats.Mana / stats.MaxMana, 10f * Time.deltaTime);
        expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, stats.CurrentExp / stats.NextLevelExp, 10f * Time.deltaTime);

        // 更新文字描述
        levelTMP.text = $"等级 {stats.Level}";
        healthTMP.text = $"{stats.Health} / {stats.MaxHealth}";
        manaTMP.text = $"{stats.Mana} / {stats.MaxMana}";
    }

    private void UpdateStatsPanel()
    {
        // 更新各种UI数据
        statLevelTMP.text = stats.Level.ToString();
        statHealthTMP.text = stats.MaxHealth.ToString();
        statManaTMP.text = stats.MaxMana.ToString();
        statDamageTMP.text = stats.TotalDamage.ToString();
        statCChanceTMP.text = stats.CriticalChance.ToString();
        statCDamageTMP.text = stats.CriticalDamage.ToString();
        statExpTMP.text = stats.CurrentExp.ToString();
        statTotalExpTMP.text = stats.TotalExp.ToString();
        statRequiredExpTMP.text = stats.NextLevelExp.ToString();
        attributePointsTMP.text = stats.AttributePoints.ToString();
        strenghTMP.text = stats.Strength.ToString();
        dexterityTMP.text = stats.Dexterity.ToString();
        intelligenceTMP.text = stats.Intelligence.ToString();

    }

    private void UpgradeCallback()
    {
        UpdateStatsPanel();
    }

    private void OnEnable()
    {
        PlayerUpgrade.OnPlayerUpgradeEvent += UpgradeCallback;
    }

    private void OnDisable()
    {
        PlayerUpgrade.OnPlayerUpgradeEvent -= UpgradeCallback;
    }
}
