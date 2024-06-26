﻿//HintName: Testing.TestAggregate.Core.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.EventSourcing.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#nullable enable

namespace Testing
{
	partial class TestAggregate
	{
		System.Collections.Concurrent.ConcurrentQueue<Purview.EventSourcing.Aggregates.Events.IEvent> _events = new System.Collections.Concurrent.ConcurrentQueue<Purview.EventSourcing.Aggregates.Events.IEvent>();
		readonly System.Collections.Generic.List<System.Type> _eventTypes = new System.Collections.Generic.List<System.Type>() {
			// System events
			typeof(Purview.EventSourcing.Aggregates.Events.DeleteEvent),
			typeof(Purview.EventSourcing.Aggregates.Events.RestoreEvent),
			typeof(Purview.EventSourcing.Aggregates.Events.ForceSaveEvent),
			// Generated events
			typeof(Testing.TheBadgerEvent),
		};

		System.String Purview.EventSourcing.Aggregates.IAggregate.AggregateType { get; } = "test";

		Purview.EventSourcing.Aggregates.AggregateDetails Purview.EventSourcing.Aggregates.IAggregate.Details { get; init; } = new Purview.EventSourcing.Aggregates.AggregateDetails();

		System.Boolean Purview.EventSourcing.Aggregates.IAggregate.HasUnsavedEvents() { return !_events.IsEmpty; }

		System.Collections.Generic.IEnumerable<System.Type> Purview.EventSourcing.Aggregates.IAggregate.GetRegisteredEventTypes() { return _eventTypes.ToArray(); }

		System.Collections.Generic.IEnumerable<Purview.EventSourcing.Aggregates.Events.IEvent> Purview.EventSourcing.Aggregates.IAggregate.GetUnsavedEvents() { return _events.ToArray(); }

		System.Boolean Purview.EventSourcing.Aggregates.IAggregate.CanApplyEvent(Purview.EventSourcing.Aggregates.Events.IEvent aggregateEvent) { return _eventTypes.Contains(aggregateEvent.GetType()); }

		void Purview.EventSourcing.Aggregates.IAggregate.ClearUnsavedEvents(int? upToVersion)
		{
			var unsavedEventCount = _events.Count;
			if (upToVersion.HasValue)
			{
				_events = new System.Collections.Concurrent.ConcurrentQueue<Purview.EventSourcing.Aggregates.Events.IEvent>(
					System.Linq.Enumerable.Where(_events, m => m.Details.AggregateVersion > upToVersion)
				);
			}
			else
			{
				_events.Clear();
			}

			unsavedEventCount -= _events.Count;

			var details = ((Purview.EventSourcing.Aggregates.IAggregate)this).Details;

			details.CurrentVersion -= unsavedEventCount;
		}

		void RecordAndApplyEvent(Purview.EventSourcing.Aggregates.Events.IEvent @event)
		{
			RecordEvent(@event);
			((Purview.EventSourcing.Aggregates.IAggregate)this).ApplyEvent(@event);
		}

		void Purview.EventSourcing.Aggregates.IAggregate.ApplyEvent(Purview.EventSourcing.Aggregates.Events.IEvent @event)
		{
			var details = ((Purview.EventSourcing.Aggregates.IAggregate)this).Details;
			if (details.Locked)
			{
				throw new Purview.EventSourcing.Aggregates.Exceptions.LockedException(details.Id);
			}

			if (@event.Details.AggregateVersion == 1)
			{
				details.Created = @event.Details.When;
			}

			details.Updated = @event.Details.When;
			details.CurrentVersion = @event.Details.AggregateVersion;

			if (@event is Purview.EventSourcing.Aggregates.Events.DeleteEvent)
			{
				ApplySystemEvent((Purview.EventSourcing.Aggregates.Events.DeleteEvent)@event);
			}
			else if (@event is Purview.EventSourcing.Aggregates.Events.RestoreEvent)
			{
				ApplySystemEvent((Purview.EventSourcing.Aggregates.Events.RestoreEvent)@event);
			}
			else if (@event is Purview.EventSourcing.Aggregates.Events.ForceSaveEvent)
			{
				ApplySystemEvent((Purview.EventSourcing.Aggregates.Events.ForceSaveEvent)@event);
			}
			else if (@event is Testing.TheBadgerEvent)
			{
				ApplyTheBadger((Testing.TheBadgerEvent)@event);
			}
		}

		void RecordEvent(Purview.EventSourcing.Aggregates.Events.IEvent @event)
		{
			var details = ((Purview.EventSourcing.Aggregates.IAggregate)this).Details;
			if (details.Locked)
			{
				throw new Purview.EventSourcing.Aggregates.Exceptions.LockedException(details.Id);
			}

			@event.Details.AggregateVersion = details.CurrentVersion + 1;
			@event.Details.When = System.DateTimeOffset.UtcNow;

			_events.Enqueue(@event);
		}

		void ApplySystemEvent(Purview.EventSourcing.Aggregates.Events.DeleteEvent @event)
		{
			var details = ((Purview.EventSourcing.Aggregates.IAggregate)this).Details;
			details.IsDeleted = true;
		}

		void ApplySystemEvent(Purview.EventSourcing.Aggregates.Events.RestoreEvent @event)
		{
			var details = ((Purview.EventSourcing.Aggregates.IAggregate)this).Details;
			details.IsDeleted = false;
		}

		static void ApplySystemEvent(Purview.EventSourcing.Aggregates.Events.ForceSaveEvent _)
		{
			// This is intentionally left blank.
		}
	}
}
