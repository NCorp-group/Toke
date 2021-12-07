using TMPro;
using UnityEngine;

public class ConsumeableManager : MonoBehaviour
{
    [SerializeField] private int penningar_amount = 0;
    private int old_penningar_amount;

    [SerializeField] private GameObject penningar;
    // Start is called before the first frame update
    void Start()
    {
        old_penningar_amount = penningar_amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (penningar_amount != old_penningar_amount)
        {
            penningar.GetComponent<TextMeshProUGUI>().text = penningar_amount.ToString();
        }
    }
}
