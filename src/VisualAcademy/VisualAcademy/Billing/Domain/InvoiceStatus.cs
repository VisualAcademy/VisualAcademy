namespace VisualAcademy.Billing.Domain;

public enum InvoiceStatus
{
    Draft,
    Issued,
    Sent,
    Paid,
    Void,
    PaymentProcessing = 5
}
