import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

if (!localStorage.getItem("authToken")) {
  window.location.href = "/";
}
let reviews = [];
let events = [];
var currentEvent;
let services = [];
initialize();

const hamburgerMenu = document.querySelector(".hamburger-menu");
const navLinks = document.querySelector(".nav-links");

hamburgerMenu.addEventListener("click", function () {
  navLinks.classList.toggle("active");
});

window.addEventListener("click", function (event) {
  if (
    !navLinks.contains(event.target) &&
    !hamburgerMenu.contains(event.target)
  ) {
    navLinks.classList.remove("active");
  }
});

async function initialize() {
  events = await fetchData("api/events/Admin");

  services = await fetchData("api/admin/eventCategory");
  loadServices();
  loadEvents();
}

handleAuthId();

function handleAuthId() {
  const authid = getQueryParam("authid");
  if (authid == 1) {
    showToast("success", "Success", "Login Successful.");
    removeQueryParam("authid");
  }
}

function getQueryParam(param) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(param);
}

function removeQueryParam(param) {
  const url = new URL(window.location);
  const urlParams = new URLSearchParams(url.search);
  urlParams.delete(param);
  url.search = urlParams.toString();
  window.history.replaceState({}, document.title, url.toString());
}

function loadEvents() {
  const eventList = document.querySelector(".event-list");
  eventList.innerHTML = "";
  events.forEach((event) => {
    const eventItem = document.createElement("div");
    eventItem.classList.add("event-item");
    eventItem.innerHTML = `
        <img src="${event.poster}" alt="${event.eventName}">
        <h3>${event.eventName}</h3>
      `;

    eventItem.addEventListener("click", () => showEventDetails(event.eventId));

    eventList.appendChild(eventItem);
  });
}

function showEventDetails(eventId) {
  const event = events.find((event) => event.eventId === eventId);
  toggleDisplay("class", "event-container", "block");
  toggleDisplay("class", "main-content", "none");

  document.getElementById("eventTitle").setAttribute("EventId", event.eventId);
  document.querySelector(".event-banner").src = event.poster;
  document.getElementById("eventTitle").innerHTML = `
    ${event.eventName}
    <button type="button" onclick="openEditEventModal(${event.eventId})">
      <img src="../../asserts/edit.svg" alt="Edit">
    </button>
  `;
  document.getElementById("eventDate").innerText = new Date(
    event.eventDate
  ).toLocaleDateString();
  document.getElementById("eventTime").innerText = new Date(
    event.eventDate
  ).toLocaleTimeString();
  document.getElementById(
    "ticketCost"
  ).innerText = `Standard Ticket: ₹ ${event.ticketCost} each`;
  document.getElementById(
    "numberOfTickets"
  ).innerText = `Number of Tickets: ${event.numberOfTickets}`;
  document.getElementById(
    "remainingTickets"
  ).innerText = `Tickets Available: ${event.remainingTickets}`;
  document.getElementById("Booked").innerText = `Tickets Booked: ${
    event.numberOfTickets - event.remainingTickets
  }`;
  document.getElementById("eventHost").innerText = event.description;
  document.getElementById("eventMap").src = event.maplink;
}

function loadServices() {
  const serviceList = document.querySelector(".service-list");
  serviceList.innerHTML = "";
  const seeMoreBtn = document.getElementById("seeMoreBtn");

  services.forEach((service, index) => {
    const serviceItem = document.createElement("div");
    serviceItem.className = "service-item";
    if (index >= 5) serviceItem.classList.add("hidden");

    const img = document.createElement("img");
    img.src = service.poster;
    img.alt = service.eventName;

    const eventName = document.createElement("div");
    eventName.className = "event-name";
    eventName.textContent = service.eventName;

    serviceItem.addEventListener("click", () =>
      showServiceDetails(service.eventCategoryId)
    );

    serviceItem.appendChild(img);
    serviceItem.appendChild(eventName);
    serviceList.appendChild(serviceItem);

    seeMoreBtn.addEventListener("click", () => {
      const hiddenItems = document.querySelectorAll(".service-item.hidden");
      hiddenItems.forEach((item) => item.classList.remove("hidden"));
      seeMoreBtn.style.display = "none";
    });
  });
}

function showServiceDetails(eventCategoryId) {
  const service = services.find(
    (service) => service.eventCategoryId === eventCategoryId
  );

  reviews = service.reviews;
  toggleDisplay("class", "event-category-container", "block");
  toggleDisplay("class", "main-content", "none");

  document.querySelector(".event-category-banner").src = service.poster;
  document.getElementById("event-category-eventTitle").innerHTML = `
    ${service.eventName} (${service.isActive ? "Active" : "Inactive"})
    <button type="button" onclick="openEditEventServiceModal(${
      service.eventCategoryId
    })">
      <img src="../../asserts/edit.svg" alt="Edit">
    </button>
  `;
  document.getElementById("event-category-Description").innerText =
    service.description;
}

