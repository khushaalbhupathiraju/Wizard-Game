using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPoint : MonoBehaviour
{
    public GameObject healingPower;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal();
        }
    }

    private void Heal()
    {
        GameObject childObject = Instantiate(healingPower, transform);

        Destroy(childObject, 5.0f);
    }
}
