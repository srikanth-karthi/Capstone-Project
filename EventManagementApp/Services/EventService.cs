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

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<EventDTO>> GetAllEvents()
        {
            var events = await _eventRepository.GetAll();
            return events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                EventName = e.EventName,
                Description = e.Description,
                CreatedDate = e.CreatedDate,
                IsActive = e.IsActive,
                Poster = e.Poster,
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
                             CreatedDate = e.CreatedDate,
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

        public async Task<Event> UpdateEvent(EventDTO eventDto)
        {
            var eventToUpdate = await _eventRepository.GetById(eventDto.EventId?? 0);
            if (eventToUpdate == null)
            {
                throw new EventNotFoundExceptions((int)eventDto.EventId);
            }

            eventToUpdate.EventName = eventDto.EventName;
            eventToUpdate.Description = eventDto.Description;
            eventToUpdate.IsActive = eventDto.IsActive;
            eventToUpdate.Poster = eventDto.Poster;
            eventToUpdate.NumberOfTickets = eventDto.NumberOfTickets;
            eventToUpdate.RemainingTickets = (int)eventDto.RemainingTickets;
            eventToUpdate.TicketCost = eventDto.TicketCost;


            return await _eventRepository.Update(eventToUpdate);
        }

        public async Task<Event> AddEvent(EventDTO eventDto)
        {
            if (IsWeekday(eventDto.CreatedDate) && eventDto.TicketCost > 50)
            {
                throw new InvalidTicketpriceException("For events hosted during non-peak times, charges cannot exceed $50 per ticket.");
            }

            var newEvent = new Event
            {
                EventName = eventDto.EventName,
                Description = eventDto.Description,
                CreatedDate = eventDto.CreatedDate,
                IsActive = eventDto.IsActive,
                Poster = eventDto.Poster,
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

