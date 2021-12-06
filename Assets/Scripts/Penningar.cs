using TMPro;
using UnityEngine;

public class Penningar : MonoBehaviour
{
    public int amount = 0;
    private int old_amount;

    [SerializeField] private TextMeshPro amountText;
    // Start is called before the first frame update
    void Start()
    {
        //amountText = gameObject.GetComponent<TextMeshPro>();
        amountText.text = amount.ToString();
        old_amount = amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (amount != old_amount)
        {
            amountText.text = amount.ToString();
            old_amount = amount;
        }
    }
}
