using System.Data.Entity;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.DAL
{
	class RestaurantDbContext : DbContext
	{
		public DbSet<AmountExcessAction> AmountExcessActions { get; set; }
		public DbSet<DiscountAction> DiscountActions { get; set; }
		public DbSet<ClientReview> ClientReviews { get; set; }
		public DbSet<ManagerAnswer> ManagerAnswers { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Table> Tables { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Dish> Dishes { get; set; }
		public DbSet<OrderedDish> OrderedDishes { get; set; }
		public DbSet<Employee> Employees { get; set; }
	}
}
