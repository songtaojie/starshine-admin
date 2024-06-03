using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Impl.AdoJobStore;
{
	public partial class StdAdoDelegate
	{
		/// <inheritdoc />
		public virtual async ValueTask<int> UpdateJobDetail(
			ConnectionAndTransactionHolder conn,
			IJobDetail job,
			CancellationToken cancellationToken = default)
		{
			var jobData = SerializeJobData(job.JobDataMap);

			using var cmd = PrepareCommand(conn, ReplaceTablePrefix(SqlUpdateJobDetail));
			AddCommandParameter(cmd, "schedulerName", schedName);
			AddCommandParameter(cmd, "jobDescription", job.Description);
			AddCommandParameter(cmd, "jobType", job.JobType.FullName);
			AddCommandParameter(cmd, "jobDurable", GetDbBooleanValue(job.Durable));
			AddCommandParameter(cmd, "jobVolatile", GetDbBooleanValue(job.ConcurrentExecutionDisallowed));
			AddCommandParameter(cmd, "jobStateful", GetDbBooleanValue(job.PersistJobDataAfterExecution));
			AddCommandParameter(cmd, "jobRequestsRecovery", GetDbBooleanValue(job.RequestsRecovery));
			AddCommandParameter(cmd, "jobDataMap", jobData, DbProvider.Metadata.DbBinaryType);
			AddCommandParameter(cmd, "jobName", job.Key.Name);
			AddCommandParameter(cmd, "jobGroup", job.Key.Group);

			return await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
