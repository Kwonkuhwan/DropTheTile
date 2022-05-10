using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;

namespace TbsFramework.Units.Abilities
{
    public class AttackAbility : Ability
    {
        public Unit UnitToAttack { get; set; }
        public List<Unit> inAttackRange;

        //공격하는 코루틴 셀그리드를 매개변수로 받고 
        //레퍼런스에 공격가능한이 뜬다면 유닛을 받는UnitToAttack를 구하고 cill  공격가능하도록 이미 unit 과 연결되어있는 unitreference를 이어준다. 
        public override IEnumerator Act(CellGrid cellGrid)
        {
            if (UnitReference.IsUnitAttackable(UnitToAttack, UnitReference.Cell))
            {
                UnitReference.AttackHandler(UnitToAttack);
                yield return new WaitForSeconds(0.5f);
            }
            yield return 0;
            Debug.Log("1_Attack-ACT함수");
        }
        
       
        public override void Display(CellGrid cellGrid)
        {
            var unit = GetComponent<Unit>();
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inAttackRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();
            inAttackRange.ForEach(u => u.MarkAsReachableEnemy());
            //foreach(u in inAttackRange)
            //{ u.MarkAsReachebleEnemy(); }
            Debug.Log("2_Attack-Display함수");

        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                UnitToAttack = unit;
                HumanExecute(cellGrid);
                Debug.Log("3_OnUnitClicked 됬을때?");
            }
            else if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                cellGrid.CellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
                Debug.Log("3_OnUnitClicked else if 조건 됬을때?");
            }
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
            Debug.Log("4_OnCellClicked 함수 ");
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            inAttackRange.ForEach(u =>
            {
                if (u != null)
                {
                    u.UnMark();
                    Debug.Log("is Under Attack!");
                }
                Debug.Log("5_CleanUp 함수 끝");



            });
        }
        public override bool CanPerform(CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inAttackRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();
            Debug.Log("6_CanPerform 움직일 수 있따 함수");
            return UnitReference.ActionPoints > 0 && inAttackRange.Count > 0;
        }
    }
}

