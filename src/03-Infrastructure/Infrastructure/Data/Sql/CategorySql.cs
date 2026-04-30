namespace Infrastructure.Data.Sql;

public class CategorySql
{
    public const string GetCategoryById = """
                                          select id as Id,
                                            user_id as UserId,
                                            name as Name,
                                            type as Type,
                                            is_active as IsActive,
                                            created_at as CreatedAt
                                          from catalog.categories
                                          where id = @Id
                                          """;
    
    public const string CreateCategory = """
                                          insert into catalog.categories (user_id, name, type)
                                          values (@UserId, @Name, @Type)
                                          """;
    
    public const string UpdateCategory = """
                                          update catalog.categories
                                          set name = @Name,
                                              type = @Type,
                                              is_active = @IsActive
                                          where id = @Id
                                          """;
    
    public const string DeleteCategory = """
                                          delete from catalog.categories
                                          where id = @Id
                                          """;
}