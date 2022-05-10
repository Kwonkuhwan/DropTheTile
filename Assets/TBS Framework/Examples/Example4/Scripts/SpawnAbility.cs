using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.Units.UnitStates;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Example4
{
    public class SpawnAbility : Ability
    {
        public List<GameObject> Prefabs;
        [HideInInspector]
        public GameObject SelectedPrefab;

        public GameObject UnitButton;
        public GameObject UnitPanel;
        public GameObject GoldPanel;

        public event EventHandler UnitSpawned;

        private Unit SpawnedUnit;

        private List<GameObject> UnitButtons = new List<GameObject>();


        public override IEnumerator Act(CellGrid cellGrid)
        {
            if (!UnitReference.Cell.IsTaken && FindObjectOfType<EconomyController>().GetValue(GetComponent<Unit>().PlayerNumber) >= SelectedPrefab.GetComponent<Price>().Value)
            {
                FindObjectOfType<EconomyController>().UpdateValue(GetComponent<Unit>().PlayerNumber, SelectedPrefab.GetComponent<Price>().Value * (-1));

                var unitGO = Instantiate(SelectedPrefab);
                SpawnedUnit = unitGO.GetComponent<Unit>();

                UnitReference.Cell.CurrentUnits.Add(SpawnedUnit);

                SpawnedUnit.Cell = UnitReference.Cell;
                SpawnedUnit.transform.parent = UnitReference.transform.parent;
                SpawnedUnit.transform.localPosition = UnitReference.Cell.transform.localPosition;
                SpawnedUnit.transform.localRotation = Quaternion.Euler(0, 0, 0);
                SpawnedUnit.PlayerNumber = GetComponent<Unit>().PlayerNumber;
                SpawnedUnit.Initialize();
                SpawnedUnit.OnTurnStart();

                var player = FindObjectOfType<CellGrid>().Players.Find(p => p.PlayerNumber == GetComponent<Unit>().PlayerNumber);
                SpawnedUnit.transform.Find("Mask").GetComponent<SpriteRenderer>().color = player.GetComponent<ColorComponent>().Color;

                UnitReference.Cell.IsTaken = SpawnedUnit.Obstructable;

                cellGrid.AddUnit(SpawnedUnit.transform);

                if (UnitSpawned != null)
                {
                    UnitSpawned.Invoke(unitGO, new EventArgs());
                }
            }

            yield return 0;
        }
        public override void Display(CellGrid cellGrid)
        {
            for (int i = 0; i < Prefabs.Count; i++)
            {
                var UnitPrefab = Prefabs[i];

                var unitButton = Instantiate(UnitButton, UnitButton.transform.parent);
                unitButton.GetComponent<Button>().interactable = UnitPrefab.GetComponent<Price>().Value <= FindObjectOfType<EconomyController>().GetValue(GetComponent<Unit>().PlayerNumber);
                unitButton.GetComponentInChildren<Button>().onClick.AddListener(() => ActWrapper(UnitPrefab, cellGrid));

                unitButton.GetComponent<Button>().transform.Find("UnitImage").GetComponent<Image>().sprite = UnitPrefab.GetComponent<SpriteRenderer>().sprite;
                unitButton.GetComponent<Button>().transform.Find("NameText").GetComponent<Text>().text = UnitPrefab.GetComponent<AdvWrsUnit>().UnitName;
                unitButton.GetComponent<Button>().transform.Find("PriceText").GetComponent<Text>().text = UnitPrefab.GetComponent<Price>().Value.ToString();

                unitButton.SetActive(true);
                UnitButtons.Add(unitButton);
            }

            GoldPanel.GetComponentInChildren<Text>().text = string.Format("G. {0}", FindObjectOfType<EconomyController>().GetValue(UnitReference.PlayerNumber));

            UnitPanel.SetActive(true);
            GoldPanel.SetActive(true);
        }

        void ActWrapper(GameObject prefab, CellGrid cellGrid)
        {
            SelectedPrefab = prefab;
            StartCoroutine(Act(cellGrid));
            SpawnedUnit.GetComponent<Unit>().SetState(new UnitStateMarkedAsFinished(SpawnedUnit.GetComponent<Unit>()));

            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                cellGrid.CellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
            }
        }
        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
        }

        public override void OnTurnEnd(CellGrid cellGrid)
        {
            if (SpawnedUnit != null)
            {
                SpawnedUnit.GetComponent<Unit>().SetState(new UnitStateNormal(SpawnedUnit.GetComponent<Unit>()));
            }
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            foreach (var button in UnitButtons)
            {
                Destroy(button);
            }
            UnitPanel.SetActive(false);
            GoldPanel.SetActive(false);
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            return true;
        }
    }
}