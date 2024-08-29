using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [System.Serializable]
    public class SkillButton
    {
        public Button button;
        public int skillIndex;
        public SkillInfo skillInfo;
    }
    
    [System.Serializable]
    public class UnitProductButton
    {
        public Button button;
        public UnitType unitType;
    }

    public Button moveButton;
    public Button stopButton;
    public List<SkillButton> skillButtons;
    public List<UnitProductButton> unitProductButtons;
    public GameObject buildingUI;
    public GameObject unitControlUI;
    
    // 이벤트 정의
    public event Action<int> OnSkillButtonClicked;
    public event Action<UnitType> OnUnitProductButtonClicked;
    
    private Building selectedBuilding;
    
    //유닛 생성
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        for (int i = 0; i < skillButtons.Count; i++)
        {
            int index = i;
            skillButtons[i].button.onClick.AddListener(() => SkillButtonClicked(index));
        }
        
        for (int i = 0; i < unitProductButtons.Count; i++)
        {
            UnitType unitType = unitProductButtons[i].unitType;
            unitProductButtons[i].button.onClick.AddListener(() => UnitProductButtonClicked(unitType));
        }

        DisableUnitUI();
        //SetAllButtonsActive(false);
        HideBuildingUI();
    }

    private void Start()
    {
        
    }

    private void SkillButtonClicked(int skillIndex)
    {
        OnSkillButtonClicked?.Invoke(skillIndex);
    }
    
    private void UnitProductButtonClicked(UnitType unitType)
    {
        selectedBuilding?.EnqueueUnit(unitType);
        OnUnitProductButtonClicked?.Invoke(unitType);
    }

    public void UpdateButtons(List<GameObject> selectedUnits)
    {
        if (selectedUnits.Count == 0)
        {
            DisableUnitUI();
            return;
        }
        
        EnableUnitUI();
        SetAllButtonsActive(false);

        moveButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(true);

        if (AreAllUnitsOfSameType(selectedUnits))
        {
            ActivateSpecificButtons(selectedUnits[0].GetComponent<Character>().UnitType);
        }
    }
    
    public void DisableUnitUI()
    {
        unitControlUI.SetActive(false);
        SetAllButtonsActive(false);
    }

    private void EnableUnitUI()
    {
        unitControlUI.SetActive(true);
    }

    private void SetAllButtonsActive(bool active)
    {
        moveButton.gameObject.SetActive(active);
        stopButton.gameObject.SetActive(active);
        
        foreach (var skillButton in skillButtons)
        {
            skillButton.button.gameObject.SetActive(active);
        }
    }

    private void ActivateSpecificButtons(int unitType)
    {
        foreach (var skillButton in skillButtons)
        {
            if (skillButton.skillIndex == unitType)
            {
                skillButton.button.gameObject.SetActive(true);
            }
        }
    }

    private bool AreAllUnitsOfSameType(List<GameObject> units)
    {
        if (units.Count == 0) return false;

        int firstUnitType = units[0].GetComponent<Character>().UnitType;
        
        return units.TrueForAll(unit => unit.GetComponent<Character>().UnitType == firstUnitType);
    }
    
    public void ShowBuildingUI(Building building)
    {
        selectedBuilding = building;
        buildingUI.SetActive(true);
        
        foreach (var button in unitProductButtons)
        {
            button.button.gameObject.SetActive(false);
        }

        if (building is Barracks)
        {
            ActivateUnitButton(UnitType.Workers);
            ActivateUnitButton(UnitType.Archers);
            ActivateUnitButton(UnitType.SpearMan);
        }
        else if (building is MagicTowers)
        {
            ActivateUnitButton(UnitType.Mage);
        }
    }
    
    public void HideBuildingUI()
    {
        selectedBuilding = null;
        buildingUI.SetActive(false);
    }

    private void ActivateUnitButton(UnitType unitType)
    {
        var button = unitProductButtons.Find(b => b.unitType == unitType);
        if (button != null)
        {
            button.button.gameObject.SetActive(true);
        }
    }
}