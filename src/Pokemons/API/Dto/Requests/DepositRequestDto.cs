public class DepositPayload {
    public long PlayerId { get; set; }

}

public class DepositRequestDto
{
    public DepositPayload Payload {get; set; } = null!;
    public decimal Amount { get; set; }
    public string Type {get; set; } = null!;

}