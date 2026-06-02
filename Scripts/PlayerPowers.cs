using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    public GameObject clickPower;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public float damageRadius = 3f;
    public float damageAmount = 25;
    public float minDamage;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                GameObject spawnObj = Instantiate(clickPower, hit.point, Quaternion.identity);

                Collider[] hitColliders = Physics.OverlapSphere(hit.point, damageRadius, enemyLayer);

                foreach(Collider col in hitColliders)
                {
                    if(col.CompareTag("Enemy"))
                    {
                        float distance = Vector3.Distance(hit.point, col.transform.position);

                        float t = distance/damageRadius;
                        float damage = Mathf.Lerp(damageAmount, minDamage, t);
                        col.GetComponent<EnemyHealth>()?.TakeDamage(damage);
                    }
                }

                Destroy(spawnObj, 4f);
            }
        }
    }
}
