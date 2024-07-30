using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagementApp.DTOs;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;

namespace EventManagementApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IBlobService _blobService;

        public EventService(IEventRepository eventRepository, IBlobService blobService)
        {
            _eventRepository = eventRepository;
            _blobService= blobService;
        }

        public async Task<List<EventDTO>> GetAllEvents()
        {
            var events = await _eventRepository.GetAll();
            return events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                EventName = e.EventName,
                Description = e.Description,
                EventDate = e.EventDate,
                IsActive = e.IsActive,
                Poster = e.Poster,
                Maplink= e.Maplink,
                NumberOfTickets = e.NumberOfTickets,
                RemainingTickets = e.RemainingTickets,
                TicketCost = e.TicketCost
            }).ToList();
        }
        public async Task<List<EventDTO>> GetEventsForUser()
        {
            var events = await _eventRepository.GetAll();
            return events.Where(e => e.IsActive == true)
                         .Select(e => new EventDTO
                         {
                             EventId = e.EventId,
                             EventName = e.EventName,
                             Description = e.Description,
                             Maplink=e.Maplink,
                             EventDate = e.EventDate,
                             IsActive = e.IsActive,
                             Poster = e.Poster,
                             NumberOfTickets = e.NumberOfTickets,
                             RemainingTickets = e.RemainingTickets,
                             TicketCost = e.TicketCost
                         }).ToList();
        }
        public async Task<List<TicketDTO>> GetTicketsForEvent(int eventId)
        {
            var tickets = await _eventRepository.GetTicketsForEvent(eventId);
            return tickets;
        }

        public async Task<Event> UpdateEvent(UpdateEventDto eventDto, int eventId)
        {
            var eventToUpdate = await _eventRepository.GetById(eventId);
            if (eventToUpdate == null)
            {
                throw new EventNotFoundExceptions(eventId);
            }

            eventToUpdate.Description = eventDto.Description ?? eventToUpdate.Description;
            eventToUpdate.Maplink = eventDto.Maplink ?? eventToUpdate.Maplink;
            eventToUpdate.IsActive = eventDto.IsActive ?? eventToUpdate.IsActive;

            if (eventDto.Poster != null)
            {
                string posterUrl = await _blobService.UploadFileAsync(eventDto.Poster, eventDto.Poster.FileName);
                eventToUpdate.Poster = posterUrl;
            }

            eventToUpdate.NumberOfTickets += eventDto.AddedTicket ?? 0;
            eventToUpdate.RemainingTickets += eventDto.AddedTicket ?? 0;
            eventToUpdate.TicketCost = eventDto.TicketCost ?? eventToUpdate.TicketCost;

            return await _eventRepository.Update(eventToUpdate);
        }


        public async Task<Event> AddEvent(AddEventDto eventDto)
        {
            if (IsWeekday(eventDto.EventDate) && eventDto.TicketCost > 50)
            {
                throw new InvalidTicketpriceException("For events hosted during non-peak times, charges cannot exceed $50 per ticket.");
            }

            var newEvent = new Event
            {
                EventName = eventDto.EventName,
                Description = eventDto.Description,
                EventDate = eventDto.EventDate,
                Maplink=eventDto.Maplink,
                IsActive=true,
                Poster = await _blobService.UploadFileAsync(eventDto.Poster, eventDto.Poster.FileName),
                NumberOfTickets = eventDto.NumberOfTickets,
                RemainingTickets = eventDto.NumberOfTickets, 
                TicketCost = eventDto.TicketCost
            };


            return await _eventRepository.Add(newEvent);
        }

        private bool IsWeekday(DateTime dateTime)
        {
            return dateTime.DayOfWeek >= DayOfWeek.Monday && dateTime.DayOfWeek <= DayOfWeek.Friday;
        }


    }
}

