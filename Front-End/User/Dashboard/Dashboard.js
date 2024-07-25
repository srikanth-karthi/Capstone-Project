import { showToast } from "../../Pakage/Toster.js";


    if (!localStorage.getItem('authToken')) {
      window.location.href = "/";
    }
    handleAuthId();
    function handleAuthId() {
        const authid = getQueryParam('authid');
        if (authid == 1) {
          showToast('success', 'Success', 'Login Successful.');
          removeQueryParam('authid');
        }
      }
      
      function getQueryParam(param) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(param);
      }
      document.addEventListener("DOMContentLoaded", function () {
        const hamburgerMenu = document.querySelector(".hamburger-menu");
        const navLinks = document.querySelector(".nav-links");
    
        hamburgerMenu.addEventListener("click", function () {
          navLinks.classList.toggle("active");
        });
      });
      function removeQueryParam(param) {
        const url = new URL(window.location);
        const urlParams = new URLSearchParams(url.search);
        urlParams.delete(param);
        url.search = urlParams.toString();
        window.history.replaceState({}, document.title, url.toString());
      }

      document.addEventListener("DOMContentLoaded", () => {
        const events = [
            {
                eventId: 1,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-27T00:00:00",
                isActive: true,
                poster: "../../asserts/background_1.jpg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 27
            },
            {
                eventId: 3,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-28T00:00:00",
                isActive: true,
                poster: "../../asserts/background_2.jpg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 100
            },
            {
                eventId: 4,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-28T00:00:00",
                isActive: true,
                poster: "../../asserts/birthdayParty.jpeg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 100
            },
            {
                eventId: 4,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-28T00:00:00",
                isActive: true,
                poster: "../../asserts/birthdayParty.jpeg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 100
            },
            {
                eventId: 3,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-28T00:00:00",
                isActive: true,
                poster: "../../asserts/background_2.jpg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 100
            },
            {
                eventId: 4,
                eventName: "Summer Music Festival",
                description: "Join us for a day of great music and fun under the sun!",
                createdDate: "2024-07-28T00:00:00",
                isActive: true,
                poster: "../../asserts/birthdayParty.jpeg",
                numberOfTickets: 100,
                ticketCost: 120.00,
                remainingTickets: 100
            }
        ];
    
        const eventList = document.querySelector(".event-list");
        events.forEach(event => {
            const eventItem = document.createElement("div");
            eventItem.classList.add("event-item");
            eventItem.innerHTML = `
                <img src="${event.poster}" alt="${event.eventName}">
                <h3>${event.eventName}</h3>
                <p>${event.description}</p>
                <p>Tickets: $${event.ticketCost} | Remaining: ${event.remainingTickets}</p>
            `;
            eventList.appendChild(eventItem);
        });
    });
    