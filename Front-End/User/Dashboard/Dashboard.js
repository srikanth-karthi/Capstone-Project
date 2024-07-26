import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

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

function removeQueryParam(param) {
  const url = new URL(window.location);
  const urlParams = new URLSearchParams(url.search);
  urlParams.delete(param);
  url.search = urlParams.toString();
  window.history.replaceState({}, document.title, url.toString());
}

async function loadEvents() {
  try {
    const events = await fetchData("api/events");
    const eventList = document.querySelector(".event-list");

    events.forEach(event => {
      const eventItem = document.createElement("div");
      eventItem.classList.add("event-item");
      eventItem.innerHTML = `
        <img src="../../asserts/background_1.jpg" alt="${event.eventName}">
        <h3>${event.eventName}</h3>
        <p>${event.description}</p>
      `;

      eventItem.addEventListener('click', () => {
        toggleDisplay("class", "event-container", "block");
        toggleDisplay("class", "main-content", "none");
        document.getElementById("eventTitle").setAttribute("EventId", event.eventId);
        document.querySelector(".event-banner").src = "../../asserts/event.png";
        document.getElementById("eventTitle").innerText = event.eventName;
        document.getElementById("eventDate").innerText = new Date(event.createdDate).toLocaleDateString();
        document.getElementById("eventTime").innerText = new Date(event.createdDate).toLocaleTimeString();
        document.getElementById("ticketCost").innerText = `Standard Ticket: ₹ ${event.ticketCost} each`;
        document.getElementById("eventLocation").innerText = event.description;
        document.getElementById("eventHost").innerText = "Eventify Team";
        document.getElementById("eventMap").src = `https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3770.685459149781!2d72.83350461490263!3d19.053360487102342!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3be7c9434707b907%3A0x2d0bde614b6d82a7!2sBal%20Gandharva%20Rang%20Mandir!5e0!3m2!1sen!2sin!4v1676981222333!5m2!1sen!2sin`;
        ticketPrice = event.ticketCost;
      });

      eventList.appendChild(eventItem);
    });
  } catch (error) {
    console.error("Error loading events:", error);
  }
}

async function loadServices() {
  try {
    const services = await fetchData("api/eventCategory");
    const serviceList = document.querySelector('.service-list');
    const seeMoreBtn = document.getElementById('seeMoreBtn');

    services.forEach((service, index) => {
      const serviceItem = document.createElement('div');
      serviceItem.className = 'service-item';
      if (index >= 5) serviceItem.classList.add('hidden');

      const img = document.createElement('img');
      img.src = '../../asserts/background_1.jpg';
      img.alt = service.eventName;

      const eventName = document.createElement('div');
      eventName.className = 'event-name';
      eventName.textContent = service.eventName;

      serviceItem.addEventListener('click', () => {
        toggleDisplay("class", "event-category-container", "block");
        toggleDisplay("class", "main-content", "none");
        document.getElementById("event-category-eventTitle").setAttribute("EventcategoryId", service.eventCategoryId);
        document.querySelector(".event-category-banner").src = "../../asserts/event.png";
        document.getElementById("event-category-eventTitle").innerText = service.eventName;
        document.getElementById("event-category-Description").innerText = service.description;
      });

      serviceItem.appendChild(img);
      serviceItem.appendChild(eventName);
      serviceList.appendChild(serviceItem);
    });

    seeMoreBtn.addEventListener('click', () => {
      const hiddenItems = document.querySelectorAll('.service-item.hidden');
      hiddenItems.forEach(item => {
        item.classList.remove('hidden');
      });
      seeMoreBtn.style.display = 'none';
    });
  } catch (error) {
    console.error("Error loading services:", error);
  }
}


function calculateTotal() {
  const total = quantity * ticketPrice;
  document.getElementById('book-tickets').textContent = `Pay: ₹ ${total}`;
}

async function handlePayment() {
  try {
    const booktickets = await fetchData("api/Ticket/book", "POST", {
      "numberOfTickets": quantity,
      "eventId": document.getElementById("eventTitle").getAttribute("EventId")
    });

    const options = {
      key: 'rzp_test_kHemgfwkQxlJk2',
      amount: booktickets.amount,
      currency: "INR",
      name: 'Eventify',
      description: 'Book event',
      order_id: booktickets.razorpayOrderId,
      handler: async function (response) {
        try {
          const confirmRes = await fetchData('api/Ticket/confirm', "POST", {
            "contentId": booktickets.contentId,
            "paymentId": response.razorpay_payment_id,
            "RazorpayOrderId": response.razorpay_order_id,
            "signature": response.razorpay_signature
          });
          showToast("success", "Success", "Ticket Booked Successfully");
        } catch (error) {
          console.error("Error confirming payment:", error);
        }
      },
      prefill: {
        name: 'Test User',
        email: 'test.user@example.com',
        contact: '9999999999',
      },
      notes: {
        address: 'Test Address',
      },
      theme: {
        color: '#3399cc',
      },
    };

    const rzp1 = new Razorpay(options);
    rzp1.open();
  } catch (error) {
    console.error("Error handling payment:", error);
  }
}

