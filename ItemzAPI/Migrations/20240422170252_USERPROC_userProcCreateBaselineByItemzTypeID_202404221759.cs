﻿using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ItemzApp.API.Migrations
{
    public partial class USERPROC_userProcCreateBaselineByItemzTypeID_202404221759 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // EXPLANATION: In this Where clause, we are picking up specific .sql files
            // instead of files that EndsWith(".sql") only because we want to make sure
            // that if we have to undo the changes via DOWN method then we are removing 
            // particular .sql file. For now, becuase we are introducing a new .sql file 
            // as a stored procedure then we are fine to have this stored procedure dropped 
            // in the DOWN method below. In the future we have to look at best practice for
            // handling different versions of the .sql (stored procedure) files. This DROP 
            // option within DOWN method does not help in cases where we have updated existing 
            // Stored Procedure.

            var sqlFiles = assembly.GetManifestResourceNames().
                        Where(file => file.EndsWith("userProcCreateBaselineByItemzTypeID_202404221759.sql")
                        );
            foreach (var sqlFile in sqlFiles)
            {
                using (Stream? stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream!))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql(sqlScript);
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // EXPLANATION: In this Where clause, we are picking up specific .sql files
            // instead of files that EndsWith(".sql") only because we want to make sure
            // that if we have to undo the changes via DOWN method then we are removing 
            // particular .sql file. For now, becuase we are introducing a new .sql file 
            // as a stored procedure then we are fine to have this stored procedure dropped 
            // in the DOWN method below. In the future we have to look at best practice for
            // handling different versions of the .sql (stored procedure) files. This DROP 
            // option within DOWN method does not help in cases where we have updated existing 
            // Stored Procedure.

            var sqlFiles = assembly.GetManifestResourceNames().
                        Where(file => file.EndsWith("userProcCreateBaselineByItemzTypeID_202108061231.sql")
                        );
            foreach (var sqlFile in sqlFiles)
            {
                using (Stream? stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream!))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql(sqlScript);
                }
            }
        }
    }
}