using UnityEngine;

public class Linterna : MonoBehaviour
{

    public Light LuzLinterna;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("f"))
        {
            if (LuzLinterna.enabled == true)
            {
                LuzLinterna.enabled = false;
            }
            else
            {
                LuzLinterna.enabled = true;
            }
        }
    }
}