window.openModal = handlePayment;
window.openeventCategoryModal = openeventCategoryModal;
window.validateForm = validateForm;
window.closeModal = closeModal;
window.toggleCateringDescription = toggleCateringDescription;

document.addEventListener("DOMContentLoaded", async function () {
  toggleDisplay("class", "event-container", "none");

  const hamburgerMenu = document.querySelector(".hamburger-menu");
  const navLinks = document.querySelector(".nav-links");

  hamburgerMenu.addEventListener("click", function () {
    navLinks.classList.toggle("active");
  });

  window.addEventListener("click", function (event) {
    if (!navLinks.contains(event.target) && !hamburgerMenu.contains(event.target)) {
      navLinks.classList.remove("active");
    }
  });

  await loadEvents();
  await loadServices();

  document.getElementById('backButton').addEventListener('click', function () {
    toggleDisplay("class", "event-container", "none");
    toggleDisplay("class", "main-content", "block");
    quantity = 1;
    quantityDisplay.textContent = quantity;
    document.getElementById('book-tickets').textContent = "Book Tickets";
  });
  document.getElementById('event-category-back-button').addEventListener('click', function () {
    toggleDisplay("class", "event-category-container", "none");
    toggleDisplay("class", "main-content", "block");

    document.getElementById('book-tickets').textContent = "Book Tickets";
  });

  document.getElementById('increase').addEventListener('click', function () {
    if (quantity < 5) {
      quantity++;
      quantityDisplay.textContent = quantity;
      calculateTotal();
    }
  });

  document.getElementById('decrease').addEventListener('click', function () {
    if (quantity > 1) {
      quantity--;
      quantityDisplay.textContent = quantity;
      calculateTotal();
    }
  });
});

let quantity = 1;
let ticketPrice = 0;
const quantityDisplay = document.getElementById('quantity');
function openeventCategoryModal()
{
  document.querySelector(".modal-title").innerText= `${document.getElementById("event-category-eventTitle").innerText } Quotation`
  document.getElementById('form').reset();
  document.getElementById('formModal').style.display = 'flex';
  setTimeout(() => {
    document.getElementById('formContent').classList.add('show');
  }, 10);
}

function closeModal() {
  document.getElementById('formContent').classList.remove('show');
  setTimeout(() => {
    document.getElementById('formModal').style.display = 'none';
  }, 300);
}

function toggleCateringDescription() {
  const foodValue = document.getElementById('food').value;
  const cateringDescription = document.querySelector('.catering-description');
  if (foodValue === 'NoFood') {
    cateringDescription.style.display = 'none';
  } else {
    cateringDescription.style.display = 'block';
  }
}

async function validateForm(event) {
  event.preventDefault();
  const startDate = new Date(document.getElementById('start_date').value);
  const endDate = new Date(document.getElementById('end_date').value);
  const today = new Date();
  today.setHours(0, 0, 0, 0);


  if (startDate >= endDate) {
    showToast('warning', 'warning', 'Start date must be before end date.');

    return false;
  }


  if (startDate < today) {
 
    showToast('warning', 'warning', 'Start date cannot be set to  earlier');
    return false;
  }


  const requiredFields = document.querySelectorAll('#form [required]');
  for (let field of requiredFields) {
    field.value = field.value.trim();
    if (!field.value) {
      showToast('warning', 'warning', 'Please fill out all required fields.');
   
      
      return false;
    }
  }


  try {
   await fetchData("api/quotation/request", "POST", {
      "eventCategoryId":   document.getElementById("event-category-eventTitle").getAttribute("EventcategoryId"),
      "venueType": document.getElementById('vunuetype').value,
      "foodPreference": document.getElementById('food').value,
      "locationDetails": document.getElementById('locationDetails').value,
      "cateringInstructions":  document.getElementById('catering_description').value,
      "specialInstructions": document.getElementById('special_description').value,
      "eventStartDate": startDate,
      "eventEndDate": endDate
    });
    showToast('success', 'Success', 'Request Created successfully');
    closeModal()
  }
catch (e) {

}


  return true;
}