﻿using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasOne(x => x.Participant).WithMany().HasForeignKey(x => x.ParticipantId).IsRequired();
    }
}