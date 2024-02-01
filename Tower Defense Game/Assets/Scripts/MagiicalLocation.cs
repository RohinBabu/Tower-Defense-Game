using System.Collections;
using System.Collections.Generic;
using TowerDefence;
using UnityEngine;

public class MagiicalLocation : MonoBehaviour
{
    public BuildNode[] nodeObjeccts;
    // Start is called before the first frame update
    void Start()
    {
        //Select 3 - power;
        int count = 0;

        foreach (var node in nodeObjeccts)
        {
            if (count < 3)
            {
                BuildNode setMagicalNode = nodeObjeccts[Random.Range(0, nodeObjeccts.Length)];
                if (!setMagicalNode.isMagicalNode)
                {
                    setMagicalNode.isMagicalNode = true;
                    setMagicalNode.SetColor(Color.green);
                    count++;
                }
               
                
            }
            else
                return;
           
        }
        

         //   buildnode - ispower = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
