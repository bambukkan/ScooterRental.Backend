public record CreatingUserRequest(
    string Name,
    string Surname,
    decimal Wallet,
    string Password,
    string Email
);