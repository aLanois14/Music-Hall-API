﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicHall.Services.Registration
{
    /// <summary>
    /// Customerregistration result
    /// </summary>
    public class UserRegistrationResult
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public UserRegistrationResult()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Gets a value indicating whether request has been completed successfully
        /// </summary>
        public bool Success
        {
            get { return !this.Errors.Any(); }
        }

        /// <summary>
        /// Add error
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        /// <summary>
        /// Errors
        /// </summary>
        public IList<string> Errors { get; set; }
    }
}
