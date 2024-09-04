using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Serializable]
    public class SkillButton
    {
        public Button button;
        public int skillIndex;
        public SkillInfo skillInfo;
    }
    
    [Serializable]
    public class UnitProductButton
    {
        public Button button;
        public UnitType unitType;
    }
    
    [Serializable]
    public class DisplayUI
    {
        public GameObject soloUnitDP;
        public Image unitImage;
        public TextMeshProUGUI unitName;
        public TextMeshProUGUI unitHp;
        public TextMeshProUGUI unitMp;

        public GameObject multUnitDP;
        public List<Image> unitImages;

        public GameObject buildingDP;
        public Image buildingImage;
        public TextMeshProUGUI buildingName;
        public TextMeshProUGUI buildingHp;

        public GameObject buildingUnitProductDP;
        public List<Image> buildingUnitProductImages;
        public Slider buildingUnitProductSilder;
    }
    
    public List<SkillButton> skillButtons;
    public List<UnitProductButton> unitProductButtons;
    public GameObject unitProductUI;
    public GameObject unitControlUI;
    public GameObject BuildingUI;
    
    public DisplayUI displayUI;
    
    public Building selectedBuilding;
    
    // 이벤트 정의
    public event Action<int> OnSkillButtonClicked;
    
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

        DisableAllUI();
        HideBuildingUI();
    }

    private void Update()
    {
        UpdateDisplayUI();
    }

    private void SkillButtonClicked(int skillIndex)
    {
        OnSkillButtonClicked?.Invoke(skillIndex);
    }
    
    private void UnitProductButtonClicked(UnitType unitType)
    {
        selectedBuilding?.EnqueueUnit(unitType);
    }

    public void UpdateButtons(List<GameObject> selectedUnits)
    {
        if (selectedUnits.Count == 0)
        {
            DisableAllUI();
            return;
        }
        
        EnableUnitUI();
        SetAllButtonsActive(false);
        
        if (AreAllUnitsOfSameType(selectedUnits))
        {
            ActivateSpecificButtons(selectedUnits[0].GetComponent<Character>().UnitType);
        }
    }
    
    public void DisableAllUI()
    {
        unitControlUI.SetActive(false);
        BuildingUI.SetActive(false);
        SetAllButtonsActive(false);
        
        displayUI.soloUnitDP.SetActive(false);
        displayUI.multUnitDP.SetActive(false);
        displayUI.buildingDP.SetActive(false);
    }

    private void EnableUnitUI()
    {
        unitControlUI.SetActive(true);
    }

    private void SetAllButtonsActive(bool active)
    {
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
    
    public void ShowBuildingUI(Character character)
    {
        if (character.UnitType == 0)
        {
            BuildingUI.SetActive(true);
        }
        else
        {
            BuildingUI.SetActive(false);
        }
    }
    
    public void ShowUnitProductUI(Building building)
    {
        selectedBuilding = building;
        unitProductUI.SetActive(true);
        
        foreach (var button in unitProductButtons)
        {
            button.button.gameObject.SetActive(false);
        }

        if (building is Barracks)
        {
            ActivateUnitButton(UnitType.Archers);
            ActivateUnitButton(UnitType.SpearMan);
        }
        else if (building is MagicTowers)
        {
            ActivateUnitButton(UnitType.Mage);
        }
        else if (building is Castle)
        {
            ActivateUnitButton(UnitType.Workers);
        }
    }
    
    public void HideBuildingUI()
    {
        selectedBuilding = null;
        unitProductUI.SetActive(false);
    }

    private void ActivateUnitButton(UnitType unitType)
    {
        var button = unitProductButtons.Find(b => b.unitType == unitType);
        if (button != null)
        {
            button.button.gameObject.SetActive(true);
        }
    }

    private void UpdateDisplayUI()
    {
        List<GameObject> unitSelected = SelectionManager.Instance?.unitSelected ?? new List<GameObject>();
        GameObject buildingSelected = SelectionManager.Instance?.buildingSelected;

        if (buildingSelected != null)
        {
            Building building = buildingSelected.GetComponent<Building>();
            if (building != null)
            {
                if (building.productionQueue.Count > 0)
                {
                    UpdateBuildingUnitProductDisplay(building);
                }
                else
                {
                    UpdateBuildingDisplay(buildingSelected);
                }
                return;
            }
        }

        if (unitSelected.Count == 1)
        {
            UpdateSingleUnitDisplay(unitSelected[0]);
        }
        else if (unitSelected.Count > 1)
        {
            UpdateMultipleUnitDisplay(unitSelected);
        }
        else
        {
            DisableAllUI();
        }
    }

    private void UpdateBuildingUnitProductDisplay(Building building)
    {
        displayUI.soloUnitDP.SetActive(false);
        displayUI.multUnitDP.SetActive(false);
        displayUI.buildingDP.SetActive(false);
        displayUI.buildingUnitProductDP.SetActive(true);

        for (int i = 0; i < displayUI.buildingUnitProductImages.Count; i++)
        {
            if (i < building.productionQueue.Count)
            {
                UnitType unitType = building.productionQueue.ToArray()[i];
                GameObject unitPrefab = UnitFactory.GetUnitPrefab(unitType);
                Unit unitComponent = unitPrefab.GetComponent<Unit>();

                displayUI.buildingUnitProductImages[i].gameObject.SetActive(true);
                displayUI.buildingUnitProductImages[i].sprite = unitComponent.icon;
            }
            else
            {
                displayUI.buildingUnitProductImages[i].gameObject.SetActive(false);
            }
        }

        // 첫 번째 유닛의 생산 진행 상황을 슬라이더에 표시
        if (building.isProducing)
        {
            float remainingTime = building.productionTime - building.currentProductionTime;
            displayUI.buildingUnitProductSilder.value = 1 - (remainingTime / building.productionTime);
        }
        else
        {
            displayUI.buildingUnitProductSilder.value = 0;
        }
    }

    private void UpdateBuildingDisplay(GameObject building)
    {
        if (building == null)
        {
            Debug.LogWarning("UpdateBuildingDisplay called with null building");
            DisableAllUI();
            return;
        }

        displayUI.soloUnitDP.SetActive(false);
        displayUI.multUnitDP.SetActive(false);
        displayUI.buildingDP.SetActive(true);
        displayUI.buildingUnitProductDP.SetActive(false);

        Building buildingComponent = building.GetComponentInParent<Building>();
        UnitHealth buildingHealth = building.GetComponentInParent<UnitHealth>();

        if (buildingComponent == null || buildingHealth == null)
        {
            Debug.LogError($"Building {building.name} is missing Building or UnitHealth component");
            DisableAllUI();
            return;
        }

        displayUI.buildingImage.sprite = buildingComponent.icon;
        displayUI.buildingName.text = buildingComponent.Name;
        displayUI.buildingHp.text = $"HP: {buildingHealth.currrentUnitHP}/{buildingHealth.MaxUnitHP}";
    }

    private void UpdateSingleUnitDisplay(GameObject unit)
    {
        displayUI.soloUnitDP.SetActive(true);
        displayUI.multUnitDP.SetActive(false);
        displayUI.buildingDP.SetActive(false);
        displayUI.buildingUnitProductDP.SetActive(false);

        Unit unitComponent = unit.GetComponent<Unit>();
        UnitHealth unitHealth = unit.GetComponent<UnitHealth>();
        //UnitMana unitMana = unit.GetComponent<UnitMana>();

        displayUI.unitImage.sprite = unitComponent.icon;
        displayUI.unitName.text = unitComponent.Name;
        displayUI.unitHp.text = $"HP: {unitHealth.currrentUnitHP}/{unitHealth.MaxUnitHP}";
        
        // if (unitMana != null)
        // {
        //     displayUI.unitMp.gameObject.SetActive(true);
        //     displayUI.unitMp.text = $"MP: {unitMana.CurrentMp}/{unitMana.MaxMp}";
        // }
        // else
        // {
        //     displayUI.unitMp.gameObject.SetActive(false);
        // }
    }

    private void UpdateMultipleUnitDisplay(List<GameObject> units)
    {
        displayUI.soloUnitDP.SetActive(false);
        displayUI.multUnitDP.SetActive(true);
        displayUI.buildingDP.SetActive(false);
        displayUI.buildingUnitProductDP.SetActive(false);

        for (int i = 0; i < displayUI.unitImages.Count; i++)
        {
            if (i < units.Count)
            {
                displayUI.unitImages[i].gameObject.SetActive(true);
                displayUI.unitImages[i].sprite = units[i].GetComponent<Unit>().icon;
            }
            else
            {
                displayUI.unitImages[i].gameObject.SetActive(false);
            }
        }
    }
}       