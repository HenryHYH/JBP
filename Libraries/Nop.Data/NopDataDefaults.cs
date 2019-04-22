﻿
using System.IO;

namespace Nop.Data
{
    /// <summary>
    /// Represents default values related to Nop data
    /// </summary>
    public static partial class NopDataDefaults
    {
        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server indexes
        /// </summary>
        public static string SqlServerIndexesFilePath => Path.Combine("~/App_Data", "Install", "SqlServer.Indexes.sql");

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server stored procedures
        /// </summary>
        public static string SqlServerStoredProceduresFilePath => Path.Combine("~/App_Data", "Install", "SqlServer.StoredProcedures.sql");
    }
}