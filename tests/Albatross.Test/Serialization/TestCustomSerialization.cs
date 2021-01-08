using Albatross.Reflection;
using Albatross.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Albatross.Test.Serialization {
	public class JobTypeDto : Immutable {
		public string Name { get; set; }
		public string JobClassName { get; set; }
		public string DefinitionClassName { get; set; }
		public string ParameterClassName { get; set; }
		public string[] Tags { get; set; }
	}
	public class Entity : Mutable {
		public int Id { get; set; }
		public string Name { get; set; }
	}
	public class Mutable {
		public string CreatedBy { get; set; }
		public DateTime? CreatedUTC { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedUTC { get; set; }
	}
	public class Immutable {
		public string CreatedBy { get; set; }
		public DateTime? CreatedUTC { get; set; }
	}
	public class JobDefinitionDto : Immutable {
		public string Name { get; set; }
		public int Version { get; set; }
		public string[] Tags { get; set; }

		[Required]
		public JobTypeDto JobType { get; set; }
	}
	public class TriggerDto<ParamType> : Entity {
		public string LocalName { get; set; }
		public bool Enabled { get; set; }
		public ParamType Parameter { get; set; }
		public string Job { get; set; }
		public string[] Schedules { get; set; }
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum NodeType {
		Leaf = 0,
		Sequence = 1,
		Parallel = 2,
		DynamicSequence = 3,
		DynamicParallel = 4,
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum NotificationType {
		SmtpEmail = 0,
		ExchangeEmail = 1,
		Slack = 2,
		Opsgenie = 3,
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum JobInstanceStatus {
		Created = 0,
		Ready = 1,
		Running = 2,
		Success = 3,
		Failed = 4,
		Cancelled = 5,
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PostFailureAction {
		Stop = 0,
		Continue = 1,
		FailParent = 2,
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum JobStepStatus {
		// autorun the job when ready
		Run = 0,
		// skip the job when ready
		Skip = 1,
		// will not run the job when ready
		Pause = 2
	}
	public class JobStepDto {
		public PostFailureAction PostFailure { get; set; }
		public JobStepStatus Status { get; set; }
		public string Job { get; set; }
		public string Trigger { get; set; }
	}
	public class NotificationDto : Entity {
		public string Job { get; set; }
		public string LocalName { get; set; }
		public NotificationType Type { get; set; }
		public bool Enabled { get; set; }
		public JobInstanceStatus JobInstanceStatus { get; set; }

		public string[] Recipients { get; set; }
		public string[] TestRecipients { get; set; }

		public string ReplyToAddress { get; set; }
		public string TestReplyToAddress { get; set; }

		public string TitleTemplate { get; set; }
		public string MsgTemplate { get; set; }
		public string Sender { get; set; }
		public bool Critical { get; set; }
	}

	public class JobDto<DefinitionType, ParamType> : Entity {
		public bool Enabled { get; set; }
		public string Description { get; set; }
		public string Group { get; set; }
		public NodeType NodeType { get; set; }
		public string Retention { get; set; }
		public DefinitionType Definition { get; set; }

		public TriggerDto<ParamType>[] Triggers { get; set; }
		public NotificationDto[] Notifications { get; set; }
		public JobStepDto[] Steps { get; set; }
	}
}
