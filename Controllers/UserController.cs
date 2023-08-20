using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrudTutorial.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IConfiguration _config;

		public UserController(IConfiguration config)
		{
			_config = config;
		}


		[HttpGet]
		public async Task<ActionResult<List<User>>> GetAllUsers()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			IEnumerable<User> users = await SellectAllUsers(connection);
			return Ok(users);
		}


		[HttpGet("{userId}")]
		public async Task<ActionResult<User>> GetUser(int userId)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			var user = await connection.QueryFirstAsync<User>("select * from Users where id = @Id",
				new { Id = userId });
			return Ok(user);
		}
		[HttpPost]
		public async Task<ActionResult<List<User>>> CreateUser(User user)
		{
			try
			{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			await connection.ExecuteAsync("insert into Users (fullname, email, password, phonenumber, address) values (@FullName, @Email, @Password, @PhoneNumber, @Address)", user);
			return Ok(await SellectAllUsers(connection));

			}
			catch (Exception)
			{

				throw;
			}
		}
		[HttpPut]
		public async Task<ActionResult<List<User>>> UpdateUser(User user)
		{
			try
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				await connection.ExecuteAsync("update users set fullname = @FullName, email = @Email, password = @Password, phonenumber = @PhoneNumber, address = @Address  where id = @Id", user);
				return Ok(await SellectAllUsers(connection));

			}
			catch (Exception)
			{

				throw;
			}
		}
		[HttpDelete("{userId}")]
		public async Task<ActionResult<List<User>>> DeleteUser(int userId)
		{
			try
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				await connection.ExecuteAsync("delete from users where id = @Id", new {Id = userId});
				return Ok(await SellectAllUsers(connection));

			}
			catch (Exception)
			{

				throw;
			}
		}



		private static async Task<IEnumerable<User>> SellectAllUsers(SqlConnection connection)
		{
			return await connection.QueryAsync<User>("select * from Users");
		}
	}
}