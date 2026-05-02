namespace Infrastructure.Data.Sql;

public class UserSql
{
    public const string GetUserById = """
                                      select id as Id,
                                      	   name as Name,
                                      	   email as Email,
                                      	   password_hash as PasswordHash,
                                      	   created_at as CreatedAt,
                                      	   updated_at as UpdatedAt
                                      from auth.users
                                      where id = @Id
                                      """;
    
    public const string GetUserByEmail = """
                                      select id as Id,
                                      	   name as Name,
                                      	   email as Email,
                                      	   password_hash as PasswordHash,
                                      	   created_at as CreatedAt,
                                      	   updated_at as UpdatedAt
                                      from auth.users
                                      where email = @Email
                                      """;

    public const string CreateUser = """
                                     insert into auth.users(
                                     	name,
                                     	email,
                                     	password_hash,
                                     	created_at
                                     )
                                     values(
                                            (@Name),
                                            (@Email),
                                            (@PasswordHash),
                                            (@CreatedAt)
                                     )
                                     """;
    
    public const string UpdateUser = """
                                     update auth.users
                                     set password_hash = @PasswordHash,
                                     updated_at = @UpdatedAt
                                     where id = @Id
                                     """;
    
    public const string DeleteUser = """
                                     delete from auth.users
                                     where id = @Id
                                     """;
}