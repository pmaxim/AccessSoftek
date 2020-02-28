1. SQL tables ...
Answer:
We look at the ConsoleApp project
@"INSERT INTO Orders (Id, Name)
  SELECT 2, Name
  FROM Orders
  WHERE id = 1
  UPDATE OrderItems
  SET OrderId=2
  DELETE FROM Orders
  WHERE Id=1";
  
2. What is wrong in the following code?...

answer:
We look at the WebUi project
HomeController.cs
public Order LoadOrder(int id)
{
   if (!_cache.TryGetValue(id, out Order model))
     {
       model = _repoOrder.Get(id);
       if (model != null)
       {
         _cache.Set(model.Id, model,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
       }
    return model;
 }
