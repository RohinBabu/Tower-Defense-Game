﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class BuildNode : MonoBehaviour
    {
        public bool isMagicalNode { get;  set; }
        GameManager gameManager;

        public GameObject OutterObject;

        [HideInInspector]
        public Color startColor;
        public Color activeColor;
      //  [HideInInspector]
        public Renderer rend;

        [HideInInspector]
        public Tower towerScript;
        [HideInInspector]
        public GameObject tower;
        [HideInInspector]
        public TowerData towerBlueprint;
        [HideInInspector]
        public bool isUpgraded = false;
        [HideInInspector]
        public int upgradeIndex = 0;
        [Space]
        public Vector3 offset = new Vector3(-1.5f, 0, -1.5f);
        [HideInInspector]
        public bool isSelected;
        public GameObject mag;

        private void Start()
        {
            gameManager = GameManager.instance;

            rend = GetComponent<Renderer>();
            offset = new Vector3(-1.5f, 0, -1.5f);
            startColor = rend.material.color;
        }

        public void SetColor(Color color)
        {
            rend.material.color = color;
        }

        public Vector3 GetBuildPosition()
        {
            return transform.position + offset;
        }

        public void UpgradeTurret01()
        {
            // prevents upgrading after final upgrade
            if (upgradeIndex >= 5 || towerBlueprint.towerPrefab == null)
                return;

            if (towerBlueprint.upgrade01 == null)
                return;

            if (PlayerStats.Gold < towerBlueprint.upgrade01.cost)
            {
                Debug.Log("Not enough gold to upgrade that!");
                return;
            }

            PlayerStats.Gold -= towerBlueprint.upgrade01.cost;

            //Get rid of the old tower
            Destroy(tower);

            //Build a new one
            //if (MagiicalLocation)
            //{
            //  GameObject _tower = Instantiate(towerBlueprint.upgrade02.towerPrefab, GetBuildPosition(), Quaternion.identity);
            // tower = _tower;
            //towerScript = _tower.GetComponent<Tower>();

            // towerBlueprint = towerBlueprint.upgrade02;
            //Add upgrade effect

            //  upgradeIndex++;

            //   if (upgradeIndex >= 5)
            //      isUpgraded = true;

            // rend.material.color = startColor;
            //} 
            GameObject _tower = Instantiate(towerBlueprint.upgrade01.towerPrefab, GetBuildPosition(), Quaternion.identity);
            tower = _tower;
            towerScript = _tower.GetComponent<Tower>();

            towerBlueprint = towerBlueprint.upgrade01;
            //Add upgrade effect

            upgradeIndex++;

            if (upgradeIndex >= 5)
                isUpgraded = true;

            rend.material.color = startColor;
            //if (Collider.CompareTag == "Magic") //del
            // {
            //MagicTurrets();
            // }
        }
        //delete
        public void MagicTurrets()
        {
            // prevents upgrading after final upgrade
            if (upgradeIndex >= 5 || towerBlueprint.towerPrefab == null)
                return;

            if (towerBlueprint.upgrade02 == null)
                return;
            //Get rid of the old tower
            Destroy(tower);

            //Build a new one
            GameObject _tower = Instantiate(towerBlueprint.upgrade02.towerPrefab, GetBuildPosition(), Quaternion.identity);
            tower = _tower;
            towerScript = _tower.GetComponent<Tower>();

            towerBlueprint = towerBlueprint.upgrade02;
            //Add upgrade effect

            upgradeIndex++;

            if (upgradeIndex >= 5)
                isUpgraded = true;

            rend.material.color = startColor;
        }
        //del
        public void UpgradeTurret02()
        {
            // prevents upgrading after final upgrade
            if (upgradeIndex >= 5 || towerBlueprint.towerPrefab == null)
                return;

            if (towerBlueprint.upgrade02 == null)
                return;

            if (PlayerStats.Gold < towerBlueprint.upgrade02.cost)
            {
                Debug.Log("Not enough gold to upgrade that!");
                return;
            }

            PlayerStats.Gold -= towerBlueprint.upgrade02.cost;

            //Get rid of the old tower
            Destroy(tower);

            //Build a new one
            GameObject _tower = Instantiate(towerBlueprint.upgrade02.towerPrefab, GetBuildPosition(), Quaternion.identity);
            tower = _tower;
            towerScript = _tower.GetComponent<Tower>();

            towerBlueprint = towerBlueprint.upgrade02;
            //Add upgrade effect

            upgradeIndex++;

            if (upgradeIndex >= 5)
                isUpgraded = true;

            rend.material.color = startColor;
        }

        public void SellTower()
        {
            PlayerStats.Gold += towerBlueprint.sellValue;

            //Sell effect here 

            Destroy(tower);
            towerBlueprint = null;
            isUpgraded = false;
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Utils.Utilities.IsPointerOverUIObject())
                return;

            rend.material.color = activeColor;
        }

        private void OnMouseDown()
        {
            //if (isMagicalNode)
            //{
            //    Debug.Log("Ismagical");
            //    isUpgraded = true;
            //    GameManager.instance.ShowMagicalUI(this);
            //}
            //else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Utils.Utilities.IsPointerOverUIObject())
                    return;

                rend.material.color = activeColor;
                isSelected = true;

                gameManager.SelectBuildGround(this);
                
            }
        }

        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Utils.Utilities.IsPointerOverUIObject())
                return;

            if (isSelected == false)
                rend.material.color = startColor;
        }
    }
}
