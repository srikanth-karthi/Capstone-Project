using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using EventManagementApp.Enums;
using EventManagementApp.Repositories;

public class TicketProcessingService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(10);
    private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(5);
    private readonly ILogger<TicketProcessingService> _logger;

    public TicketProcessingService(IServiceScopeFactory serviceScopeFactory, ILogger<TicketProcessingService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_checkInterval, stoppingToken);
            await ProcessPendingTickets(stoppingToken);
        }
    }

    private async Task ProcessPendingTickets(CancellationToken stoppingToken)
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ticketRepo = scope.ServiceProvider.GetRequiredService<ITicketRepository>();
                var eventRepo = scope.ServiceProvider.GetRequiredService<IEventRepository>();

                var pendingTickets = await ticketRepo.GetAll();

                foreach (var ticket in pendingTickets.Where(t => t.PaymentStatus == PaymentStatus.Pending))
                {
                    if (IsTicketExpired(ticket))
                    {
                        await HandlePendingTicket(ticket, eventRepo, ticketRepo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending tickets");
        }
    }

    private bool IsTicketExpired(Tickets ticket)
    {
        return DateTime.Now - ticket.CreatedAt >= _expirationTime;
    }

    private async Task HandlePendingTicket(Tickets ticket, IEventRepository eventRepo, ITicketRepository ticketRepo)
    {
        try
        {
            var eventToUpdate = await eventRepo.GetById(ticket.EventId);
            if (eventToUpdate == null)
            {
                _logger.LogWarning($"Event with ID {ticket.EventId} not found for ticket ID {ticket.TicketId}");
                return;
            }

            eventToUpdate.RemainingTickets += ticket.NumberOfTickets;
            await eventRepo.Update(eventToUpdate);
            await ticketRepo.Delete(ticket);
            _logger.LogInformation($"Handled pending ticket ID {ticket.TicketId}, updated event ID {eventToUpdate.EventId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error handling pending ticket ID {ticket.TicketId}");
        }
    }
}
