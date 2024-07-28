import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

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
    calendar: { labels: true },
  },
  onEventClick: function (args) {
    clickevent(args);
  },
});

function clickevent(args) {
    reviewBox.style.display = 'none';
  const clickedEvent = data.find(event => event.scheduledEventId === args.event.eventid);
  showModal(clickedEvent);
}

const data = await fetchData("api/user/eventCategory");

function Rendercalander()
{
const events = data.map(event => {
    const color = event.isCompleted ? '#24d71e' : '#ff0000'; // Green for completed, orange for pending
    return {
      start: new Date(event.eventStartDate).toISOString(),
      end: new Date(event.eventEndDate).toISOString(),
      title: event.eventCategory,
      color: color,
      eventid: event.scheduledEventId // Adding ID for event reference
    };
  });
  inst.setEvents(events);
}
Rendercalander()
  function showModal(event) {
    console.log(event);
  
    const modal = document.getElementById('eventModal');
    document.getElementById('modalTitle').innerText = event.eventCategory;
    
    document.getElementById('modalDetails').innerHTML = `
    <div class="modal-grid">
      <div class="event-items">
        <p><strong>Venue:</strong> ${event.venueType}</p>
        <p><strong>Location:</strong> ${event.locationDetails}</p>
        <p><strong>Food Preference:</strong> ${event.foodPreference || "Not Provided"}</p>
        <p><strong>Request Date:</strong> ${new Date(event.requestDate).toLocaleString()}</p>
      </div>
      <div class="event-items">
        <p><strong>Catering Instructions:</strong> ${event.cateringInstructions || "Not Provided"}</p>
        <p><strong>Special Instructions:</strong> ${event.specialInstructions || "Not Provided"}</p>
        <p><strong>Start Date:</strong> ${new Date(event.eventStartDate).toLocaleString()}</p>
        <p><strong>End Date:</strong> ${new Date(event.eventEndDate).toLocaleString()}</p>
      </div>
    </div>
  <div class="modal-buttons">
    ${!event.isCompleted ? `<button class="completed" onclick="markAsCompleted(${event.scheduledEventId})">Mark as Completed</button>` : '<button class="completed" >Event Completed</button>'}
    ${!event.isReviewed ? `<button class="review" onclick="showReviewBox(${event.orderId})">Add Review</button>` : '<button class="review">Reviewed</button>'}
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
window.showReviewBox=showReviewBox
window.closeModal=closeModal
window.markAsCompleted=markAsCompleted
function showReviewBox(id) {
  const reviewBox = document.getElementById('reviewBox');
  reviewBox.style.display = 'flex';
  reviewBox.setAttribute("eventid",id)
  document.getElementById("rate-2").checked = true;
  document.getElementById("reviewText").value = "";
}


document.getElementById("submitReviewButton").addEventListener("click", async function() {
  try {
    let eventId = document.getElementById('reviewBox').getAttribute("eventid");
    let eventFound = false;

    data.forEach((a) => {
      if (a.orderId == eventId) {
        eventFound = true;
        if (a.isCompleted) {
        
          a.isReviewed = true;
        } else {
          showToast("warning", "Warning", "Please mark the event as completed");
          throw new Error("Event not completed");
        }
      }
    });

    if (!eventFound) {
      showToast("warning", "Warning", "Event not found");
      return;
    }

    const rating = document.querySelector('input[name="rating"]:checked').value;
    const reviewText = document.getElementById("reviewText").value.trim();

    if (reviewText.length <= 0) {
      showToast("warning", "Warning", "Please add a Review");
      return;
    }

    await fetchData(`api/user/orders/reviews/${eventId}`, "POST", {
      "Comments": reviewText,
      "Rating": rating
    });

    data.forEach((a) => {
      if (a.scheduledEventId === eventId) {
        a.isReviewed = true;
      }
    });





    closeModal();

    showToast("success", "Success", "Review submitted successfully");

  } catch (error) {
    console.error("Error submitting review:", error);
  }
});


