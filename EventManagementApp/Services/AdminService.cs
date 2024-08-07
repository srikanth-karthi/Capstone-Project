﻿using System.Collections.Generic;
using EventManagementApp.DTOs;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;

namespace EventManagementApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBlobService _blobService;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IScheduledEventRepository _scheduledEventRepository;
        private readonly IQuotationRequestRepository _quotationRequestRepository;

        public AdminService(
            IEventCategoryRepository eventCategoryRepository,
            IScheduledEventRepository scheduledEventRepository,
            IQuotationRequestRepository quotationRequestRepository,
            IBlobService blobService
            )
        {
            _blobService= blobService;
            _eventCategoryRepository = eventCategoryRepository;
            _scheduledEventRepository = scheduledEventRepository;
            _quotationRequestRepository = quotationRequestRepository;
        }

        public async Task<List<AdminBaseEventCategoryDTO>> GetAllEventCategories()
        {
            List<EventCategory> events = await _eventCategoryRepository.GetAll();
            List<AdminBaseEventCategoryDTO> eventCategoryDTOs = events
                .Select(ec => new AdminBaseEventCategoryDTO
                {
                    EventCategoryId = ec.EventCategoryId,
                    EventName = ec.EventName,
                    Poster=ec.Poster,
                    Description = ec.Description,
                    CreatedDate = ec.CreatedDate,
                    IsActive = ec.IsActive
                })
                .ToList();

            List < AdminBaseEventCategoryDTO > eventCategories = await _eventCategoryRepository.GetAllWithReviews();

            return eventCategories;
        }

        public async Task<EventCategory> CreateEventCategory(CreateEventCategoryDTO eventCategoryDTO)
        {
            EventCategory category = new EventCategory();
            category.EventName = eventCategoryDTO.EventName;
            category.Description = eventCategoryDTO.Description;
            category.Poster = await _blobService.UploadFileAsync(eventCategoryDTO.Poster, eventCategoryDTO.Poster.FileName);
            category.IsService=eventCategoryDTO.IsService;
            category.IsActive = true;
          return   await _eventCategoryRepository.Add(category);
        }

        public async Task<List<AdminScheduledEventListDTO>> GetScheduledEvents()
        {
            List<AdminScheduledEventListDTO> events = await _scheduledEventRepository.GetScheduledEvents();
            return events;
        }

        public async Task<EventCategory> UpdateEventDetails(int id, UpdateEventCategoryDTO updateEventCategoryDTO)
        {
            EventCategory eventCategory = await _eventCategoryRepository.GetById(id);
            if (eventCategory == null)
            {
                throw new NoEventCategoryFoundException();
            }

            if (updateEventCategoryDTO.EventName == null 
                && updateEventCategoryDTO.Description == null
                && updateEventCategoryDTO.IsActive == null
                && updateEventCategoryDTO.Poster== null
                )
            {
                throw new NullReferenceException();
            }

            if (updateEventCategoryDTO.EventName != null)
            {
                eventCategory.EventName = updateEventCategoryDTO.EventName;
            }

            if (updateEventCategoryDTO.Description != null)
            {
                eventCategory.Description = updateEventCategoryDTO.Description;
            }

            if (updateEventCategoryDTO.Poster != null)
            {
                eventCategory.Poster = await _blobService.UploadFileAsync(updateEventCategoryDTO.Poster, updateEventCategoryDTO.Poster.FileName);
            }
            if (updateEventCategoryDTO.IsActive != null)
            {
                eventCategory.IsActive = (bool)updateEventCategoryDTO.IsActive;
            }

            return await _eventCategoryRepository.Update(eventCategory);
        }

        public async Task<List<BasicQuotationRequestDTO>> GetQuotations(bool isNew)
        {
            List<BasicQuotationRequestDTO> requests = await _quotationRequestRepository.GetAllWithEventName(isNew);
            return requests;
        }

        public async Task<BasicQuotationRequestDTO> GetQuotationsByid(int id)
        {
            var requests = await _quotationRequestRepository.GetRequestById(id)?? throw new NoQuotationRequestFoundException();
            
            return requests;
        }
    }
}
