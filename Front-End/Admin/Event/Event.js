import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";


if (!localStorage.getItem('authToken')) {
    window.location.href = "/";
}


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

var inst = mobiscroll.eventcalendar("#demo-desktop-month-view", {
  theme: "ios",
  themeVariant: "dark",
  clickToCreate: false,
  dragToCreate: false,
  dragToMove: false,
  dragToResize: false,
  eventDelete: false,
  view: {
    calendar: {    labels: 'all',
      type: 'month' },
  },
  onEventClick: function (args) {
    clickevent(args);
  },
});

function clickevent(args) {

  const clickedEvent = data.find(event => event.scheduledEventId === args.event.eventid);
  showModal(clickedEvent);
}

const data = await fetchData("api/admin/eventCategory/scheduled");

function Rendercalander()
{
const events = data.map(event => {
    const color = event.isCompleted ? '#24d71e' : '#ff0000'; // Green for completed, orange for pending
    return {
      start: new Date(event.eventStartDate).toISOString(),
      end: new Date(event.eventEndDate).toISOString(),
      title: `${event.clientName}'s-${event.eventCategory}`,
      color: color,
      eventid: event.scheduledEventId // Adding ID for event reference
    };
  });
  inst.setEvents(events);
}
Rendercalander()
  function showModal(event) {

  
    const modal = document.getElementById('eventModal');
    document.getElementById('modalTitle').innerText = event.eventCategory;
    
    document.getElementById('modalDetails').innerHTML = `
    <div class="modal-grid">
      <div class="event-items">

      <p><strong>Client Name:</strong> ${event.clientName}</p>
        <p><strong>Venue:</strong> ${event.venueType}</p>
        <p><strong>Location:</strong> ${event.locationDetails}</p>
        <p><strong>Food Preference:</strong> ${event.foodPreference || "Not Provided"}</p>
        <p><strong>Request Date:</strong> ${new Date(event.requestDate).toLocaleString()}</p>
      </div>
      <div class="event-items">
      <p><strong>Client Email:</strong> ${event.clientEmail}</p>

        <p><strong>Catering Instructions:</strong> ${event.cateringInstructions || "Not Provided"}</p>
        <p><strong>Special Instructions:</strong> ${event.specialInstructions || "Not Provided"}</p>
        <p><strong>Start Date:</strong> ${new Date(event.eventStartDate).toLocaleString()}</p>
        <p><strong>End Date:</strong> ${new Date(event.eventEndDate).toLocaleString()}</p>
      </div>
    </div>
  <div class="modal-buttons">
    ${!event.isCompleted ? `<button class="completed" onclick="markAsCompleted(${event.scheduledEventId})">Mark as Completed</button>` : '<button class="completed" >Event Completed</button>'}
  </div>
    `;
    
    modal.style.display = 'block';  modal.classList.add('show');

  }
  

function closeModal() {
  const modal = document.getElementById('eventModal');
  modal.classList.remove('show');
  setTimeout(() => { modal.style.display = 'none'; }, 300);
}

async function markAsCompleted(eventid) {
await fetchData(`api/user/event/${eventid}`,"PUT");
data.forEach((a)=>
{

    if(a.scheduledEventId===eventid)
        a.isCompleted=true;
})
Rendercalander()
closeModal();
 showToast('success','Success','Event Marked as Completed')

}
window.closeModal=closeModal
window.markAsCompleted=markAsCompleted




