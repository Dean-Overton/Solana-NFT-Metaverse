using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Payment : MonoBehaviour
{
    public static Payment current;
    [DllImport("__Internal")] private static extern void PromptForPayment ();
    [DllImport("__Internal")] private static extern void CheckForFolkOwnership ();

    public bool _folkOwnership = false;
    public event Action onProvenFolkOwnership;
    public void FolkOwnership () //Called when react returns they are an owner
    {
        _folkOwnership = true;
        if (onProvenFolkOwnership != null)
            onProvenFolkOwnership();
    }
    private bool hasPaid = false;
    public event Action onPaymentSuccessful;
    
    private void Awake()
    {
        current = this;
    }
    public void PaymentSuccessful () //Called when react returns they have payed
    {
        if (onPaymentSuccessful != null)
            onPaymentSuccessful();
        hasPaid = true;
        Debug.Log ("payment success");
    }
    private void Start() {
#if UNITY_WEBGL
        CheckForFolkOwnership();
#endif
    }
    public void PromptPay() {
        //Trigger website payment
        PromptForPayment();
    }
}
