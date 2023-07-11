using Microsoft.EntityFrameworkCore;
using Npgsql;
using MusicHall.Core.Domain.Common;
using MusicHall.Core.Domain.Messages;
using MusicHall.Core.Domain.Users;
using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHall.Core.Domain.Bands;
using MusicHall.Core.Domain.Publications;

namespace MusicHall.Data
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //COMMON
        //public DbSet<Civility> Civilities { get; set; }

        //MESSAGES
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        public DbSet<QueuedEmail> QueuedEmails { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<Band> Bands { get; set; }

        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationFile> PublicationFiles { get; set; }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The DbDataReader object that lets us read the return data.</returns>
        public DataSet ExecuteSqlCommandWithReturn(string sql, params NpgsqlParameter[] parameters)
        {
            var result = Database.ExecuteSqlRaw(sql, parameters);

            NpgsqlConnection connection = (NpgsqlConnection)Database.GetDbConnection();

            connection.Open();

            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);

            var dSet = new DataSet();
            var da = new NpgsqlDataAdapter(command);

            da.Fill(dSet);

            connection.Close();

            return dSet;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            var result = Database.ExecuteSqlRaw(sql, parameters);

            return result;
        }
    }
}
