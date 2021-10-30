using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Payment : MonoBehaviour
{
    public static Payment current;
    [DllImport("__Internal")] private static extern void PromptForPayment ();
    [DllImport("__Internal")] private static extern void CheckForFolkOwnership ();

    private bool _folkOwnership = false;
    public bool folkOwnership {
        get { return _folkOwnership; }
    }
    public event Action onProvenFolkOwnership;
    public void FolkOwnership () //Called when react returns they are an owner
    {
        _folkOwnership = true;
        if (onProvenFolkOwnership != null)
            onProvenFolkOwnership();
    }
    private bool hasPaid = false;
    public event Action onPaymentSuccessful;
    public void PaymentSuccessful () //Called when react returns they have payed
    {
        this.hasPaid = true;
        if (onPaymentSuccessful != null)
            onPaymentSuccessful();
    }
    private void Start() {
        CheckForFolkOwnership();
    }
    public void PromptPay() {
        //Trigger website payment
        PromptForPayment();
    }
}