function showreviews() {
  if (reviews.length <= 0) {
    showToast("warning", "info", "No Review for the event");
    return;
  }

  const reviewModal = document.getElementById("reviewModal");
  const closeModalBtn = document.querySelector(".review-close");
  const reviewList = document.querySelector(".review-list");

  reviewList.innerHTML = "";
  reviews.forEach((review) => {
    const reviewItem = document.createElement("div");
    reviewItem.classList.add("review-item");
    reviewItem.innerHTML = `
            <div class="review-header">
                <div class="review-user">${review.userName}</div>
                <div class="review-rating">Rating: ${review.rating} ⭐</div>
                </div>
                <div class="review-comments">${review.comments}</div>
            `;
    reviewList.appendChild(reviewItem);
  });
  reviewModal.style.display = "block";

  closeModalBtn.addEventListener("click", function () {
    reviewModal.style.display = "none";
  });
}

function openEditEventModal(eventId) {
  const event = events.find((e) => e.eventId === eventId);
  currentEvent = event;
  document.getElementById("editDescription").value = event.description;
  document.getElementById("editMapLink").value = event.maplink;
  document.getElementById("editTicketCost").value = event.ticketCost;
  document.getElementById("editNumberOfTickets").value = 0;
  document.getElementById("toggle").checked = event.isActive;
  document.getElementById("editEventModal").style.display = "flex";
}

function closeModalEvent() {
  document.getElementById("editEventModal").style.display = "none";
}

function openEditEventServiceModal(eventCategoryId) {
  const service = services.find((e) => e.eventCategoryId === eventCategoryId);
  currentEvent = service;
  document.getElementById("editServiceDescription").value = service.description;
  document.getElementById("toggle toggleService").checked = service.isActive;
  document.getElementById("editEventServiceModal").style.display = "flex";
}

function closeModalServiceEvent() {
  document.getElementById("editEventServiceModal").style.display = "none";
}

async function saveChangesEvent(event) {
  event.preventDefault();

  const updatedTicketCost = document.getElementById("editTicketCost").value;
  let updatedNumberOfTickets = document.getElementById(
    "editNumberOfTickets"
  ).value;
  const updatedDescription = document.getElementById("editDescription").value;
  const updatedMapLink = document.getElementById("editMapLink").value;
  const eventBanner = document.getElementById("eventBanner").files[0];
  const isActive = document.getElementById("toggle").checked;

  if (updatedNumberOfTickets !== "") {
    updatedNumberOfTickets = parseInt(updatedNumberOfTickets, 10);
    if (isNaN(updatedNumberOfTickets) || updatedNumberOfTickets < 0) {
      alert("Please enter a valid number of tickets.");
      return;
    }
  }

  const formData = new FormData();
  formData.append("ticketCost", updatedTicketCost);
  formData.append("AddedTicket", updatedNumberOfTickets);
  formData.append("Description", updatedDescription);
  formData.append("maplink", updatedMapLink);
  formData.append("isActive", isActive);
  if (eventBanner) {
    formData.append("poster", eventBanner);
  }

  const updatedEvent = await fetchData(
    `api/Events/${currentEvent.eventId}`,
    "PUT",
    formData,
    true
  );
  const eventIndex = events.findIndex(
    (e) => e.eventId === currentEvent.eventId
  );
  if (eventIndex !== -1) {
    events[eventIndex] = { ...events[eventIndex], ...updatedEvent };
  }
  showEventDetails(currentEvent.eventId);
  closeModalEvent();
  showToast("success", "Success", "Event Updated Successfully");
}

async function saveChangesService(event) {
  event.preventDefault();

  const description = document.getElementById("editServiceDescription").value;
  const isActive = document.getElementById("toggle toggleService").checked;
  const poster = document.getElementById("editServiceBanner").files[0];

  const formData = new FormData();
  formData.append("description", description);
  formData.append("isActive", isActive);
  if (poster) {
    formData.append("poster", poster);
  }

  const updatedService = await fetchData(
    `api/admin/eventCategory/${currentEvent.eventCategoryId}`,
    "PUT",
    formData,
    true
  );
  const serviceIndex = services.findIndex(
    (e) => e.eventCategoryId === currentEvent.eventCategoryId
  );
  if (serviceIndex !== -1) {
    services[serviceIndex] = { ...services[serviceIndex], ...updatedService };
  }
  showServiceDetails(currentEvent.eventCategoryId);
  closeModalServiceEvent();
  showToast("success", "Success", "Service Updated Successfully");
}

