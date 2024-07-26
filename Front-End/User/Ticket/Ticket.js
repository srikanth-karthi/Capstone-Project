import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

if (!localStorage.getItem('authToken')) {
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

document.addEventListener("DOMContentLoaded", async function () {
    const tickets = await fetchData("api/Ticket/GetTickets")

    const ticketsContainer = document.getElementById('tickets-container');

    tickets.forEach(ticketData => {
        const ticketElement = document.createElement('div');
        ticketElement.classList.add('ticket');

        ticketElement.innerHTML = `
            <div class="stub">
                <div class="top">
                    <span class="admit">${ticketData.attendeeName}</span>
                                 <div class="qr" id="qrcode-${ticketData.ticketId}"></div>

                </div>
                <div class="number">${ticketData.numberOfTickets}</div>
                <div class="invite">
                    Invite for you
                </div>
            </div>
            <div class="check">
                <div class="big">
                    You're <br> Invited
                </div>
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

        // Generate QR code
        new QRCode(document.getElementById(`qrcode-${ticketData.ticketId}`), {
            text: `http://localhost:5232/api/ticket/checkin/${ticketData.ticketId}`,
            width: 128,
            height: 128
        });
    });
});
