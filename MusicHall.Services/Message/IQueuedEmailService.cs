﻿using MusicHall.Core.Domain.Messages;
using System;
using System.Collections.Generic;

namespace MusicHall.Services.Message
{
    /// <summary>
    /// Queued email service
    /// </summary>
    public interface IQueuedEmailService
    {
        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        void InsertQueuedEmail(QueuedEmail queuedEmail);

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        void UpdateQueuedEmail(QueuedEmail queuedEmail);

        /// <summary>
        /// Search queued emails
        /// </summary>
        /// <param name="fromEmail">From Email</param>
        /// <param name="toEmail">To Email</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="loadOnlyItemsToBeSent">A value indicating whether to load only emails for ready to be sent</param>
        /// <param name="maxSendTries">Maximum send tries</param>
        /// <param name="loadNewest">A value indicating whether we should sort queued email descending; otherwise, ascending.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Queued emails</returns>
        IList<QueuedEmail> SearchEmails(string fromEmail,
            string toEmail, DateTime? createdFromUtc, DateTime? createdToUtc,
            bool loadNotSentItemsOnly, bool loadOnlyItemsToBeSent, int maxSendTries,
            bool loadNewest, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
