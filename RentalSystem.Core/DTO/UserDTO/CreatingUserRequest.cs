public record CreatingUserRequest(
    string Name,
    string Surname,
    string PasswordHash,
    string Email
);