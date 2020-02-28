1. SQL tables
Let’s say we have a production database for a large web application like Amazon. We have two tables:
table Order
 OrderID int,
 ... -- // Other order columns
table OrderItem
 OrderItemID int,
 OrderID int, -- // Points to Order table.
 ... -- // Other order item columns
Let’s say we have and Order row with OrderID = 1 and several OrderItem rows with OrderID = 1, and we need to change OrderID to
2.
How would you implement this? Provide a SQL fragment that accomplishes this task.
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
  
2. What is wrong in the following code?
You are involved in a new project. This is a web service that implements access to a database for a web application like Amazon.
Your IT lead asks you to analyze the following code fragment (next page). Your goal is to identify as many issues as you can and
compose a list of those issues in the following form:
1. brief issue description - how to resolve
…
N. brief issue description - how to resolve
After identifying the issues, implement a refactoring and present your version of the code fragment.
...
[WebMethod]
public Order LoadOrderInfo( string orderCode )
{
 try
 {
 Debug.Assert( null != orderCode && orderCode != "" );
 Stopwatch stopWatch = new Stopwatch();
 stopWatch.Start();
 lock ( cache )
 {
 if ( cache.ContainsKey( orderCode ) )
 {
 stopWatch.Stop();
 logger.Log( "INFO",
 "Elapsed - {0}", stopWatch.Elapsed );
 return cache[ orderCode ];
 }
 }
 string queryTemplate =
 "SELECT OrderID, CustomerID, TotalMoney" +
 " FROM dbo.Orders where OrderCode='{0}'";
 string query = string.Format( queryTemplate, orderCode );
 SqlConnection connection =
 new SqlConnection( this.connectionString );
 SqlCommand command =
 new SqlCommand( query, connection );
 connection.Open();
 SqlDataReader reader = command.ExecuteReader();
 if ( reader.Read() )
 {
 Order order = new Order(
 ( string ) reader[ 0 ],
 ( string ) reader[ 1 ],
 ( int ) reader[ 2 ] );
 lock ( cache )
 {
 if ( !cache.ContainsKey( orderCode ) )
 cache[ orderCode ] = order;
 }
 stopWatch.Stop();
 logger.Log( "INFO", "Elapsed - {0}", stopWatch.Elapsed );
 return order;
 }
 stopWatch.Stop();
 logger.Log( "INFO", "Elapsed - {0}", stopWatch.Elapsed );
 return null;
 }
 catch ( SqlException ex )
 {
 logger.Log( "ERROR", ex.Message );
 throw new ApplicationException( "Error" );
 }
}
public IDictionary<string, Order> cache;

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
