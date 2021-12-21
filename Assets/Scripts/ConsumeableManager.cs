using TMPro;
using UnityEngine;

public class ConsumeableManager : MonoBehaviour
{
    [SerializeField] private int penningarAmount = 10;

    [SerializeField] private GameObject penningar;

    private void Start()
    {
        penningar.GetComponent<TextMeshProUGUI>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>().penningar.ToString();
    }

    private void OnPenningarAmountChangedCB(int updatedPeningars)
    {
        //Debug.Log("In CB");
        penningarAmount = updatedPeningars;
        penningar.GetComponent<TextMeshProUGUI>().text = penningarAmount.ToString();
    }

    private void OnEnable()
    {
        Stats.OnPenningarAmountChanged += OnPenningarAmountChangedCB;
    }
    private void OnDisable()
    {
        Stats.OnPenningarAmountChanged -= OnPenningarAmountChangedCB;
    }
}
