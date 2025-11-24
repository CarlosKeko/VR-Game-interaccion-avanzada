using UnityEngine;
using TMPro;

public class bolos : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    bool tirado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tirado = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!tirado && collision.gameObject.name == "Grab Interactable" || collision.gameObject.name == "Capsule" && !tirado) {
            Debug.Log(collision.gameObject.name); // Log the collision
            tirado = true;
            int num = int.Parse(text.text);
            num++;
            text.text = num.ToString();
        

        }

    }
}
