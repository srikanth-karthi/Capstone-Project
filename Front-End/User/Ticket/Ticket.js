import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

if (!localStorage.getItem("authToken")) {
  window.location.href = "/";
}
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
var tickets ;
document.addEventListener("DOMContentLoaded", async function () {
   tickets = await fetchData("api/Ticket/GetTickets");

  rendertickets(tickets)
  // Attach event listener to all buttons with class "confirmticket"
});


function rendertickets(tickets) {
    const ticketsContainer = document.getElementById("tickets-container");
    ticketsContainer.innerHTML=``
    tickets.forEach((ticketData) => {
      const ticketElement = document.createElement("div");
      ticketElement.classList.add("ticket");
      const qrClass = ticketData.paymentStatus === "Pending" ? 'blurred' : '';
      ticketElement.innerHTML = `
        <div class="stub">
          <div class="top">
            <span class="admit">${ticketData.attendeeName}</span>
            <div class="qr ${qrClass}" id="qrcode-${ticketData.ticketId}"></div>
          </div>
          <div class="number">${ticketData.numberOfTickets}</div>
          <div class="invite">Invite for you</div>
        </div>
        <div class="check">
          ${
            ticketData.paymentStatus === "Pending"
              ? `<button type="button" class="confirmticket" data-ticketid="${ticketData.ticketId}" data-quantity="${ticketData.numberOfTickets}">Pay now</button>`
              : ""
          }
          <div class="big">You're <br> Invited</div>
          <div class="info">
            <section>
              <div class="title">Event</div>
              <div>${ticketData.eventName}</div>
            </section>
            <section>
              <div class="title">Price</div>
              <div>₹ ${ticketData.ticketCost.toFixed(2)}</div>
            </section>
          </div>
        </div>
      `;
  
      ticketsContainer.appendChild(ticketElement);
  console.log(ticketData.ticketId);
      new QRCode(document.getElementById(`qrcode-${ticketData.ticketId}`), {
        text: `localhost:5232/api/ticket/checkin/${ticketData.ticketId}`,
        width: 128,
        height: 128,
      })
    });
    document.querySelectorAll(".confirmticket").forEach((button) => {
      button.addEventListener("click", (event) => {
        const ticketId = event.target.getAttribute("data-ticketid");
        const quantity = event.target.getAttribute("data-quantity");
        handlePayment(ticketId, quantity);
      });
    });
  }

async function handlePayment(ticketId, quantity) {
  try {
    const bookTickets = await fetchData(`api/Ticket/TicketRepayment/${ticketId}`, "POST")
    const options = {
      key: "rzp_test_kHemgfwkQxlJk2",
      amount: bookTickets.amount,
      currency: "INR",
      name: "Eventify",
      description: "Book event",
      order_id: bookTickets.razorpayOrderId,
      handler: async function (response) {
        try {
      await fetchData("api/Ticket/confirm", "POST", {
            contentId: bookTickets.contentId,
            paymentId: response.razorpay_payment_id,
            RazorpayOrderId: response.razorpay_order_id,
            signature: response.razorpay_signature,
          });

          tickets.forEach((ticket) => {
            if (
              ticket.ticketId ==ticketId
            ) {
              ticket.paymentStatus="Completed"
            }
          });
          rendertickets(tickets);
          showToast("success", "Success", "Ticket Confirmed Successfully");
        } catch (error) {
          console.error("Error confirming payment:", error);
        }
      },
      prefill: {
        name: "Test User",
        email: "test.user@example.com",
        contact: "9999999999",
      },
      notes: {
        address: "Test Address",
      },
      theme: {
        color: "#3399cc",
      },
    };
    const rzp1 = new Razorpay(options);
    rzp1.open();
  } catch (error) {
   
      console.error("Error booking tickets:", error);
    
  }
}
