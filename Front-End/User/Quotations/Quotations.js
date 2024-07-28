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

function formatDate(dateString) {
    const options = {
        year: "numeric",
        month: "long",
        day: "numeric",
        hour: "2-digit",
        minute: "2-digit",
    };
    return new Date(dateString).toLocaleDateString(undefined, options);
}

function getStatusClass(status) {
    switch (status.toLowerCase()) {
        case "initiated":
            return "status-initiated";
        case "responded":
            return "status-responded";
        case "accepted":
            return "status-accepted";
        case "completed":
            return "status-completed";
        case "denied":
            return "status-denied";
        default:
            return "";
    }
}

function createQuotationElement(quotation) {
    const div = document.createElement("div");
    div.className = "quotation";
    div.setAttribute("data-request-id", quotation.quotationRequestId);

    const statusClass = getStatusClass(quotation.quotationStatus);

    div.innerHTML = `
    <div>
        <h2>${quotation.eventCategory} <div class="status ${statusClass}">${quotation.quotationStatus}</div></h2>
        <div class="quotation-item">
            <div>
                <p><strong>Start Date:</strong> ${formatDate(quotation.eventStartDate)}</p>
                <p><strong>End Date:</strong> ${formatDate(quotation.eventEndDate)}</p>
                <p><strong>Special Instructions:</strong> ${quotation.specialInstructions}</p>
                ${quotation.foodPreference && quotation.foodPreference !== "NoFood" ? `<p><strong>Catering Instructions:</strong> ${quotation.cateringInstructions ? quotation.cateringInstructions : "Not provided"}</p>` : ""}
            </div>
            <div style="margin-left: auto; margin-right:auto;">
                ${quotation.foodPreference ? `<p><strong>Food Preference:</strong> ${quotation.foodPreference}</p>` : ""}
                <p><strong>Venue Type:</strong> ${quotation.venueType}</p>
                <p><strong>Location Details:</strong> ${quotation.locationDetails}</p>
            </div>
        </div>
    </div>
    <div id="response-details-${quotation.quotationRequestId}" class="response-details"></div>
`;


    const handleClick = () => {
        showResponseDetails(quotation.quotationRequestId);
        div.removeEventListener("click", handleClick); 
    };

    div.addEventListener("click", handleClick);
    return div;
}

async function showResponseDetails(requestId) {
  var  response = await fetchData(`api/user/requests/${requestId}`)
  response=response.quotationResponse

    const responseDetails = document.getElementById(`response-details-${requestId}`);

    if (response) {
        responseDetails.style.display = "block";
        responseDetails.innerHTML = `
            <h2>Admin Response <div class="status ${getStatusClass(response.requestStatus)}">${response.requestStatus}</div></h2>
            <p><strong>Response Message:</strong> ${response.responseMessage}</p>
            ${response.requestStatus === "Accepted" ? `
                <div class="quotation-item">
                    <div>
                        <p><strong>Quoted Amount:</strong> ${response.quotedAmount} ${response.currency}</p>
                    </div>
                </div>
                ${response.clientResponse ? `
                    <h2>Client Response <div class="status ${getStatusClass(response.clientResponse.clientDecision)}">${response.clientResponse.clientDecision}</div></h2>
                    ${response.clientResponse.clientDecision === "Accepted" ? `
${response.clientResponse.isPaid ? `<div class="paid"><img src="../../asserts/paid.png" alt="Paid" width="20">Paid</div>` : `<button type='button'class="paid" onclick='paynow(${response.clientResponse.quotedAmount}, ${response.clientResponse.orderId})'>Pay Now</button>`}

                    ` : ""}
                ` : `
                    <div class="buttons">
                        <button class="accept" onclick="handleClientResponse(${response.quotationResponseId}, 'Accepted',${requestId})">Accept</button>
                        <button class="reject" onclick="handleClientResponse(${response.quotationResponseId}, 'Denied',${requestId})">Reject</button>
                    </div>
                `}
            ` : ""}
        `;

        // Add animation effect
        setTimeout(() => {
            responseDetails.style.opacity = 1;
        }, 100);
    }
}


async function paynow( requestId,orderId) {
  try {
 const order=   await fetchData(`api/Orderpayment/${orderId}`,"POST")

    const options = {
      key: 'rzp_test_kHemgfwkQxlJk2',
      amount: order.amount,
      currency: "INR",
      name: 'Eventify',
      description: 'Book event',
      order_id: order.razorpayOrderId,
      handler: async function (response) {
        try {
          const confirmRes = await fetchData('api/Orderpayment/confirm', "POST", {
            "contentId": order.contentId,
            "paymentId": response.razorpay_payment_id,
            "RazorpayOrderId": response.razorpay_order_id,
            "signature": response.razorpay_signature
          });
          showToast("success", "Success", "Event Booked Successfully");
          showResponseDetails(requestId);
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
   window.paynow=paynow
   window.handleClientResponse=handleClientResponse
async function handleClientResponse(quotationResponseId, decision,requestId) {

    await fetchData("api/user/response","POST", {
      
        "quotationResponseId": quotationResponseId,
        "clientDecision": decision
      }
    )
  showResponseDetails(requestId);
}
document.addEventListener("DOMContentLoaded", async () => {
    const quotations = await fetchData('api/user/requests');
    const quotationsContainer = document.getElementById("quotations");
    quotations.forEach((quotation) => {
        quotationsContainer.appendChild(createQuotationElement(quotation));
    });



});