document.getElementById("toggle").addEventListener("change", function () {
  this.setAttribute("aria-checked", this.checked);
});

document.getElementById("backButton").addEventListener("click", function () {
  toggleDisplay("class", "event-container", "none");
  toggleDisplay("class", "main-content", "block");
  loadServices();
  loadEvents();
});

document
  .getElementById("event-category-back-button")
  .addEventListener("click", function () {
    toggleDisplay("class", "event-category-container", "none");
    toggleDisplay("class", "main-content", "block");
    loadServices();
    loadEvents();
  });

function openAddServiceModal() {
  const form = document.getElementById("AddServiceform");

  form.reset();

  toggleDisplay("id", "AddServiceModal", "flex");
}
function closeModalAddService() {
  toggleDisplay("id", "AddServiceModal", "none");
}
function openAddEventModal() {
  const form = document.getElementById("AddEventform");

  form.reset();

  toggleDisplay("id", "AddEventModal", "flex");
}
function closeModalAddEvent() {
  toggleDisplay("id", "AddEventModal", "none");
}

async function saveChangesAddService(event) {
  event.preventDefault();

  const title = document.getElementById("addServiceTitle").value;
  const description = document.getElementById("addServiceDescription").value;
  const isservice = document.getElementById("toggle isservice").checked;
  const bannerFile = document.getElementById("addServiceBanner").files[0];

  if (!title || !description) {
    alert("Please fill in all required fields.");
    return false;
  }

  const formData = new FormData();
  formData.append("EventName", title);
  formData.append("description", description);
  formData.append("IsService", isservice);

  formData.append("Poster", bannerFile);

  const newevent = await fetchData(
    "api/admin/eventCategory",
    "POST",
    formData,
    true
  );
  services.push(newevent);
  showToast("success", "Success", "Service Added Successfully");
  closeModalAddService();
  loadServices();
}
async function saveChangesAddEvent(event) {
  event.preventDefault();

  const title = document.getElementById("addEventTitle").value;
  const description = document.getElementById("addEventDescription").value;
  const mapLink = document.getElementById("addEventMapLink").value;
  const ticketCost = document.getElementById("addEventTicketCost").value;
  const numberOfTickets = document.getElementById("addNumberOfTickets").value;
  const bannerFile = document.getElementById("addEventBanner").files[0];
  const eventDate = document.getElementById("addEventDate").value;

  if (
    !title ||
    !description ||
    !mapLink ||
    !ticketCost ||
    !numberOfTickets ||
    !eventDate
  ) {
    alert("Please fill in all required fields.");
    return false;
  }

  const formData = new FormData();
  formData.append("EventName", title);
  formData.append("description", description);
  formData.append("Maplink", mapLink);
  formData.append("TicketCost", ticketCost);
  formData.append("NumberOfTickets", numberOfTickets);
  formData.append("Poster", bannerFile);
  formData.append("EventDate", eventDate);
  try {
    const newevent = await fetchData("api/Events", "POST", formData, true);
    events.push(newevent);
    showToast("success", "Success", "Event  Added Successfully");
    closeModalAddEvent();
    loadEvents();
  } catch (error) {
    if (error.message.includes("400")) {
      showToast(
        "error",
        "Error",
        "Event Posted on weekdays ticket amount cannot be greater than ₹50"
      );
    }
  }
}
window.showreviews = showreviews;
window.openEditEventModal = openEditEventModal;
window.closeModalAddEvent = closeModalAddEvent;
window.saveChangesEvent = saveChangesEvent;
window.closeModalEvent = closeModalEvent;
window.openEditEventServiceModal = openEditEventServiceModal;
window.saveChangesService = saveChangesService;
window.saveChangesAddEvent = saveChangesAddEvent;
window.closeModalServiceEvent = closeModalServiceEvent;
window.openAddEventModal = openAddEventModal;
window.openAddServiceModal = openAddServiceModal;
window.closeModalAddService = closeModalAddService;
window.saveChangesAddService = saveChangesAddService;
function setMinDateTime() {
  const now = new Date();
  const minDateTime = new Date();
  minDateTime.setDate(now.getDate() + 2);
  const minDateTimeString = minDateTime.toISOString().slice(0, 16);
  document
    .getElementById("addEventDate")
    .setAttribute("min", minDateTimeString);
}

window.onload = setMinDateTime;
