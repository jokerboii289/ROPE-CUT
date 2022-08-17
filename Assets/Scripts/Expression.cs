using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expression : MonoBehaviour
{
    public GameObject afraidExpression;
    public GameObject happyExpression;
    // Start is called before the first frame update
    void Start()
    {
        Character.expressionChange += ChangeExpression;
        afraidExpression.SetActive(false);
        happyExpression.SetActive(true);
    }

    void ChangeExpression()
    {
        afraidExpression.SetActive(true);
        happyExpression.SetActive(false);
    }

    private void OnDisable()
    {
        Character.expressionChange  -= ChangeExpression;
    }
}
